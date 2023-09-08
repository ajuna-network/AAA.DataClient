using System.Text.Json.Serialization;
using Substrate.NetApi.Model.Types;
using Newtonsoft.Json;

namespace AAA.DataClient.Model.Subscan
{
    public class ApiAccounts : BaseApiResponse
    {
        [JsonPropertyName("data")]
        public AccountData Data { get; set; }
    }

    public class AccountData
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("list")]
        public Account[] List { get; set; }
    }

    public class Account
    {
        [JsonProperty("account_display")]
        public AccountDisplay AccountDisplay { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("balance")]
        public string Balance { get; set; }

        [JsonProperty("balance_lock")]
        public string BalanceLock { get; set; }

        [JsonProperty("count_extrinsic")]
        public int CountExtrinsic { get; set; }

        [JsonProperty("derive_token")]
        public object DeriveToken { get; set; }

        [JsonProperty("evm_account")]
        public string EvmAccount { get; set; }

        [JsonProperty("is_evm_contract")]
        public bool IsEvmContract { get; set; }

        [JsonProperty("lock")]
        public string Lock { get; set; }

        [JsonProperty("registrar_info")]
        public object RegistrarInfo { get; set; }

        [JsonProperty("substrate_account")]
        public object SubstrateAccount { get; set; }
    }

}