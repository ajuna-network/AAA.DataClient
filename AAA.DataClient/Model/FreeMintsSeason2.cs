namespace Ajuna.TheOracle.DataClient.Model
{
    public class FreeMintsSeason2
    {
        public long Id { get; set; }
        public int Reward { get; set; }
        public int Rarity { get; set; }
        public int Role { get; set; }
        public string Address { get; set; }
        public string PublicKey { get; set; }
        public bool Proccessed { get; set; }
    }
}