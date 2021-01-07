using Grand.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using Owl.Grand.Plugin.Misc.Essentials.Models.Analytics;
using Owl.Grand.Plugin.Misc.Essentials.Services;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Misc.Essentials.Controllers
{
    public class AnalyticsController : BasePluginController
    {
        private readonly AnalyticsService _service;

        public AnalyticsController(AnalyticsService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Track([FromBody]TrackEvent e)
        {
            _service.Insert(e);
            return Ok();
        }

        public IActionResult GetOnlineUsersCount()
        {
            return Content(_service.GetOnlineUsersCount(60).ToString());
        }
    }
}
