using static MudBlazor.CategoryTypes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Newrise.Models {
	public class MyEvent {
		public Event Event;

		public MyEvent() { Event = new Event(); }

		[NotMapped]
		public DateTime? FromDate {
			get {
				return Event.From;
			}
			set {
				if (value != null) {
					Event.From = new DateTime(
						value.Value.Year,
						value.Value.Month,
						value.Value.Day,
						Event.From.Hour,
						Event.From.Minute,
						Event.From.Second
					);
				}
			}
		}

		public string Id {
			get { return Event.Id; }
			set { Event.Id = value; }
		}


		[NotMapped]
		public TimeSpan? FromTime {
			get {
				return Event.From.TimeOfDay;
			}
			set {
				if (value != null) {
					Event.From = new DateTime(
						Event.From.Year,
						Event.From.Month,
						Event.From.Day,
						value.Value.Hours,
						value.Value.Minutes,
						value.Value.Seconds
					);
				}
			}
		}


	}
}
