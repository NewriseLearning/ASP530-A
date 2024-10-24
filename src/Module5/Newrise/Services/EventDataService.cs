using Microsoft.EntityFrameworkCore;
using Newrise.Models;
using System.Data;

namespace Newrise.Services {
	public class EventDataService {
		readonly IDbContextFactory<NewriseDbContext> _dcFactory;

		public EventDataService(IDbContextFactory<NewriseDbContext> dcFactory) {
			_dcFactory = dcFactory;
		}

		public async Task<bool> HasParticipantAsync(string eventId, string participantId) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var eventItem = await dc.Events.Include(e => e.Participants).SingleOrDefaultAsync(e => e.Id == eventId);
				if (eventItem != null) return eventItem.Participants.SingleOrDefault(p => p.Id == participantId) != null;
				return false;
			}
		}

		public async Task AddParticipantAsync(string eventId, string participantId) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var transaction = await dc.Database.BeginTransactionAsync(
				IsolationLevel.RepeatableRead);
				try {
					var eventItem = await dc.Events.Include(e => e.Participants)
					.SingleOrDefaultAsync(e => e.Id == eventId);
					if (eventItem == null) throw new Exception($"Event '{eventId}' does not exist.");
					if (eventItem.Participants.SingleOrDefault(p => p.Id == participantId) != null)
						throw new Exception($"User '{participantId}' is already a participant of event '{eventId}.");
				if (eventItem.RemainingSeats == 0)
						throw new Exception($"Event '{eventId}' is full.");
					var participant = await dc.Participants.FindAsync(participantId);
					if (participant == null) throw new Exception(
					$"Participant '{participantId}' does not exist.");
					eventItem.Participants.Add(participant);
					eventItem.AllocatedSeats++;
					await dc.SaveChangesAsync();
					await transaction.CommitAsync();
				}
				catch (Exception) { await transaction.RollbackAsync(); throw; }
			}
		}

		public async Task RemoveParticipantAsync(string eventId, string participantId) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var transaction = await dc.Database.BeginTransactionAsync(
				IsolationLevel.RepeatableRead);
				try {
					var eventItem = await dc.Events.Include(e => e.Participants)
					.SingleOrDefaultAsync(e => e.Id == eventId);
					if (eventItem == null) throw new Exception($"Event '{eventId}' does not exist.");
					var participant = eventItem.Participants.SingleOrDefault(
					p => p.Id == participantId);
					if (participant == null) throw new Exception(
					$"User '{participantId}' is not a participant of event '{eventId}.");
					eventItem.Participants.Remove(participant);
					eventItem.AllocatedSeats--;
					await dc.SaveChangesAsync();
					await transaction.CommitAsync();
				}
				catch (Exception) {
					await transaction.RollbackAsync();
					throw;
				}
			}
		}
		public void AddEvent(Event item) {
			using (var dc = _dcFactory.CreateDbContext()) {
				if (dc.Events.Find(item.Id) != null)
					throw new Exception("Event already exists.");
				dc.Events.Add(item);
				dc.SaveChanges();
			}
		}

		//public Task AddEventAsync(Event item) {
		//	var task = new Task(() => {
		//		using (var dc = _dcFactory.CreateDbContext()) {
		//			dc.Events.Add(item);
		//			dc.SaveChanges();
		//		}
		//	});
		//	task.Start();
		//	return task;
		//}

		//public Task AddEventAsync(Event item) {
		//	var task = new Task(() => AddEvent(item));
		//	task.Start();
		//	return task;
		//}

		public async Task AddEventAsync(Event item) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				Event result = await dc.Events.FindAsync(item.Id);
				if (result != null)
					throw new Exception("Event already exists.");
				await dc.Events.AddAsync(item);
				await dc.SaveChangesAsync();
			}
		}


		public void RemoveEvent(string id) {
			using (var dc = _dcFactory.CreateDbContext()) {
				var item = dc.Events.Find(id);
				if (item != null) {
					dc.Events.Remove(item);
					dc.SaveChanges();
				}
				else throw new Exception($"Event {id} does not exist or already removed.");
			}
		}

		public async Task RemoveEventAsync(string id) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var item = await dc.Events.FindAsync(id);
				if (item != null) {
					dc.Events.Remove(item);
					await dc.SaveChangesAsync();
				}
				else throw new Exception($"Event {id} does not exist or already removed.");
			}
		}

		public void UpdateEvent(Event item) {
			using (var dc = _dcFactory.CreateDbContext()) {
				var entity = dc.Events.Attach(item);
				entity.State = EntityState.Modified;
				dc.SaveChanges();
			}
		}

		public async Task UpdateEventAsync(Event item) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var entity = dc.Events.Attach(item);
				entity.State = EntityState.Modified;
				await dc.SaveChangesAsync();
			}
		}

		public List<Event> GetEvents() {
			using (var dc = _dcFactory.CreateDbContext()) {
				return dc.Events.ToList();
			}
		}

		public async Task<List<Event>> GetEventsAsync() {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				return await dc.Events.ToListAsync();
			}
		}

	}
}
