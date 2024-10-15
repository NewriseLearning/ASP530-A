using Microsoft.AspNetCore.Mvc;
using Newrise.Services.Models;

namespace Newrise.Controllers {
	public class HomeController : Controller {
		
		public IActionResult Index(string id) {
			if (id != null) {
				id = id.Replace("_", " ");
				var zone = TimeZoneInfo.FindSystemTimeZoneById(id);
				var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
				var data = new ServerTimeModel(id, time);
				//	return Json(data);
				return View(data);
			}
			return Content(
				"<h3>No time zone specified.</h3>",
				"text/html");

			//	return Content("Hello!");
			//	return Content("<message>Hello!</message>", "text/xml");
			//	return Content("<h1>Hello!</h1>", "text/html");
			//	return Json(new { var1 = 123, var2 = "hello! " });
			//	return View();
		}

	}
}
