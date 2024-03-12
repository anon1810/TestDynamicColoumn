using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestDynamicColoumn.Models;

namespace TestDynamicColoumn.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int monthStart, int yearStart, int monthEnd, int yearEnd)
        {

            var fromDate = new DateTime(2023, 1, 1);
            var toDate = new DateTime(2023, 12, 31);

            if (monthStart > 0 && yearStart > 0 && monthEnd > 0 && yearEnd > 0)
            {
                fromDate = new DateTime(yearStart, monthStart, 1);
                toDate = new DateTime(yearEnd, monthEnd, DateTime.DaysInMonth(yearEnd, monthEnd));
            }       

            // สมมติว่าคุณดึงข้อมูลจากฐานข้อมูลมาได้และเก็บใน `dataItems`
            List<DataItem> dataItems = GetDataItems();

            dataItems = dataItems.Where(x => x.Date >= fromDate && x.Date <= toDate)
                .OrderBy(x => x.Date)
                .ToList();

            var viewModel = new DataViewModel
            {
                Headers = GetHeaders(dataItems),
                Data = GetData(dataItems)
            };

            return View(viewModel);
        }

        private List<string> GetHeaders(List<DataItem> dataItems)
        {
            // สร้าง headers ตามเดือนและปีที่มีในข้อมูล
            return dataItems.Select(x => x.Date.ToString("yyyy-MM")).Distinct().OrderBy(x => x).ToList();
        }

        private Dictionary<string, List<int?>> GetData(List<DataItem> dataItems)
        {
            // จัดรูปแบบข้อมูลเพื่อแสดงผลใน View
            var result = new Dictionary<string, List<int?>>();

            var boxIds = dataItems.Select(x => x.BoxID).Distinct().OrderBy(x => x);
            foreach (var boxId in boxIds)
            {
                var resultsForBoxId = dataItems.Where(x => x.BoxID == boxId)
                                                .GroupBy(x => x.Date.ToString("yyyy-MM"))
                                                .ToDictionary(x => x.Key, x => x.Select(y => (int?)y.Result).FirstOrDefault());

                var dataList = new List<int?>();
                foreach (var header in GetHeaders(dataItems))
                {
                    dataList.Add(resultsForBoxId.ContainsKey(header) ? resultsForBoxId[header] : null);
                }

                result.Add(boxId, dataList);
            }

            return result;
        }

        // ต้องการ method สำหรับดึงข้อมูลจากฐานข้อมูล
        private List<DataItem> GetDataItems()
        {
            return new List<DataItem>
    {
        new DataItem { No = 1, BoxID = "001", Date = new DateTime(2023, 1, 1), Result = 14 },
        new DataItem { No = 2, BoxID = "002", Date = new DateTime(2023, 1, 1), Result = 29 },
        new DataItem { No = 3, BoxID = "003", Date = new DateTime(2023, 1, 1), Result = 14 },
        new DataItem { No = 4, BoxID = "004", Date = new DateTime(2023, 1, 1), Result = 18 },
        new DataItem { No = 5, BoxID = "005", Date = new DateTime(2023, 1, 1), Result = 25 },
        new DataItem { No = 6, BoxID = "001", Date = new DateTime(2023, 2, 1), Result = 24 },
        new DataItem { No = 7, BoxID = "002", Date = new DateTime(2023, 2, 1), Result = 39 },
        new DataItem { No = 8, BoxID = "003", Date = new DateTime(2023, 2, 1), Result = 24 },
        new DataItem { No = 9, BoxID = "004", Date = new DateTime(2023, 2, 1), Result = 28 },
        new DataItem { No = 10, BoxID = "005", Date = new DateTime(2023, 2, 1), Result = 15 },
        new DataItem { No = 11, BoxID = "001", Date = new DateTime(2023, 3, 1), Result = 24 },
        new DataItem { No = 12, BoxID = "002", Date = new DateTime(2023, 3, 1), Result = 39 },
        new DataItem { No = 13, BoxID = "003", Date = new DateTime(2023, 3, 1), Result = 24 },
        new DataItem { No = 14, BoxID = "001", Date = new DateTime(2023, 4, 1), Result = 28 },
        new DataItem { No = 15, BoxID = "002", Date = new DateTime(2023, 4, 1), Result = 15 },
        // ตัวอย่างข้อมูลสำหรับปี 2024
        new DataItem { No = 102, BoxID = "001", Date = new DateTime(2024, 1, 23), Result = 34 },
        new DataItem { No = 103, BoxID = "002", Date = new DateTime(2024, 1, 23), Result = 49 },
        new DataItem { No = 104, BoxID = "003", Date = new DateTime(2024, 1, 23), Result = 24 },
        new DataItem { No = 105, BoxID = "004", Date = new DateTime(2024, 1, 23), Result = 28 },
        new DataItem { No = 106, BoxID = "005", Date = new DateTime(2024, 1, 23), Result = 45 },

        new DataItem { No = 107, BoxID = "006", Date = new DateTime(2024, 2, 1), Result = 48 },
        new DataItem { No = 108, BoxID = "007", Date = new DateTime(2024, 2, 1), Result = 15 },
    };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}