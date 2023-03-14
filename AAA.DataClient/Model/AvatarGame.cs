using System.Numerics;

namespace Ajuna.TheOracle.DataClient.Model
{
    public class AvatarGame
    {
        public DateTime Created { get; set; }

        public Dictionary<int, double> SeasonTreasuries { get; set; }
        
        public Dictionary<string, PlayerInfo> Players { get; set; }
    }
}