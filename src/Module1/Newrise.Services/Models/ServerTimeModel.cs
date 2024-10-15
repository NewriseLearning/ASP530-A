using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newrise.Services.Models {
	public class ServerTimeModel {
		public string Id { get; set; }
		public DateTime Time { get; set; }

		public ServerTimeModel(string id, DateTime time) {
			Id = id;
			Time = time;
		}
	}
}
