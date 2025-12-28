namespace Khdamatk.Server.Statics.Settings;

public class JwtSetting
{
    [Required]
    public string Issuer { get; set; } = null!;
    [Required]
    public string Audience { get; set; } = null!;
    [Required]
    public string SecretKey { get; set; } = null!;
    [Required]
    public int ExpiryInHours { get; set; }

}