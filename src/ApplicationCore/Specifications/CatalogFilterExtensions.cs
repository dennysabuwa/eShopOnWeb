using Ardalis.Specification;
using Microsoft.eShopWeb.ApplicationCore.Entities;

namespace Microsoft.eShopWeb.ApplicationCore.Specifications;

/// <summary>
/// The single catalog filter predicate, shared by the count and paginated specifications
/// so the item list and the total count can't diverge.
/// </summary>
internal static class CatalogFilterExtensions
{
    // minPrice is inclusive, maxPrice is exclusive, so a price on a boundary falls in exactly one band.
    public static ISpecificationBuilder<CatalogItem> ApplyCatalogFilter(
        this ISpecificationBuilder<CatalogItem> query,
        int? brandId,
        int? typeId,
        decimal? minPrice,
        decimal? maxPrice)
    {
        return query.Where(i => (!brandId.HasValue || i.CatalogBrandId == brandId) &&
            (!typeId.HasValue || i.CatalogTypeId == typeId) &&
            (!minPrice.HasValue || i.Price >= minPrice) &&
            (!maxPrice.HasValue || i.Price < maxPrice));
    }
}
