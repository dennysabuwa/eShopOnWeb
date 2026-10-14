using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

public class CatalogFilterSpecification : Specification<CatalogItem>
{
    public CatalogFilterSpecification(int? brandId, int? typeId, decimal? minPrice = null, decimal? maxPrice = null)
    {
        Query.ApplyCatalogFilter(brandId, typeId, minPrice, maxPrice);
    }
}
