using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newrise.Models;
using System.Security.Cryptography;
using System.Text;

namespace Newrise.Services {
	public class ParticipantDataService {
		readonly IDbContextFactory<NewriseDbContext> _dcFactory;
		readonly CustomAuthenticationStateProvider _authenticationStateProvider;

		public ParticipantDataService(
			IDbContextFactory<NewriseDbContext> dcFactory,
			AuthenticationStateProvider authenticateStateProvider) {
			_dcFactory = dcFactory;
			_authenticationStateProvider =
				(CustomAuthenticationStateProvider)
					authenticateStateProvider;
		}

		const string PASSWORD_SALT = "$_{0}@newrise_921";
		public string HashPassword(string password) {
			var ha = SHA256.Create();
			password = string.Format(PASSWORD_SALT, password);
			var hashedPassword = ha.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashedPassword);
		}

		public async Task InitializeAsync() {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				if (await dc.Participants.FindAsync("admin") == null) {
					var user = new Participant {
						Id = "admin",
						Name = "Administrator",
						Email = "symbolicon@live.com",
						Company = "Newrise Learning",
						Position = "Web Administrator",
						Password = HashPassword("P@ssword"),
						IsAdmin = true
					};
					await dc.Participants.AddAsync(user);
					await dc.SaveChangesAsync();
				}
			}
		}

		public async Task SignInAsync(string userId, string password) {
			var hashedPassword = HashPassword(password);
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var user = dc.Participants.FirstOrDefault(
					p => (p.Id == userId || p.Email == userId) &&
					p.Password == hashedPassword);
				if (user == null) throw new Exception("Invalid user ID or password.");
				//	if (user.IsLocked) throw new Exception("Account has been locked.");
				var userSession = new UserSession {
					UserId = user.Id,
					IsAdmin = user.IsAdmin
				};
				await _authenticationStateProvider.UpdateAuthenticationState(userSession);
			}
		}

		public async Task SignOutAsync() {
			await _authenticationStateProvider.UpdateAuthenticationState(null);
		}

        public async Task<Participant> GetParticipantAsync(string id) {
            using (var dc = await _dcFactory.CreateDbContextAsync())
                return dc.Participants.FirstOrDefault(p => p.Id == id || p.Email == id);
        }

        public async Task<Participant> GetCurrentParticipantAsync() {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (state != null) return await GetParticipantAsync(state.User.Identity.Name);
            return null;
        }

		public async Task AddParticipantAsync(Participant participant) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				if (await dc.Participants.FirstOrDefaultAsync(
				p => p.Id == participant.Id) != null)
					throw new Exception("User ID already taken. Use another ID.");
				if (await dc.Participants.FirstOrDefaultAsync(
				p => p.Email == participant.Email) != null)
					throw new Exception("Email already registered. Use another email.");
				participant.Password = HashPassword(participant.Password);
				await dc.Participants.AddAsync(participant);
				await dc.SaveChangesAsync();
			}
		}
	}
}
