using Microsoft.eShopWeb.ApplicationCore.Entities;
using FilterSpec = Microsoft.eShopWeb.ApplicationCore.Specifications.CatalogFilterSpecification;
using PaginatedSpec = Microsoft.eShopWeb.ApplicationCore.Specifications.CatalogFilterPaginatedSpecification;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Specifications;

public class CatalogFilterPriceRange
{
    // Item names encode brand, type and price: "b{brandId}t{typeId}-{price}".
    public static TheoryData<int?, int?, decimal?, decimal?, string[]> Cases => new()
    {
        // No price range: existing brand/type behaviour unchanged
        { null, null, null, null, ["b1t1-9.99", "b1t1-10", "b2t2-14.99", "b2t1-15", "b1t2-15", "b1t1-20"] },
        // Minimum is inclusive
        { null, null, 10m, 15m, ["b1t1-10", "b2t2-14.99"] },
        // Maximum is exclusive: $15 is excluded from $10-$15 and included in $15-$20
        { null, null, 15m, 20m, ["b2t1-15", "b1t2-15"] },
        // Open-ended bands
        { null, null, null, 10m, ["b1t1-9.99"] },
        { null, null, 20m, null, ["b1t1-20"] },
        // Price combines with brand and type
        { 1, null, 15m, 20m, ["b1t2-15"] },
        { 2, 1, 15m, 20m, ["b2t1-15"] },
        { 2, 2, 10m, 15m, ["b2t2-14.99"] },
        { 2, 1, 10m, 15m, [] },
        // Clearing the price keeps the brand filter applied
        { 1, null, null, null, ["b1t1-9.99", "b1t1-10", "b1t2-15", "b1t1-20"] },
    };

    [Theory]
    [MemberData(nameof(Cases))]
    public void FilterSpecificationAppliesPriceRange(int? brandId, int? typeId, decimal? minPrice, decimal? maxPrice, string[] expected)
    {
        var spec = new FilterSpec(brandId, typeId, minPrice, maxPrice);

        var result = spec.Evaluate(GetTestItemCollection()).Select(i => i.Name);

        Assert.Equal(expected, result);
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void PaginatedSpecificationMatchesFilterSpecification(int? brandId, int? typeId, decimal? minPrice, decimal? maxPrice, string[] expected)
    {
        var spec = new PaginatedSpec(0, 0, brandId, typeId, minPrice, maxPrice);

        var result = spec.Evaluate(GetTestItemCollection()).Select(i => i.Name);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void PaginatedSpecificationPagesWithinPriceRange()
    {
        var spec = new PaginatedSpec(1, 1, null, null, 15m, 20m);

        var result = spec.Evaluate(GetTestItemCollection()).Select(i => i.Name);

        Assert.Equal(["b1t2-15"], result);
    }

    private static List<CatalogItem> GetTestItemCollection()
    {
        // CatalogItem(catalogTypeId, catalogBrandId, ...)
        return new List<CatalogItem>()
        {
            new(1, 1, "Description", "b1t1-9.99", 9.99m, "FakePath"),
            new(1, 1, "Description", "b1t1-10", 10m, "FakePath"),
            new(2, 2, "Description", "b2t2-14.99", 14.99m, "FakePath"),
            new(1, 2, "Description", "b2t1-15", 15m, "FakePath"),
            new(2, 1, "Description", "b1t2-15", 15m, "FakePath"),
            new(1, 1, "Description", "b1t1-20", 20m, "FakePath"),
        };
    }
}
