using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingCart.EventFeed;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart.ShoppingCart;

public class ShoppingCart
{
    private readonly HashSet<ShoppingCartItem> items = new();
    public int UserId { get; }
    public IEnumerable<ShoppingCartItem> Items  => this.items;

    public ShoppingCart(int userId)
    {
        UserId = userId;
    }

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
    {
        foreach (var item in shoppingCartItems)
        {
            if (this.items.Add(item) )
            {
                eventStore.Raise("ShoppingCartItemAdded", new{ UserId, item});
            }
        }
    }

    public void RemoveItems(int[] productCatalogueIds, IEventStore eventStore)
    {
        //this.items.RemoveWhere(item => productCatalogueIds.Contains(item.ProductCatalogueId));

        foreach (var productId in productCatalogueIds)
        {
            if( items.RemoveWhere( i => i.ProductCatalogueId == productId) > 0 )
            {
                eventStore.Raise("ShoppingCartItemRemoved", new{ UserId, productId});
            }
        }
    }
}

