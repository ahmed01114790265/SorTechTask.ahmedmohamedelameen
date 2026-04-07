using Newtonsoft.Json;

namespace SorTechTask.ahmedmohamedelameen.Models
{
    public class IPLookupResponse
    {
        public string Ip { get; set; }

        [JsonProperty("country_code2")] 
        public string Country_Code { get; set; }

        [JsonProperty("country_name")]
        public string Country_Name { get; set; }

        [JsonProperty("isp")] 
        public string Org { get; set; }
    }
}
