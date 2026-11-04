using Xunit;

namespace Microsoft.eShopWeb.FunctionalTests.Web.Pages;

// Seeded brand 2 is ".NET": Bot Black Sweatshirt $19.50; Foundation Sweatshirt, Blue Sweatshirt,
// Foundation Sheet $12; Black & White Mug, Cup<T> Sheet $8.50.
// Price bands: 1 = Under $10, 2 = $10 - $15, 3 = $15 - $20, 4 = $20 and over.
[Collection("Sequential")]
public class HomePagePriceFilter(TestApplication factory) : IClassFixture<TestApplication>
{
    public HttpClient Client { get; } = factory.CreateClient();

    [Fact]
    public async Task ShowsPriceFilterAlongsideBrandAndType()
    {
        var page = await GetHomePage("/");

        Assert.Contains("name=\"CatalogModel.BrandFilterApplied\"", page);
        Assert.Contains("name=\"CatalogModel.TypesFilterApplied\"", page);
        Assert.Contains("name=\"CatalogModel.PriceBandFilterApplied\"", page);
        Assert.Contains("Under $10", page);
        Assert.Contains("$20 and over", page);
    }

    [Fact]
    public async Task AppliesPriceBandTogetherWithBrand()
    {
        var page = await GetHomePage("/?CatalogModel.BrandFilterApplied=2&CatalogModel.PriceBandFilterApplied=3");

        Assert.Contains(".NET Bot Black Sweatshirt", page);
        Assert.DoesNotContain(".NET Blue Sweatshirt", page);
        Assert.DoesNotContain("Prism White T-Shirt", page);
        Assert.Contains("Showing 1 of 1 products", page);
    }

    [Fact]
    public async Task CountMatchesItemsForLowestBand()
    {
        var page = await GetHomePage("/?CatalogModel.BrandFilterApplied=2&CatalogModel.PriceBandFilterApplied=1");

        Assert.Contains(".NET Black &amp; White Mug", page);
        Assert.Contains("Cup&lt;T&gt; Sheet", page);
        Assert.DoesNotContain(".NET Bot Black Sweatshirt", page);
        Assert.Contains("Showing 2 of 2 products", page);
    }

    [Fact]
    public async Task ClearingPriceBandKeepsBrandFilter()
    {
        var page = await GetHomePage("/?CatalogModel.BrandFilterApplied=2&CatalogModel.PriceBandFilterApplied=");

        Assert.Contains(".NET Bot Black Sweatshirt", page);
        Assert.Contains(".NET Blue Sweatshirt", page);
        Assert.DoesNotContain("Prism White T-Shirt", page);
        Assert.Contains("Showing 6 of 6 products", page);
    }

    [Fact]
    public async Task UnknownPriceBandIsIgnored()
    {
        var page = await GetHomePage("/?CatalogModel.BrandFilterApplied=2&CatalogModel.PriceBandFilterApplied=99");

        Assert.Contains("Showing 6 of 6 products", page);
    }

    private async Task<string> GetHomePage(string url)
    {
        var response = await Client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
    }
}
