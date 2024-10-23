using Microsoft.EntityFrameworkCore;
using Newrise.Models;

namespace Newrise.Services {
	public class EventDataService {
		readonly IDbContextFactory<NewriseDbContext> _dcFactory;

		public EventDataService(IDbContextFactory<NewriseDbContext> dcFactory) {
			_dcFactory = dcFactory;
		}

		public void AddEvent(Event item) {
			using (var dc = _dcFactory.CreateDbContext()) {
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
