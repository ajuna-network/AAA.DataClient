using System.Text.Json.Serialization;

namespace AAA.DataClient.Model.Subscan
{
    public class AccountDisplay
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
    }
}