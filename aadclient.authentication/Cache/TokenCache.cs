using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aadclient.authentication.Utilities;
using Newtonsoft.Json;

namespace aadclient.authentication.Cache
{
    public class TokenCache
    {
        private const string FileName = "tokenCache.dat";

        public static TokenCache GetCacheFromFile()
        {
            return EncryptedFile.ReadEncryptedFile<TokenCache>(TokenCache.FileName);
        }

        public void SaveCacheToFile()
        {
            EncryptedFile.WriteEncryptedFile(TokenCache.FileName, this);
        }

        public void ClearCache()
        {
            EncryptedFile.DeleteEncryptedFile(TokenCache.FileName);
        }

        public static async Task<TokenCache> InitializeTokenCache()
        {
            var tokenCache = new TokenCache
            {
                Tokens = new Dictionary<string, List<TokenInfo>>()
            };

            await AADUtil.GetTenantIds(tokenCache);

            return tokenCache;
        }

        [JsonProperty]
        public Dictionary<string, List<TokenInfo>> Tokens { get; set; }

        [JsonProperty]
        public string UserId { get; set; }

        public void SetTokenInfo(string tenantId, TokenInfo info)
        {
            if (!this.Tokens.ContainsKey(tenantId))
            {
                this.Tokens.Add(tenantId, new List<TokenInfo>());
            }

            var existingInfo = this.Tokens[tenantId].FirstOrDefault(tokenInfo => tokenInfo.Audience.Equals(info.Audience, StringComparison.InvariantCultureIgnoreCase));

            if (existingInfo != null)
            {
                this.Tokens[tenantId].Remove(existingInfo);
            }

            this.Tokens[tenantId].Add(info);
        }

        public string GetAuthorizationHeader(string tenantId, string audience)
        {
            if (this.Tokens.ContainsKey(tenantId))
            {
                var tokenInfo = this.Tokens[tenantId].FirstOrDefault(info => info.Audience.Equals(audience, StringComparison.InvariantCultureIgnoreCase));
                return string.Format("Bearer {0}", tokenInfo.AccessToken);
            }

            throw new ArgumentException(string.Format("No tokens stored for tenant id '{0}'", tenantId), tenantId);
        }
    }
}
