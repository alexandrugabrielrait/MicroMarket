using MicroMarket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicroMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IRepository<Product> _productRepository;

        public ProductController(ILogger<ProductController> logger, IRepository<Product> productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public IActionResult Index(int id)
        {
            return View(_productRepository.Get(id));
            //var products = _productRepository.GetAll();
            //return View(products.Where(x => x.Id == id).FirstOrDefault());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}