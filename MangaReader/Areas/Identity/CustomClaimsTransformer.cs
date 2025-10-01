using MangaReader.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MangaReader.Areas.Identity
{
    public class CustomClaimsTransformer : IClaimsTransformation
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public CustomClaimsTransformer(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var clone = principal.Clone();

            if (clone.Identity is ClaimsIdentity identity)
            {
                User? user = await _userManager.GetUserAsync(principal);
                if (user != null)
                {
                    var oldRoles = identity.FindAll(identity.RoleClaimType).ToList();
                    foreach (var oldRole in oldRoles)
                    {
                        identity.RemoveClaim(oldRole);
                    }

                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        identity.AddClaim(new Claim(identity.RoleClaimType, role));
                    }

                    string? firstRoleName = roles.FirstOrDefault();

                    if (firstRoleName != null) {
                        Role? roleEntity = await _roleManager.FindByNameAsync(firstRoleName);
                        if (roleEntity != null)
                        {
                            identity.AddClaim(new Claim("RoleDisplayName", roleEntity.DisplayName));
                        }
                    }

                    if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
                    {
                        identity.AddClaim(new Claim("FullName", $"{user.FirstName} {user.LastName}"));
                    }

                    if (!string.IsNullOrEmpty(user.Avatar))
                    {
                        identity.AddClaim(new Claim("Avatar", user.Avatar));
                    }
                }
            }

            return clone;
        }
    }
}
