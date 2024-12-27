namespace WebApp.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public CartItem? GetItem(string productId)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId.Equals(productId));
            return existingItem;
        }
        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId.Equals(item.ProductId));
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void UpdateItem(string productId, int quantity)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId.Equals(productId));
            if (existingItem is not null)
            {
                existingItem.Quantity = quantity;
            }
        }

        public void RemoveItem(string productId)
        {
            Items.RemoveAll(i => i.ProductId.Equals(productId));
        }
    }
}
