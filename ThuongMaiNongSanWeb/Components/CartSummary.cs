using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Components
{
    [Authorize]
    public class CartSummary : ViewComponent
    {
        public CartSummary() { }
        public IViewComponentResult Invoke()
        {
            ShoppingCart? cart = GetCartItems();
            Cart viewModel = new()
            {
                NumberOfItems = cart.Items.Count,
            };
            return View(viewModel);
        }
        ShoppingCart? GetCartItems()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (sessionCart is not null)
            {
                return HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            }
            return new ShoppingCart();
        }
    }
}
