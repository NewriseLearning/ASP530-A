using Microsoft.AspNetCore.Components;
using Newrise.Models;
using Newrise.Services;

namespace Newrise.Components.Pages {
	public partial class Contacts {
		//	const string s1 = "vertical-align:top";

		[Inject]OfficeListProvider OfficeList { get; set; }


		List<Office> offices;

		protected override void OnInitialized() {
			offices = OfficeList.GetList();
		}

	}
}
