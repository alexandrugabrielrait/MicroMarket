using MicroMarket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicroMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IRepositoryManager _repositoryManager;

        public ProductController(ILogger<ProductController> logger, IRepositoryManager repositoryManager)
        {
            _logger = logger;
            _repositoryManager = repositoryManager;
        }

        public IActionResult Index(int id)
        {
            var product = ((IRepository<Product>)_repositoryManager.Get(typeof(Product))).Get(id);
            ViewBag.ProductType = ((IRepository<ProductType>)_repositoryManager.Get(typeof(ProductType))).Get(product.ProductTypeId);
            return View(product);
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