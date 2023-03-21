using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
}

public class ProductCatalogClient : IProductCatalogClient
{
    private readonly HttpClient  _client;
    private static string productCatalogBaseUrl = @"https://gist.githubusercontent.com/vladcris/2d15d743a18ea6f6f2c64a7fda53d482/raw/f9b251caaf167e534d7239d7fabbbd4158b50ab0/test.json";
    public static string getProductPathTemplate = "?productIds=[{0}]";

    public ProductCatalogClient(HttpClient client)
    {
        client.BaseAddress = new Uri(productCatalogBaseUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _client = client;
    }

    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
    {
        using var response = await RequestProductFromProductCatalog(productCatalogIds);

        return await ConvertToShoppingCartItems(response);
    }

    private async Task<HttpResponseMessage> RequestProductFromProductCatalog(int[] productIds)
    {
        var productsResource = string.Format(getProductPathTemplate, string.Join(",", productIds));
        //var productsResource = @$"?productIds=[{ string.Join(",", productIds) }]";
        
        return await _client.GetAsync(productsResource);
    }

    private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();

        var products = JsonSerializer.Deserialize<List<ProductCatalogProduct>>(
            await response.Content.ReadAsStreamAsync(),
            new JsonSerializerOptions{
                PropertyNameCaseInsensitive = true
            }) ?? new();
        
        return products.Select( p => 
            new ShoppingCartItem
            (
                p.ProductId,
                p.ProductName,
                p.ProductDescription,
                p.Price
            ));

    }

    private record ProductCatalogProduct(int ProductId, string ProductName, string ProductDescription, Money Price);
    // private record ProductCatalogProduct
    // {
    //     public int ProductId { get; set; }
    //     public string? ProductName { get; set; }
    //     public string? ProductDescription { get; set; }
    //     public Money? Price { get; set; }
    // }
}
