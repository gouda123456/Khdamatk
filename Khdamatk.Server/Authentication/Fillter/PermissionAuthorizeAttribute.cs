using Microsoft.AspNetCore.Authorization;

namespace Khdamatk.Server.Authentication.Fillter;


//Requirements => Done
//AuthorizeAttribute ===> Done
//AuthorizationHandler
//PolicyProviderHandler

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}


public class PermissionAuthorizeAttribute(string permission) : AuthorizeAttribute(permission)
{
}


public class PermissionAuthorizeHandler : AuthorizationHandler<PermissionRequirement>
{
    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {

        //check if user is not null
        //check if user is Authenticated
        // check if user have claim with type permission and value requirement.Permission

        if (context.User != null &&
            context.User.Identity is { IsAuthenticated: true } &&
            context.User.HasClaim(c => c.Type == PermissionsDefault.Type && c.Value == requirement.Permission))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return;
    }
}

public class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    private readonly AuthorizationOptions authorizationOptions = options.Value;

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy == null)
        {
            var newPolicy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
            
            // Add the new policy to the authorization options
            authorizationOptions.AddPolicy(policyName, newPolicy);
            return newPolicy;
        }
        return policy;
    }
}