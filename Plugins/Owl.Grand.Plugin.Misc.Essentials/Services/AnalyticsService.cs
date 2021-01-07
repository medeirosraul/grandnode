using Grand.Domain.Data;
using Owl.Grand.Plugin.Misc.Essentials.Models.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Misc.Essentials.Services
{
    public class AnalyticsService
    {
        private static List<TrackEvent> _cache;

        private readonly IRepository<TrackEvent> _repository;

        public List<TrackEvent> EventCache {
            get {
                if (_cache == null)
                    _cache = new List<TrackEvent>();
                return _cache;
            }
        }

        public async Task Insert(TrackEvent e)
        {
            if (e == null) return;

            EventCache.Add(e);

            //if(EventCache.Count >= 1)
            //{
            //    await _repository.InsertManyAsync(_cache);
            //}
        }

        public int GetOnlineUsersCount(int lastSeconds)
        {
            return EventCache.Where(p => 
            p.DateTime > DateTime.Now.AddSeconds(lastSeconds * -1)
            && p.Code == TrackEventCode.Navigation
            )
                .GroupBy(p => (p.Session, p.Code))
                .Count();
        }
    }
}
