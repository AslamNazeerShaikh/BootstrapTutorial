using System.Diagnostics;
using BootstrapTutorial.WebUi.Models;
using BootstrapTutorial.WebUi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BootstrapTutorial.WebUi.Controllers
{
    [Route("[controller]")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IGenericRepository<Product> _repository;

        public ProductController(ILogger<ProductController> logger, IGenericRepository<Product> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAllAsync();
            return View(products);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
