namespace Khdamatk.Server.Services.Implementations;


public class TokensService(
    IOptions<JwtSetting> jwtSetting,
    Database db,
    UserManager<User> userManager
    ) : ITokensService
{
    private readonly JwtSetting jwtSetting = jwtSetting.Value;
    private readonly Database db = db;
    private readonly UserManager<User> userManager = userManager;
    const int RefreshTokenSize = 64;
    const int RefreshTokenExpiryInDays = 7;

    //generate JWT token
    public async Task<JwtToken> GetJwtToken(User user)
    {

        var (userRoles,userPermissions) = await GetUserPermissions(user);

        var Claims = new List<Claim>
            {
                new (JWTClaimsDefault.ID,Guid.NewGuid().ToString()),
                new (JWTClaimsDefault.UserId,user.Id),
                new (JWTClaimsDefault.Email,user.Email!),
                new (JWTClaimsDefault.UserName,user.UserName!),


            };


        Claims.AddRange(userRoles.Select(role => new Claim(JWTClaimsDefault.Role, role)));
        Claims.AddRange(userPermissions.Select(perm => new Claim(JWTClaimsDefault.Permissions, perm)));



        /*TODO: Serlize the JSon Array to make app can read them
                new (JWTClaimsDefault.Roles,JsonSerializer.Serialize(userRoles),JsonClaimValueTypes.JsonArray),
                new (JWTClaimsDefault.Permissions,JsonSerializer.Serialize(userPermissions),JsonClaimValueTypes.JsonArray)
                */

        var jwtToken = new JwtSecurityToken(
            issuer: jwtSetting.Issuer,
            audience: jwtSetting.Audience,
            claims: Claims,
            expires: DateTime.UtcNow.AddHours(jwtSetting.ExpiryInHours),
            signingCredentials: new SigningCredentials(SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new JwtToken(token, jwtSetting.ExpiryInHours);
    }

    
    public RefreshToken RefreshToken(User user)
    {

        string refreshTokenValue = GenerateRefreshToken();
        var expires = DateTime.UtcNow.AddDays(RefreshTokenExpiryInDays); // configurable


        //Delete The Inactive Refresh Tokens for the user



        var refreshToken = new RefreshTokens
        {
            Token = refreshTokenValue,
            ExpireAt = expires,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            RevokedAt = DateTime.UtcNow.AddDays(value: RefreshTokenExpiryInDays)
        };

        user.RefreshTokens.Add(refreshToken);
        db.SaveChanges();

        return new RefreshToken(refreshTokenValue, expires);
    }


    //validate JWT token
    [Obsolete("No Need to Validate Jwt Token , App will use different Method")]
    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = SymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;


            return jwtToken.Claims.First(x => x.Type == JWTClaimsDefault.UserId).Value;
        }
        catch
        {
            return null;
        }
    }


    #region Helpers Functions

    //SymmetricSecurityKey
    SymmetricSecurityKey SymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey));

    //TODO: Get UserId from token => move to Extension method
    public static string? GetUserIdFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        return jwtToken.Claims.First(x => x.Type == JWTClaimsDefault.UserId).Value;
    }
    

    string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    

    private async Task<(List<string> Roles, List<string> Permissions)> GetUserPermissions(User user)
    {
        var Roles = (await userManager.GetRolesAsync(user)).ToList();
        var Permissions = await (
                from ur in db.UserRoles
                where ur.UserId == user.Id

                join p in db.RoleClaims on ur.RoleId equals p.RoleId
                where p.ClaimType == PermissionsDefault.Type
                select p.ClaimValue!
            )
            .Distinct()
            .ToListAsync();

        return (Roles, Permissions);
    }


    /// <summary>
    /// without user manger its good but not optimal
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    [Obsolete]
    private async Task<(List<string> Roles, List<string> Permissions)> GetUserPermissions1(User user)
    {
        //method 1: using joins
        var resultRaw = await (
            from ur in db.UserRoles
            where ur.UserId == user.Id      // Get roles IDs for the specific user

            join r in db.Roles on ur.RoleId equals r.Id     // Get roles details

            join p in db.RoleClaims on r.Id equals p.RoleId   // Get permission for each role
            where p.ClaimType == PermissionsDefault.Type // Filter only permission claims

            select new
            {
                RoleName = r.Name,
                Permission = p.ClaimValue
            }
            )
            //.Distinct()   // Im so Confident that this is not needed here 
            .ToListAsync();

        (List<string> Roles, List<string> Permissions) =
            (resultRaw.Select(x => x.RoleName!).Distinct().ToList(),
            resultRaw.Select(x => x.Permission!).Distinct().ToList());
        return (Roles, Permissions);
    }

    #endregion
}
