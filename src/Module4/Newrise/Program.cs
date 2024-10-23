using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Newrise.Components;
using Newrise.Models;
using Newrise.Services;

namespace Newrise {
	public class Program {
		public static void Main(string[] args) {
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddScoped<ProtectedSessionStorage>();

			builder.Services.AddScoped<AuthenticationStateProvider,
				CustomAuthenticationStateProvider>();

			builder.Services.AddAuthentication();
			builder.Services.AddCascadingAuthenticationState();

			//builder.Services.AddAuthentication(
			//	NegotiateDefaults.AuthenticationScheme).AddNegotiate();

			//builder.Services.AddAuthorization(
			//options => options.FallbackPolicy = options.DefaultPolicy);



			var connectionString = builder.Configuration.GetConnectionString("NewriseDb");
			builder.Services.AddDbContextFactory<NewriseDbContext>(
				options => options.UseSqlServer(connectionString));

			builder.Services.AddSingleton<EventDataService>();
			builder.Services.AddScoped<ParticipantDataService>();

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

			app.UseAuthentication();
			app.UseAuthorization();

			app.Run();
		}
	}
}
