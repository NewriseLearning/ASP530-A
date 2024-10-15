using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newrise.Services.Services {
	public class ServerTimeMiddleware {
		private readonly RequestDelegate _next;

		public ServerTimeMiddleware(RequestDelegate next) {
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context) {
			var id = context.Request.Query["timezone"];
			if (id.Count > 0) {
			//	var zone_id = id[0] is null ? "" : id[0];
				var zone_id = id[0] ?? "";
				var zone = TimeZoneInfo.FindSystemTimeZoneById(zone_id);
				var time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zone);
				context.Response.ContentType = "text/xml";
				await context.Response.WriteAsync(
					"<?xml version='1.0' encoding='utf-8' ?>" +
					$"<time zone='{zone.Id}'>{time}</time>");

			} else await _next(context);
		}

	}
}
