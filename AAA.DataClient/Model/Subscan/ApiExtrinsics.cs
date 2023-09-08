using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace AAA.DataClient.Model.Subscan
{
    public class ApiExtrinsics : BaseApiResponse
    {
        [JsonPropertyName("data")]
        public ExtrinsicsData Data { get; set; }
    }


    public class ExtrinsicsData
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("extrinsics")]
        public Extrinsic[] Extrinsics { get; set; }
    }

    public class Extrinsic
    {
        [JsonProperty("block_timestamp")]
        public int block_timestamp { get; set; }

        [JsonProperty("block_num")]
        public int block_num { get; set; }

        [JsonProperty("extrinsic_index")]
        public string extrinsic_index { get; set; }

        [JsonProperty("call_module_function")]
        public string call_module_function { get; set; }

        [JsonProperty("call_module")]
        public string call_module { get; set; }

        [JsonProperty("params")]
        public string Params { get; set; }

        [JsonProperty("account_id")]
        public string account_id { get; set; }

        [JsonProperty("account_index")]
        public string account_index { get; set; }

        [JsonProperty("signature")]
        public string signature { get; set; }

        [JsonProperty("nonce")]
        public int nonce { get; set; }

        [JsonProperty("extrinsic_hash")]
        public string extrinsic_hash { get; set; }

        [JsonProperty("success")]
        public bool success { get; set; }

        [JsonProperty("fee")]
        public string fee { get; set; }

        [JsonProperty("fee_used")]
        public string fee_used { get; set; }

        [JsonProperty("from_hex")]
        public string from_hex { get; set; }

        [JsonProperty("finalized")]
        public bool finalized { get; set; }

        [JsonProperty("account_display")]
        public AccountDisplay account_display { get; set; }
    }


    public class Params
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("type_name")]
        public string type_name { get; set; }

        [JsonProperty("value")]
        public object value { get; set; }
    }

    public class ParamsExt
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("type_name")]
        public string type_name { get; set; }

        [JsonProperty("value")]
        public List<Call> value { get; set; }
    }

    public class Call
    {
        [JsonProperty("call_index")]
        public string call_index { get; set; }


        [JsonProperty("call_module")]
        public string call_module { get; set; }

        [JsonProperty("call_name")]
        public string call_name { get; set; }

        [JsonProperty("params")]
        public Param[] @params { get; set; }
    }

    public class Param
    {
        public string name { get; set; }
        public string type { get; set; }
        public object value { get; set; }
    }

}