using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MudBlazor.CategoryTypes;

namespace Newrise.Models {
	public class Event {
		[Key, Required(ErrorMessage = "{0} is required.")]
		[StringLength(6, ErrorMessage = "{0} can only have {1} characters.")]
		[RegularExpression(@"^[A-Z]{3}\d{3}$", ErrorMessage = "{0} is not correctly formatted.")]
		public string Id { get; set; }

		[Range(
			(int)EventType.Presentation,
			(int)EventType.Forum,
			ErrorMessage = "{0} is not valid.")]
		public EventType Type { get; set; }

		[Required(ErrorMessage = "{0} is required.")]
		[StringLength(50, ErrorMessage = "{0} can only have {1} characters.")]
		public string Title { get; set; }

		[StringLength(1000, ErrorMessage = "{0} can only contain {1} characters.")]
		public string Description { get; set; }
		
		public DateTime From { get; set; }

		[Range(0.5, 8.0, ErrorMessage = "{0} must be from {1} to {2}.")]
		public double Hours { get; set; }

		public DateTime To { get { return From.Add(TimeSpan.FromHours(Hours)); } }

		[Range(1, 200, ErrorMessage = "{0} must be from {1} to {2}.")]
		public int Seats { get; set; }

		public int AllocatedSeats { get; set; }

		public int RemainingSeats { get { return Seats - AllocatedSeats; } }

		[Range(0, 5000, ErrorMessage = "{0} must be from {1} to {2}.")]
		public decimal Fee { get; set; }

		public bool Online { get; set; }

		[NotMapped]
		public DateTime? FromDate {
			get {
				return From;
			}
			set {
				if (value != null) {
					From = new DateTime(
						value.Value.Year,
						value.Value.Month,
						value.Value.Day,
						From.Hour,
						From.Minute,
						From.Second
					);
				}
			}
		}

		[NotMapped]
		public TimeSpan? FromTime {
			get {
				return From.TimeOfDay;
			}
			set {
				if (value != null) {
					From = new DateTime(
						From.Year,
						From.Month,
						From.Day,
						value.Value.Hours,
						value.Value.Minutes,
						value.Value.Seconds
					);
				}
			}
		}

		public virtual ICollection<Participant> Participants { get; set; } = new HashSet<Participant>();
	}
}
