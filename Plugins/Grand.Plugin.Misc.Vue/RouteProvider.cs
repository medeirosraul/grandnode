using Grand.Core.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Grand.Plugin.Misc.Vue
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapControllerRoute("Plugin.Misc.Vue",
                 "vue/component",
                 new { controller = "Vue", action = "Component" }
            );
        }
        public int Priority {
            get {
                return 10;
            }
        }
    }
}
