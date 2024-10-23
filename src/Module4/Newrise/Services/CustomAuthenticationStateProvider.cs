using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newrise.Models;
using System.Security.Claims;

namespace Newrise.Services {

	public class CustomAuthenticationStateProvider : AuthenticationStateProvider {
		private readonly ProtectedSessionStorage _sessionStorage;
		private readonly ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

		public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage) {
			_sessionStorage = sessionStorage;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync() {
			try {
				var result = await _sessionStorage.GetAsync<UserSession>("UserSession");
				var userSession = result.Success ? result.Value : null;
				if (userSession == null) return await Task.FromResult
						(new AuthenticationState(_anonymous));
				var claims = new List<Claim>();
				claims.Add(new Claim(ClaimTypes.Name, userSession.UserId));
				if (userSession.IsAdmin) claims.Add(new Claim(ClaimTypes.Role, "admin"));
				var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuthentication"));
				return await Task.FromResult(new AuthenticationState(principal));
			}
			catch {
				return await Task.FromResult(new AuthenticationState(_anonymous));
			}
		}

		public async Task UpdateAuthenticationState(UserSession userSession) {
			if (userSession != null) await _sessionStorage.SetAsync("UserSession", userSession);
			else await _sessionStorage.DeleteAsync("UserSession");
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}
	}
}
