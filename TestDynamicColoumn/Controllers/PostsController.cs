using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestDynamicColoumn.Controllers
{
    public class PostsController : Controller
    {
        private readonly HttpClient _client;

        public PostsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new System.Uri("https://jsonplaceholder.typicode.com/");
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await _client.GetAsync("posts");

            if (response.IsSuccessStatusCode)
            {
                var posts = await response.Content.ReadAsStringAsync();
                var postsList = JsonConvert.DeserializeObject<List<Post>>(posts);
                return View(postsList);
            }
            else
            {
                // Handle error
                return Content("Error occurred while fetching posts.");
            }
        }
    }

    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
