using Grand.Core.ModelBinding;
using Grand.Core.Models;

namespace Owl.Grand.Plugin.Misc.Essentials.Models
{
    public class OwlEssentialsSettingsModel : BaseModel
    {
        [GrandResourceDisplayName("Ambiente Sandbox")]
        public bool IsSandbox { get; set; }
    }
}