﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.ComponentModel.DataAnnotations;

namespace Newrise.Models {
	public class Participant {
		[Key, Required(ErrorMessage = "{0} is required.")]
		[StringLength(40, ErrorMessage = "{0} can only have {1} characters.")]
		public string Id { get; set; }

		[Required(ErrorMessage = "{0} is required.")]
		[StringLength(40, ErrorMessage = "{0} can only have {1} characters.")]
		public string Name { get; set; }

		[StringLength(40, ErrorMessage = "{0} can only have {1} characters.")]
		public string Company { get; set; }

		[StringLength(40, ErrorMessage = "{0} can only have {1} characters.")]
		public string Position { get; set; }

		[Required(ErrorMessage = "{0} is required.")]
		[StringLength(254, ErrorMessage = "{0} can only have {1} characters.")]
		[EmailAddress(ErrorMessage = "{0} is not correctly formatted.")]
		public string Email { get; set; }

		public byte[] Photo { get; set; }

		public virtual ICollection<Event> Events { get; set; } = new HashSet<Event>();
	}
}
