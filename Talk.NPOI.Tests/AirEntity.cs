namespace Talk.NPOI.Tests
{
    public class AirEntity
    {
        [Alias("*起运空港")]
        public string Shipment { get; set; }
        [Alias("*目标空港")]
        public string Destination { get; set; }
        [Alias("*开航日期")]
        public string DateOfSailing { get; set; }
        [Alias("*币制")]
        public string Monetary { get; set; }
    }
}
