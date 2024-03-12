namespace TestDynamicColoumn.Models
{
    public class DataViewModel
    {
        public List<string> Headers { get; set; }
        public Dictionary<string, List<int?>> Data { get; set; }
    }
}
