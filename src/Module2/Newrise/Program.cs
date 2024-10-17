using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Newrise.Components;
using Newrise.Models;
using Newrise.Services;

namespace Newrise {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);

			var connectionString = builder.Configuration.GetConnectionString("NewriseDb");
			builder.Services.AddDbContextFactory<NewriseDbContext>(
				options => options.UseSqlServer(connectionString));

			builder.Services.AddSingleton<EventDataService>();

			builder.Services.AddMemoryCache();

			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();

			// Add MudBlazor UI services
			builder.Services.AddMudServices();

			builder.Services.AddSingleton<OfficeListProvider>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment()) {
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();

			app.Run();
		}
	}
}
