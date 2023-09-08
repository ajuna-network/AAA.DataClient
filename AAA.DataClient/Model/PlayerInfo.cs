using Substrate.NetApi.Model.Types.Primitive;
using Ajuna.TheOracle.DataClient.Model.Avatar;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.account;
using Ajuna.Integration.Model.Avatar;

namespace Ajuna.TheOracle.DataClient.Model
{
    public class PlayerSeasonInfo
    {
        public uint Mint { get; set; }
        public uint FirstMint { get; set; }
        public uint LastMint { get; set; }

        public uint Forge { get; set; }
        public uint FirstForge { get; set; }
        public uint LastForge { get; set; }

        public uint Bought { get; set; }
        public uint Sold { get; set; }
    }

    public class PlayerInfo
    {
        public int FreeMints { get; set; }
        public List<AvatarInfo>? AvatarInfos { get; set; }
        public Dictionary<int, PlayerSeasonInfo> SeasonInfos { get; set; }
    }
}