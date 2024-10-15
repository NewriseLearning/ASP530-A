using Microsoft.AspNetCore.Builder;
using Newrise.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newrise.Services {
	public static class Extensions {
		public static IApplicationBuilder UseServerTime(this IApplicationBuilder app) {
			app.UseMiddleware<ServerTimeMiddleware>();
			return app;
		}
	}
}
