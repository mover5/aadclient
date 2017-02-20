using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aadclient.authentication.Cache
{
    public class TokenInfo
    {
        public string Audience { get; set; }

        public string TenantDisplayName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
