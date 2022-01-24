using MicroMarket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MicroMarket.Controllers
{
    public class BrowseController : Controller
    {
        private readonly ILogger<BrowseController> _logger;
        private readonly IRepositoryManager _repositoryManager;
        private const int productsPerPage = 4;

        public BrowseController(ILogger<BrowseController> logger, IRepositoryManager repositoryManager)
        {
            _logger = logger;
            _repositoryManager = repositoryManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Page", new { id = 0 });
        }

        public IActionResult Page(int id, string startsWith = "")
        {
            if (id < 0)
                return RedirectToAction("Page", new { id = 0 });

            var products = ((IRepository<Product>)_repositoryManager.Get(typeof(Product))).GetAll().Where(x => x.Name.StartsWith(startsWith));
            var lastPageId = products.Count() / productsPerPage;
            if (id > lastPageId)
                return RedirectToAction("Page", new { id = lastPageId });

            var startIndex = id * productsPerPage;

            var endIndex = startIndex + productsPerPage;

            ViewBag.HTMLId = id;
            ViewBag.LastPageId = lastPageId;
            return View(products.Skip(startIndex).Take(endIndex - startIndex).ToList());
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