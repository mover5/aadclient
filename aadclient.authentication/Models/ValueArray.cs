using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace aadclient.authentication.Models
{
    public class ValueArray<T>
    {
        [JsonProperty(PropertyName = "value")]
        public T[] Value { get; set; }
    }
}
