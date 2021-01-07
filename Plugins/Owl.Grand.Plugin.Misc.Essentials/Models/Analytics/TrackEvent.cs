using Grand.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Misc.Essentials.Models.Analytics
{
    public class TrackEvent: BaseEntity
    {
        public TrackEvent()
        {
            Parameters = new List<TrackEventParameter>();
        }

        public DateTime DateTime { get; set; }

        public string Session { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public List<TrackEventParameter> Parameters { get; set; }
    }

    public class TrackEventParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public static class TrackEventCode
    {
        public static string Navigation { get; } = "Event.Navigation";
    }
}
