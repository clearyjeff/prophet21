//Generate a class for a cart
public class Cart
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<CartItem> Items { get; set; }
    public decimal Total { get; set; }

    public Cart() => Items = new List<CartItem>();
}

public class CartItem
{
}
