namespace TestDynamicColoumn.Models
{
    public class BoxDataViewModel
    {
        public string BoxID { get; set; }
        public List<KeyValuePair<DateTime, int>> DateResults { get; set; } = new List<KeyValuePair<DateTime, int>>();
    }
}
