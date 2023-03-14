using Ajuna.NetApi.Model.Types.Primitive;
using Ajuna.TheOracle.DataClient.Model.Avatar;
using Bajun.Network.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.account;

namespace Ajuna.TheOracle.DataClient.Model
{
    public class PlayerInfo
    {
        public int FreeMints { get; set; }
        public MintStats MintStats { get; set; }
        public ForgeStats ForgeStats { get; set; }
        public MarketStats MarketStats { get; set; }
        public CurrentSeasonStats CurrentSeasonStats { get; set; }
        public int StorageTier { get; set; }
        public List<AvatarInfo>? AvatarInfos { get; set; }
    }
}