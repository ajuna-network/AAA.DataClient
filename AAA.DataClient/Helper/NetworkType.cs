namespace Ajuna.Integration.Helper
{
    public enum NetworkType
    {
        Local = 0,
        Test = 1,
        Rococo = 2,
        Bajun = 3,
        Ajuna = 4,
    }

    public static class Network
    {
        public static string GetUrl(NetworkType networkType)
        {
            switch (networkType)
            {
                case NetworkType.Bajun:
                    //return "wss://rpc-parachain.bajun.network";
                    return "wss://bajun.api.onfinality.io/public-ws";
                default:
                    throw new NotImplementedException($"The {networkType} is currently not implemented!");
            }
        }
    }
}