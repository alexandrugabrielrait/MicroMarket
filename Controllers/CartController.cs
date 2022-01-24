using MicroMarket.Models;
using MicroMarket.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicroMarket.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly IRepository<Product> _productRepository;

        public CartController(ILogger<CartController> logger, IRepository<Product> productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            return View(cart);
        }

        public int IndexOfProduct(int id)
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.ProductId == id)
                    return i;
            return -1;
        }

        public void RefreshCartProducts()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            foreach (var cartItem in cart)
            {
                cartItem.Product = _productRepository.Get(cartItem.Product.ProductId);
            }
        }

        public IActionResult AddProduct(int id)
        {
            RefreshCartProducts();
            var product = _productRepository.Get(id);
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
                cart.Add(new CartItem(product));
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = IndexOfProduct(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem(product));
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return new JsonResult(new { count = cart.Select(x => x.Quantity).Sum() });
        }

        public IActionResult SetProductQuantity(int id, int quantity)
        {
            if (quantity <= 0)
                return SetProductQuantity(id, 1);
            RefreshCartProducts();
            var product = _productRepository.Get(id);
            if (product == null)
                return BadRequest();
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            var cartCount = 0;
            if (cart == null)
            {
                cart = new List<CartItem>();
                cart.Add(new CartItem(product));
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = IndexOfProduct(id);
                if (index != -1)
                {
                    cart[index].Quantity = quantity;
                    if (cart[index].Quantity == 0)
                    {
                        cart.RemoveAt(index);
                    }
                }
                else if (quantity != 0)
                {
                    var cartItem = new CartItem(product);
                    cartItem.Quantity = quantity;
                    cart.Add(cartItem);
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                cartCount = cart.Select(x => x.Quantity).Sum();
            }
            return new JsonResult(new { count = cartCount, productPrice = (quantity * product.Price).ToString("0.####"), quantity = quantity });
        }

        public IActionResult RemoveProduct(int id)
        {
            RefreshCartProducts();
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            var cartCount = 0;
            if (cart != null)
            {
                int index = IndexOfProduct(id);
                if (index == -1)
                {
                    return BadRequest();
                }
                cart.RemoveAt(index);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                cartCount = cart.Select(x => x.Quantity).Sum();
            }
            return new JsonResult(new { count = cartCount });
        }

        public IActionResult Checkout()
        {
            RefreshCartProducts();
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            foreach (var cartItem in cart)
            {
                if (cartItem.Product.Stock < cartItem.Quantity)
                {
                    var errorMessage = "Error: Insufficient stock for " + cartItem.Product.Name + "!";
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "errorMessage", errorMessage);
                    return Index();
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}