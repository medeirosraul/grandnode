using Grand.Domain.Catalog;
using Grand.Services.Catalog;
using Grand.Services.ExportImport;
using Grand.Services.ExportImport.Help;
using Grand.Services.Media;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Misc.Essentials.Services
{
    public class MercadoLivreService
    {
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly IExportManager _exportManager;

        public MercadoLivreService(IProductService productService, IPictureService pictureService, IExportManager exportManager)
        {
            _productService = productService;
            _pictureService = pictureService;
            _exportManager = exportManager;
        }

        public async Task<byte[]> ExportProductsToMercadoLivre() 
        {
            var result = await _productService.SearchProducts(overridePublished: true);
            var products = result.products;

            var properties = new[]
            {
                new PropertyByName<Product>("Título", p => p.Name),
                new PropertyByName<Product>("GTIN", p => p.Gtin),
                new PropertyByName<Product>("Pictures", p =>  GetPicturesUrls(p)),
                new PropertyByName<Product>("SKU", p => p.Id),
                new PropertyByName<Product>("Quantidade", p => 1),
                new PropertyByName<Product>("Preço", p => p.Price),
                new PropertyByName<Product>("Condição", p => "Novo"),
                new PropertyByName<Product>("Descrição", p => GetDescription(p)),
                new PropertyByName<Product>("Link do youtube", p => ""),
                new PropertyByName<Product>("Tipo de Anúncio", p => "Clássico"),
                new PropertyByName<Product>("Forma de Envio", p => "Mercado Envios"),
                new PropertyByName<Product>("Custo de envio", p => "Por conta do comprador"),
                new PropertyByName<Product>("Retirar pessoalmente", p => "Não aceito"),
                new PropertyByName<Product>("Tipo de garantia", p => "Garantia de fábrica"),
                new PropertyByName<Product>("Tempo de garantia", p => 3),
                new PropertyByName<Product>("Unidade de tempo de garantia", p => "meses"),
                new PropertyByName<Product>("Material", p => "Cerâmica"),
                new PropertyByName<Product>("Fabricante", p => ""),
                new PropertyByName<Product>("Modelo", p => ""),
                new PropertyByName<Product>("Formato de venda", p => "Unidade"),
                new PropertyByName<Product>("Unidades por kit", p => 1),
                new PropertyByName<Product>("Capacidade em volume", p => ""),
                new PropertyByName<Product>("Unidade de capacidade em volume", p => "mL")
            };

            var xlsx = (_exportManager as ExportManager).ExportToXlsx(properties, products);
            return xlsx;
        }

        public string GetPicturesUrls(Product product)
        {
            var result = string.Empty;
            var first = true;

            foreach(var p in product.ProductPictures)
            {
                if (!first) result += ",";
                first = false;

                var url = _pictureService.GetPictureUrl(p.PictureId).GetAwaiter().GetResult();
                result += "https://corujinhapresentes.com.br" + url;
            }
            return result;
        }

        public string GetDescription(Product product)
        {
            return Regex.Replace(product.FullDescription, "<[a-zA-Z/].*?>", string.Empty);
        }
    }
}
