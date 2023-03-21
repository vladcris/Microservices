using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.ShoppingCart;

public record ShoppingCartItem(
    int ProductCatalogueId,
    string ProductName,
    string Description,
    Money Price)
{
    public virtual bool Equals(ShoppingCartItem? obj) =>
        obj != null && this.ProductCatalogueId == obj.ProductCatalogueId;
    
    public override int GetHashCode() => this.ProductCatalogueId.GetHashCode();

}

public record Money(string Currency, decimal Amount);