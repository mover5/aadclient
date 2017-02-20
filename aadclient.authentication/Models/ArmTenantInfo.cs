using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace aadclient.authentication.Models
{
    public class ArmTenantInfo
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; }
    }
}
