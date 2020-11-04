using Grand.Core.Plugins;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.Vue
{
    public class VuePlugin : BasePlugin
    {
        public VuePlugin()
        {
        }
        public override async Task Install()
        {
            await base.Install();
        }

        public override async Task Uninstall()
        {
            await base.Uninstall();
        }

    }
}
