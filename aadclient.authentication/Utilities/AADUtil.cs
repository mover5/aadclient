using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace aadclient.authentication.Utilities
{
    public static class AADUtil
    {
        public const string AADCommonTenant = "common";
        public const string AADClientId = "1950a258-227b-4e31-a9cf-717495945fc2";
        public const string AADRedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        public const string AADAuthority = "https://login.microsoftonline.com";

        public const string ArmAudience = "https://management.azure.com/";
        public const string GraphAudience = "https://graph.windows.net/";

        public static async Task<string[]> GetTenantIds(Cache.TokenCache cache)
        {
            var authResult = await AADUtil.GetAuthenticationResult();

            cache.SetTokenInfo(AADUtil.AADCommonTenant, new Cache.TokenInfo
            {
                AccessToken = authResult.AccessToken,
                Audience = AADUtil.ArmAudience,
                TenantDisplayName = "Common"
            });
            cache.UserId = authResult.UserInfo == null ? null : authResult.UserInfo.DisplayableId;

            return null;
        }

        public static Task<AuthenticationResult> GetAuthenticationResult(
            string tenantId = AADUtil.AADCommonTenant,
            string audience = AADUtil.ArmAudience,
            string userId = null)
        {
            var authority = string.Format("{0}/{1}", AADUtil.AADAuthority, tenantId);
            var authContext = new AuthenticationContext(
                authority: authority,
                validateAuthority: true);

            if (!string.IsNullOrEmpty(userId))
            {
                return authContext.AcquireTokenAsync(
                    resource: audience,
                    clientId: AADUtil.AADClientId,
                    redirectUri: new Uri(AADUtil.AADRedirectUri),
                    parameters: new PlatformParameters(PromptBehavior.Never),
                    userId: new UserIdentifier(userId, UserIdentifierType.OptionalDisplayableId));
            }
            else
            {
                return authContext.AcquireTokenAsync(
                    resource: audience,
                    clientId: AADUtil.AADClientId,
                    redirectUri: new Uri(AADUtil.AADRedirectUri),
                    parameters: new PlatformParameters(PromptBehavior.Always));
            }
                
        }
    }
}
