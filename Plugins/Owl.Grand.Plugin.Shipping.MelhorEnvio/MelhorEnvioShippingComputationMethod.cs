using Grand.Core;
using Grand.Domain.Orders;
using Grand.Domain.Shipping;
using Grand.Core.Plugins;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Services;
using Grand.Services.Catalog;
using Grand.Services.Configuration;
using Grand.Services.Localization;
using Grand.Services.Shipping;
using Grand.Services.Shipping.Tracking;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owl.Grand.Plugin.Shipping.MelhorEnvio.Domain;
using Newtonsoft.Json.Linq;
using System.Linq;
using DotLiquid.Tags;

namespace Owl.Grand.Plugin.Shipping.MelhorEnvio
{
    public class MelhorEnvioShippingComputationMethod : BasePlugin, IShippingRateComputationMethod
    {
        #region Fields

        private readonly IShippingService _shippingService;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly IStoreContext _storeContext;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IProductService _productService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly ShippingMelhorEnvioService _shippingMelhorEnvioService;
        private readonly ShippingMelhorEnvioSettings _settings;
        #endregion

        #region Ctor
        public MelhorEnvioShippingComputationMethod(IShippingService shippingService,
            IShippingMethodService shippingMethodService,
            IStoreContext storeContext,
            IPriceCalculationService priceCalculationService,
            ISettingService settingService,
            IWebHelper webHelper,
            IWorkContext workContext,
            ILocalizationService localizationService,
            ILanguageService languageService,
            IProductService productService,
            IServiceProvider serviceProvider,
            ShippingMelhorEnvioService shippingMelhorEnvioService, ShippingMelhorEnvioSettings settings, IPriceFormatter priceFormatter)
        {
            _shippingService = shippingService;
            _shippingMethodService = shippingMethodService;
            _storeContext = storeContext;
            _priceCalculationService = priceCalculationService;
            _settingService = settingService;
            _webHelper = webHelper;
            _workContext = workContext;
            _localizationService = localizationService;
            _languageService = languageService;
            _productService = productService;
            _serviceProvider = serviceProvider;
            _shippingMelhorEnvioService = shippingMelhorEnvioService;
            _settings = settings;
            _priceFormatter = priceFormatter;
        }
        #endregion

        #region Utilities

