using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newrise.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Newrise.Services {
    public class OfficeListProvider {
        private const string Filename = "offices.json";
    //    private readonly FileSystemWatcher _watcher;
        private readonly string _path;
    //    private List<Office> _offices;
        private IMemoryCache _cache;
        private IChangeToken _token;

        public OfficeListProvider(IWebHostEnvironment environment, IMemoryCache cache) {
			var filepath = @"Content\" + Filename;
			_path = Path.Combine(environment.ContentRootPath, filepath);
			_token = environment.ContentRootFileProvider.Watch(filepath);
			_cache = cache;
			// var directory = Path.Combine(environment.ContentRootPath, "Content");
			//  _path = Path.Combine(directory, Filename);
			//   _watcher = new FileSystemWatcher(directory, Filename);
			//   _watcher.NotifyFilter = NotifyFilters.LastWrite;
			//   _watcher.EnableRaisingEvents = true;
			//   _watcher.Changed += OnListChanged;
			//    Load();
		}

		//      private void OnListChanged(object sender, FileSystemEventArgs e) {
		//          //    if (e.Name == Filename) Load();
		//          _cache.Remove(Filename);
		//}

		public List<Office> GetList() {
            var offices = _cache.Get<List<Office>>(Filename);
            if (offices == null) {
				using (var stream = new FileStream(_path, FileMode.Open, FileAccess.Read)) {
					offices = JsonSerializer.Deserialize<List<Office>>(stream);
					//    _cache.Set(Filename, offices, TimeSpan.FromMinutes(15));
					//  _cache.Set(Filename, offices, new MemoryCacheEntryOptions {
					//	SlidingExpiration = TimeSpan.FromMinutes(15),
					//  });
					_cache.Set(Filename, offices, _token);
				}
			}
            return offices;
		}
    }
}
