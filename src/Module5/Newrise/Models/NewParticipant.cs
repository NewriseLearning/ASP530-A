using Newrise.Validators;
using System.ComponentModel.DataAnnotations;

namespace Newrise.Models {
	public class NewParticipant : Participant {
		[Required, Display(Name = "Confirmation Password")]
		[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
		[Password]
		public string ConfirmPassword { get; set; }
	}
}
