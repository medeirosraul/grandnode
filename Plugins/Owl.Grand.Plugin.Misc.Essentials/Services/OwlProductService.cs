using Grand.Core;
using Grand.Core.Caching;
using Grand.Domain.Catalog;
using Grand.Domain.Data;
using Grand.Services.Catalog;
using Grand.Services.Security;
using Grand.Services.Stores;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owl.Grand.Plugin.Misc.Essentials.Services
{
    public class OwlProductService : ProductService, IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public OwlProductService(ICacheManager cacheManager, IRepository<Product> productRepository, IRepository<ProductDeleted> productDeletedRepository, IWorkContext workContext, IMediator mediator, IAclService aclService, IStoreMappingService storeMappingService, CatalogSettings catalogSettings) : base(cacheManager, productRepository, productDeletedRepository, workContext, mediator, aclService, storeMappingService, catalogSettings)
        {
            _productRepository = productRepository;
        }

        public override async Task InsertRelatedProduct(RelatedProduct relatedProduct)
        {
            var product1 = await GetProductById(relatedProduct.ProductId1, true);
            var product2 = await GetProductById(relatedProduct.ProductId2, true);

            if (!product1.RelatedProducts.Any(x => x.ProductId2 == product2.Id))
                await base.InsertRelatedProduct(relatedProduct);

            if(!product2.RelatedProducts.Any(x => x.ProductId2 == product1.Id))
            {
                var relatedProduct2 = new RelatedProduct { ProductId1 = product2.Id, ProductId2 = product1.Id, DisplayOrder = 1 };
                await base.InsertRelatedProduct(relatedProduct2);

            }
        }
    }
}
