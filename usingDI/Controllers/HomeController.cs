using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using usingDI.Models;
using usingDI.Services;

namespace usingDI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly ISingleton singleton;
        private readonly IScoped scoped;
        private readonly ITransient transient;
        private readonly GuidService guidService;
        public HomeController(ILogger<HomeController> logger, IProductService productService,ISingleton singleton,IScoped scoped, ITransient transient,GuidService guidService)
        {
            _logger = logger;
            this.productService = productService;
            this.singleton = singleton;
            this.scoped = scoped;
            this.transient = transient;
            this.guidService = guidService;
        }

        public IActionResult Index()
        {
            var products=productService.GetProducts();
            return View(products);
        }

        public IActionResult Privacy()
        {
            ViewBag.Singleton = singleton.Guid.ToString();
            ViewBag.Transient = transient.Guid.ToString();
            ViewBag.Scoped = scoped.Guid.ToString();

            ViewBag.SingletonService = guidService.Singleton.Guid.ToString();
            ViewBag.TransientService = guidService.Transient.Guid.ToString();
            ViewBag.ScopedService = guidService.Scoped.Guid.ToString() ;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