        private async Task<decimal?> GetRate(decimal subTotal, decimal weight, string shippingMethodId,
            string storeId, string warehouseId, string countryId, string stateProvinceId, string zip)
        {

            var shippingMelhorEnvioService = _serviceProvider.GetRequiredService<IShippingMelhorEnvioService>();
            var shippingMelhorEnvioSettings = _serviceProvider.GetRequiredService<ShippingMelhorEnvioSettings>();

            var shippingMelhorEnvioRecord = await shippingMelhorEnvioService.FindRecord(shippingMethodId,
                storeId, warehouseId, countryId, stateProvinceId, zip, weight);
            if (shippingMelhorEnvioRecord == null)
            {
                return decimal.Zero;
            }

            //additional fixed cost
            decimal shippingTotal = shippingMelhorEnvioRecord.AdditionalFixedCost;
            //charge amount per weight unit
            if (shippingMelhorEnvioRecord.RatePerWeightUnit > decimal.Zero)
            {
                var weightRate = weight - shippingMelhorEnvioRecord.LowerWeightLimit;
                if (weightRate < decimal.Zero)
                    weightRate = decimal.Zero;
                shippingTotal += shippingMelhorEnvioRecord.RatePerWeightUnit * weightRate;
            }
            //percentage rate of subtotal
            if (shippingMelhorEnvioRecord.PercentageRateOfSubtotal > decimal.Zero)
            {
                shippingTotal += Math.Round((decimal)((((float)subTotal) * ((float)shippingMelhorEnvioRecord.PercentageRateOfSubtotal)) / 100f), 2);
            }

            if (shippingTotal < decimal.Zero)
                shippingTotal = decimal.Zero;
            return shippingTotal;
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="getShippingOptionRequest">A request for getting shipping options</param>
        /// <returns>Represents a response of getting shipping rate options</returns>
        public async Task<GetShippingOptionResponse> GetShippingOptions(GetShippingOptionRequest getShippingOptionRequest)
        {
            if (getShippingOptionRequest == null)
                throw new ArgumentNullException("getShippingOptionRequest");

            var response = new GetShippingOptionResponse();

            if (getShippingOptionRequest.Items == null || getShippingOptionRequest.Items.Count == 0)
            {
                response.AddError("No shipment items");
                return response;
            }

            if (getShippingOptionRequest.ShippingAddress == null)
            {
                response.AddError("Shipping address is not set");
                return response;
            }

            var shippingItems = new List<MelhorEnvioShipmentProduct>();

            decimal subTotal = decimal.Zero;
            foreach (var packageItem in getShippingOptionRequest.Items)
            {
                if (packageItem.ShoppingCartItem.IsFreeShipping)
                    continue;

                var product = await _productService.GetProductById(packageItem.ShoppingCartItem.ProductId);

                // Add product to ship calc
                var productSubtotal = (await _priceCalculationService.GetSubTotal(packageItem.ShoppingCartItem, product)).subTotal;
                shippingItems.Add(new MelhorEnvioShipmentProduct {
                    Id = product.Id,
                    Width = Convert.ToInt32(product.Width),
                    Height = Convert.ToInt32(product.Height),
                    Length = Convert.ToInt32(product.Length),
                    Weight = product.Weight,
                    Quantity = packageItem.GetQuantity(),
                    InsuranceValue = productSubtotal
                });

                if (product != null)
                    subTotal += productSubtotal;
            }

            var postalCodeFrom = getShippingOptionRequest.ZipPostalCodeFrom;
            if (string.IsNullOrEmpty(postalCodeFrom))
                postalCodeFrom = _settings.PostalCodeFrom;

            var melhorEnvioShipment = new MelhorEnvioShipment {
                From = new MelhorEnvioShipmentAddress { PostalCode = postalCodeFrom.Replace("-", string.Empty) },
                To = new MelhorEnvioShipmentAddress { PostalCode = getShippingOptionRequest.ShippingAddress.ZipPostalCode.Replace("-", string.Empty) },
                Products = shippingItems,
                Options = new MelhorEnvioShipmentOptions { OwnHand = false, Receipt = false }
            };

            var result =  await _shippingMelhorEnvioService.CalcShipping(melhorEnvioShipment);
            var resultObject = JObject.Parse("{ \"result\": " + result + "}");
            var shippingOptionsList = resultObject["result"].Children();
            decimal minShippingValue = 0;

            // Show ship options
            foreach(var option in shippingOptionsList)
            {
                if (!string.IsNullOrEmpty(option["error"]?.ToString()))
                    continue;
                var description = $"{option["custom_delivery_range"]["min"]}-{option["custom_delivery_range"]["max"]} dias úteis.";
                var shippingValue = Convert.ToDecimal(option["custom_price"]);
                var shippingOption = new ShippingOption {
                    Name = option["company"]["name"].ToString() + " - " + option["name"].ToString(),
                    Description = description,
                    Rate = shippingValue,
                    ShippingRateComputationMethodSystemName = "Shipping.MelhorEnvio"
                };

                response.ShippingOptions.Add(shippingOption);

                if (minShippingValue == 0 || minShippingValue > shippingValue) 
                    minShippingValue = shippingValue;
            }

            

            // Free shipping over
            if (_settings.FreeShippingOver > 0 && subTotal >= _settings.FreeShippingOver)
            {
                var canFreeShipping = false;

                // Validate free shipping state
                if(!string.IsNullOrEmpty(_settings.FreeShippingStates))
                {
                    var cepInfo = _shippingMelhorEnvioService.GetCepInfo(melhorEnvioShipment.To.PostalCode);
                    var cepInfoObject = JObject.Parse(cepInfo);

                    if (string.IsNullOrEmpty(cepInfoObject["error"]?.ToString()))
                    {
                        var uf = cepInfoObject["uf"]?.ToString();
                        if (!string.IsNullOrEmpty(uf))
                        {
                            if (_settings.FreeShippingStates.Split(';').Any(x => x.Trim().ToUpper() == uf))
                                canFreeShipping = true;
                        }
                    }
                }
                else
                {
                    canFreeShipping = true;
                }

                if(canFreeShipping)
                {
                    var shippingOption = new ShippingOption {
                        Name = "Frete grátis",
                        Description = $"5-11 dias úteis. Frete grátis a partir de {_priceFormatter.FormatPrice(_settings.FreeShippingOver)} em compras. ",
                        Rate = 0,
                        ShippingRateComputationMethodSystemName = "Shipping.MelhorEnvio"
                    };

                    if(!string.IsNullOrWhiteSpace(_settings.FreeShippingStates))
                    {
                        shippingOption.Description += $"Estados: {string.Join(", ", _settings.FreeShippingStates.Split(';'))}.";
                    }
                    response.ShippingOptions.Add(shippingOption);
                }
            }

            // Combine shipping over
            if (_settings.CombineShippingOver > 0 && minShippingValue >= _settings.CombineShippingOver)
            {
                var shippingOption = new ShippingOption {
                    Name = "Combinar envio",
                    Description = $"Disponível somente para as regiões Sul e Sudeste quando o frete mínimo ultrapassa {_priceFormatter.FormatPrice(_settings.CombineShippingOver)}.",
                    Rate = _settings.CombineShippingOver,
                    ShippingRateComputationMethodSystemName = "Shipping.MelhorEnvio"
                };

                response.ShippingOptions.Add(shippingOption);
            }

            return response;
        }

        /// <summary>
        /// Gets fixed shipping rate (if shipping rate computation method allows it and the rate can be calculated before checkout).
        /// </summary>
        /// <param name="getShippingOptionRequest">A request for getting shipping options</param>
        /// <returns>Fixed shipping rate; or null in case there's no fixed shipping rate</returns>
        public async Task<decimal?> GetFixedRate(GetShippingOptionRequest getShippingOptionRequest)
        {
            return await Task.FromResult(default(decimal?));
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override async Task Install()
        {
            //settings
            var settings = new ShippingMelhorEnvioSettings {
                
            };
            await _settingService.SaveSetting(settings);

            //locales
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Store", "Store");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Store.Hint", "If an asterisk is selected, then this shipping rate will apply to all stores.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Warehouse", "Warehouse");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Warehouse.Hint", "If an asterisk is selected, then this shipping rate will apply to all warehouses.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Country", "Country");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Country.Hint", "If an asterisk is selected, then this shipping rate will apply to all customers, regardless of the country.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.StateProvince", "State / province");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.StateProvince.Hint", "If an asterisk is selected, then this shipping rate will apply to all customers from the given country, regardless of the state.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Zip", "Zip");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.Zip.Hint", "Zip / postal code. If zip is empty, then this shipping rate will apply to all customers from the given country or state, regardless of the zip code.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.ShippingMethod", "Shipping method");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.ShippingMethod.Hint", "The shipping method.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.From", "Order weight from");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.From.Hint", "Order weight from.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.To", "Order weight to");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.To.Hint", "Order weight to.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.AdditionalFixedCost", "Additional fixed cost");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.AdditionalFixedCost.Hint", "Specify an additional fixed cost per shopping cart for this option. Set to 0 if you don't want an additional fixed cost to be applied.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.LowerWeightLimit", "Lower weight limit");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.LowerWeightLimit.Hint", "Lower weight limit. This field can be used for \"per extra weight unit\" scenarios.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.PercentageRateOfSubtotal", "Charge percentage (of subtotal)");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.PercentageRateOfSubtotal.Hint", "Charge percentage (of subtotal).");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.RatePerWeightUnit", "Rate per weight unit");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.RatePerWeightUnit.Hint", "Rate per weight unit.");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.LimitMethodsToCreated", "Limit shipping methods to configured ones");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.LimitMethodsToCreated.Hint", "If you check this option, then your customers will be limited to shipping options configured here. Otherwise, they'll be able to choose any existing shipping options even they've not configured here (zero shipping fee in this case).");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Fields.DataHtml", "Data");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.AddRecord", "Add record");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Formula", "Formula to calculate rates");
            await this.AddOrUpdatePluginLocaleResource(_localizationService, _languageService, "Plugins.Shipping.MelhorEnvio.Formula.Value", "[additional fixed cost] + ([order total weight] - [lower weight limit]) * [rate per weight unit] + [order subtotal] * [charge percentage]");

            await base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override async Task Uninstall()
        {
            await base.Uninstall();
        }

        /// <summary>
        /// Returns a value indicating whether shipping methods should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>true - hide; false - display.</returns>
        public async Task<bool> HideShipmentMethods(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this shipping methods if all products in the cart are downloadable
            //or hide this shipping methods if current customer is from certain country
            return await Task.FromResult(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a shipping rate computation method type
        /// </summary>
        public ShippingRateComputationMethodType ShippingRateComputationMethodType {
            get {
                return ShippingRateComputationMethodType.Offline;
            }
        }


        /// <summary>
        /// Gets a shipment tracker
        /// </summary>
        public IShipmentTracker ShipmentTracker {
            get {
                //uncomment a line below to return a general shipment tracker (finds an appropriate tracker by tracking number)
                return null;
            }
        }
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/ShippingMelhorEnvio/Configure";
        }

        public async Task<IList<string>> ValidateShippingForm(IFormCollection form)
        {
            //you can implement here any validation logic
            return await Task.FromResult(new List<string>());
        }


        public void GetPublicViewComponent(out string viewComponentName)
        {
            viewComponentName = "";
        }

        #endregion
    }

}
