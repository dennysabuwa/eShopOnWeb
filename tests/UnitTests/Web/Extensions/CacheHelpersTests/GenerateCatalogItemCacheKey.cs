using Microsoft.eShopWeb.Web;
using Microsoft.eShopWeb.Web.Extensions;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.Web.Extensions.CacheHelpersTests;

public class GenerateCatalogItemCacheKey
{
    [Fact]
    public void ReturnsCatalogItemCacheKey()
    {
        var pageIndex = 0;
        int? brandId = null;
        int? typeId = null;
        int? priceBandId = null;

        var result = CacheHelpers.GenerateCatalogItemCacheKey(pageIndex, Constants.ITEMS_PER_PAGE, brandId, typeId, priceBandId);

        Assert.Equal("items-0-10---", result);
    }

    [Fact]
    public void IncludesPriceBandInCacheKey()
    {
        var result = CacheHelpers.GenerateCatalogItemCacheKey(0, Constants.ITEMS_PER_PAGE, 1, 2, 3);

        Assert.Equal("items-0-10-1-2-3", result);
    }

    [Fact]
    public void DifferentPriceBandsProduceDifferentKeys()
    {
        var noBand = CacheHelpers.GenerateCatalogItemCacheKey(0, Constants.ITEMS_PER_PAGE, 1, 2, null);
        var band2 = CacheHelpers.GenerateCatalogItemCacheKey(0, Constants.ITEMS_PER_PAGE, 1, 2, 2);
        var band3 = CacheHelpers.GenerateCatalogItemCacheKey(0, Constants.ITEMS_PER_PAGE, 1, 2, 3);

        Assert.Equal(3, new[] { noBand, band2, band3 }.Distinct().Count());
    }
}
