using System.Text.Json.Serialization;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.avatar.rarity_tier;
using Substrate.Bajun.NET.NetApiExt.Generated.Model.pallet_ajuna_awesome_avatars.types.season;

namespace Ajuna.TheOracle.DataClient.Model.Avatar
{
    public class AvatarComponent
    {
        public int Index { get; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RarityTier RarityTier { get; }

        public int Variation { get; }

        public AvatarComponent(int index, RarityTier rarityTier, int variation)
        {
            Index = index;

            RarityTier = rarityTier;

            Variation = variation;
        }
    }
}