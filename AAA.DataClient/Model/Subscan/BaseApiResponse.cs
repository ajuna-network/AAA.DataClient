using System.Text.Json.Serialization;

namespace AAA.DataClient.Model.Subscan
{
    public class BaseApiResponse
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("generated_at")]
        public long GeneratedAtUnixTime { get; set; }

        [JsonIgnore]
        public DateTime GeneratedAt
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(GeneratedAtUnixTime).DateTime;
            }
        }
    }
}