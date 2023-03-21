using Polly;
using ShoppingCart;
using ShoppingCart.EventFeed;
using ShoppingCart.ShoppingCart;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
    .AddTransientHttpErrorPolicy(policy =>
        policy.WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(100*Math.Pow(2, attempt))));
    
builder.Services.Scan(selector =>
{
    selector.FromAssemblyOf<Program>().AddClasses().AsMatchingInterface();

});

// builder.Services.AddScoped<IShoppingCartStore, ShoppingCartStore>();
// builder.Services.AddScoped<IEventStore, EventStore>();
//builder.Services.AddScoped<IProductCatalogClient, ProductCatalogClient>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
