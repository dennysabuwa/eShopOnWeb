using System.Collections.Generic;
using System.Linq;

namespace Microsoft.eShopWeb.ApplicationCore.Constants;

/// <summary>
/// A preset catalog price range. MinPrice is inclusive and MaxPrice is exclusive; null means unbounded.
/// </summary>
public record CatalogPriceBand(int Id, string Label, decimal? MinPrice, decimal? MaxPrice);

public static class CatalogPriceBands
{
    public static readonly IReadOnlyList<CatalogPriceBand> All =
    [
        new(1, "Under $10", null, 10m),
        new(2, "$10 - $15", 10m, 15m),
        new(3, "$15 - $20", 15m, 20m),
        new(4, "$20 and over", 20m, null)
    ];

    /// <summary>
    /// Returns the band with the given id, or null for no id or an unknown id (treated as no price filter).
    /// </summary>
    public static CatalogPriceBand? FindById(int? id)
    {
        return id.HasValue ? All.FirstOrDefault(b => b.Id == id.Value) : null;
    }
}
