using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAutopark.Models;
using WebAutopark.Exceptions;
using WebAutopark.Data.Repositories.IRepositories;

namespace WebAutopark.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IComponentRepository _componentRepository;

        public ComponentController(IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var components = await _componentRepository.GetAllAsync();

            return View(components);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var component = await _componentRepository.GetAsync(id)
                ?? throw new NotFoundException($"Component withd ID {id} not found.");

            return View(component);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Component component)
        {
            await _componentRepository.DeleteAsync(component);

            return RedirectToAction(nameof(Index));
        }
    }
}
