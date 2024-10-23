using Newrise.Validators;
using System.ComponentModel.DataAnnotations;

namespace Newrise.Models {
	public class LoginModel {
		[Required]
		public string UserId { get; set; }

		[Required, Password]
		public string Password { get; set;  }
	}
}
