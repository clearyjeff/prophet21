public class CartService : ICartService
{
    public Cart GetCart()
    {
        return new Cart {
            Name = "My Cart 6",
        };
    }
}