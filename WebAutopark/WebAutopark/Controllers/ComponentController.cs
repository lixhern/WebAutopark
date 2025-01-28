using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAutopark.Data.Repositories;
using WebAutopark.Models;
using WebAutopark.Exceptions;

namespace WebAutopark.Controllers
{
    public class ComponentController : Controller
    {
        private readonly IRepository<Component> _componentRepository;

        public ComponentController(IRepository<Component> componentRepository)
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


        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            var component = await _componentRepository.Get(id);
            if (component == null)
                throw new NotFoundException($"There is no component with such id - {id}");

            await _componentRepository.Delete(component);
            return RedirectToAction(nameof(Index));
        }
    }
}
