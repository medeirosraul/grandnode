using Grand.Framework.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Threading.Tasks;

namespace Grand.Plugin.Misc.Vue.Controllers
{
    public class VueController : BasePluginController
    {
        #region Ctor

        public VueController()
        {
        }

        #endregion

        #region Methods

        public IActionResult Component(string component)
        {
            return ViewComponent(component);
        }

        #endregion

    }
}