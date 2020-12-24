using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Owl.Grand.Plugin.Payments.PagSeguro.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owl.Grand.Plugin.Payments.PagSeguro.Components
{
    [ViewComponent(Name = "PagSeguroPaymentInfo")]
    public class PagSeguroPaymentInfoComponent : ViewComponent
    {
        private readonly PagSeguroSettings _pagSeguroSettings;

        public PagSeguroPaymentInfoComponent(PagSeguroSettings pagSeguroSettings)
        {
            _pagSeguroSettings = pagSeguroSettings;
        }

        public IViewComponentResult Invoke()
        {
#if DEBUG
            _pagSeguroSettings.IsSandbox = true;
#endif

            if(_pagSeguroSettings.IsSandbox)
            {
                _pagSeguroSettings.PublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAr+ZqgD892U9/HXsa7XqBZUayPquAfh9xx4iwUbTSUAvTlmiXFQNTp0Bvt/5vK2FhMj39qSv1zi2OuBjvW38q1E374nzx6NNBL5JosV0+SDINTlCG0cmigHuBOyWzYmjgca+mtQu4WczCaApNaSuVqgb8u7Bd9GCOL4YJotvV5+81frlSwQXralhwRzGhj/A57CGPgGKiuPT+AOGmykIGEZsSD9RKkyoKIoc0OS8CPIzdBOtTQCIwrLn2FxI83Clcg55W8gkFSOS6rWNbG5qFZWMll6yl02HtunalHmUlRUL66YeGXdMDC2PuRcmZbGO5a/2tbVppW6mfSWG3NPRpgwIDAQAB";
            }

            var model = new PaymentInfoModel {
                PagSeguroSettings = _pagSeguroSettings,
                ExpireMonths = new List<SelectListItem>(),
                ExpireYears = new List<SelectListItem>()
            };

            for (var i = 1; i <= 12; i++)
            {
                model.ExpireMonths.Add(new SelectListItem { Value = i.ToString(), Text = ("0" + i).PadLeft(2) });
            }

            for (var i = 0; i <= 9; i++)
            {
                model.ExpireYears.Add(new SelectListItem { Value = DateTime.Now.AddYears(i).Year.ToString(), Text = DateTime.Now.AddYears(i).Year.ToString().PadLeft(2) });
            }

            return View("~/Plugins/Owl.Grand.Plugin.Payments.PagSeguro/Views/PaymentInfo.cshtml", model);
        }
    }
}
