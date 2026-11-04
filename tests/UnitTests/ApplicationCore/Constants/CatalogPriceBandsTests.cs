using Microsoft.eShopWeb.ApplicationCore.Constants;
using Xunit;

namespace Microsoft.eShopWeb.UnitTests.ApplicationCore.Constants;

public class CatalogPriceBandsTests
{
    [Theory]
    [InlineData(1, null, 10)]
    [InlineData(2, 10, 15)]
    [InlineData(3, 15, 20)]
    [InlineData(4, 20, null)]
    public void FindByIdReturnsBandRange(int id, int? expectedMin, int? expectedMax)
    {
        var band = CatalogPriceBands.FindById(id);

        Assert.NotNull(band);
        Assert.Equal((decimal?)expectedMin, band.MinPrice);
        Assert.Equal((decimal?)expectedMax, band.MaxPrice);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(99)]
    public void FindByIdReturnsNullForMissingOrUnknownId(int? id)
    {
        Assert.Null(CatalogPriceBands.FindById(id));
    }

    [Fact]
    public void BandsAreContiguousWithoutOverlap()
    {
        var bands = CatalogPriceBands.All;

        Assert.Null(bands[0].MinPrice);
        Assert.Null(bands[^1].MaxPrice);
        for (int i = 1; i < bands.Count; i++)
        {
            Assert.Equal(bands[i - 1].MaxPrice, bands[i].MinPrice);
        }
    }

    [Fact]
    public void BandIdsAreUnique()
    {
        Assert.Equal(CatalogPriceBands.All.Count, CatalogPriceBands.All.Select(b => b.Id).Distinct().Count());
    }
}
