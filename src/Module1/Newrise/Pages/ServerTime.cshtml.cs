using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Newrise.Pages
{
    public class ServerTimeModel : PageModel
    {
        public string Id { get; set; } = "Universal";
        public DateTime Time { get; set; } = DateTime.UtcNow;

        public void OnGet(string id)
        {
			if (id != null) {
				id = id.Replace("_", " ");
				var zone = TimeZoneInfo.FindSystemTimeZoneById(id);
				var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
				Id = id; Time = time;
			}
		}
	}
}
