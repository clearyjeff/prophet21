public class CartModule : IModule
{
    private const string BasePath = "/cart";

    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        _ = services.AddScoped<ICartService, CartService>();
        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var cartGroup = endpoints.MapGroup(BasePath);

        _ = cartGroup.MapGet("/", (ICartService cartService) =>
        {
            var cart = cartService.GetCart();
            return cart;
        });

        _ = cartGroup.MapPost("/", () => "Cart Get");

        return endpoints;
    }
}