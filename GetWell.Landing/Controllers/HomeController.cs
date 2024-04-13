using GetWell.Landing.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GetWell.Landing.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpGet("terms")]
        public IActionResult Terms()
		{
			return View();
		}
        
        [HttpGet("ios")]
        public IActionResult Ios()
        {
            return Redirect("https://apps.apple.com/us/app/getwell-uz/id1641445115");
        }
        
        [HttpGet("android")]
        public IActionResult Android()
		{
            return Redirect("https://play.google.com/store/apps/details?id=com.alphacentauri.getwell");
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}