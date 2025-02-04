using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAutopark.Models;
using WebAutopark.Exceptions;
using WebAutopark.Data.Repositories.Interfaces;

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
            var components = await _componentRepository.GetAll();
            return View(components);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var component = await _componentRepository.Get(id);

            if (component == null)
                throw new NotFoundException($"There is no component with such id - {id}");

            return View(component);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Component component)
        {
            await _componentRepository.Delete(component);

            return RedirectToAction(nameof(Index));
        }
    }
}
