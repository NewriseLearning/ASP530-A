using Microsoft.AspNetCore.Components.Forms;
using Newrise.Components;
using Newrise.Services;
using Newrise.Services.Services;

namespace Newrise {
	static class Program {
		//	static async Task Main(string[] args) {
		static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();
			builder.Services.AddRazorComponents().AddInteractiveServerComponents();
			var app = builder.Build();

			//app.Use(async (context, next) => {
			//	if (context.Request.Query["local"].Count > 0) {
			//		context.Response.ContentType = "text/html";
			//		await context.Response.WriteAsync(
			//			"<fieldset><legend><strong>Local Time</strong></legend>" +
			//			$"<div>{DateTime.Now}</div></fieldset>"
			//		);
			//	}
			//	else await next();
			//      });

			//app.Use(async (context, next) => {
			//	if (context.Request.Query["world"].Count > 0) {
			//		context.Response.ContentType = "text/html";
			//		await context.Response.WriteAsync(
			//			"<fieldset><legend><strong>World Time</strong></legend>" +
			//			$"<div>{DateTime.UtcNow}</div></fieldset>"
			//		);
			//	}
			//	else await next();
			//});

			//if (app.Environment.IsDevelopment()) {
			//	app.UseDeveloperExceptionPage();
			//}
			//else {
			//	app.UseHsts();
			//}

			if (!app.Environment.IsDevelopment()) {
				app.UseExceptionHandler("/Error");
				app.UseHsts();  // use HTTPS only (browser remembers up to 30 days)
			}
			app.UseHttpsRedirection();
			//	app.UseRouting();
			app.UseStaticFiles();
			app.UseAntiforgery();

			app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
			app.MapRazorPages();
			app.MapControllerRoute("default",
				"{controller=Home}/{action=Index}/{id?}");

			//app.UseEndpoints(endpoints => {
			//	endpoints.MapGet("/", async context =>
			//		await context.Response.WriteAsync("<h1>Home Page</h1>"));

			//	endpoints.MapGet("/about", async context =>
			//		await context.Response.WriteAsync("<h1>About Page</h1>"));

			//	endpoints.MapGet("/contact", async context =>
			//		await context.Response.WriteAsync("<h1>Contact Page</h1>"));
			//});

			// app.UseServerTime();
			// app.UseWelcomePage();


			//	await app.RunAsync();
			app.Run();
		}
	}
}
