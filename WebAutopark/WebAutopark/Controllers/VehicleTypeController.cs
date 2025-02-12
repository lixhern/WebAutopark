using Microsoft.AspNetCore.Mvc;
using WebAutopark.Models;
using WebAutopark.Exceptions;
using WebAutopark.Data.Repositories.IRepositories;

namespace WebAutopark.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public VehicleTypeController(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vehicleTypes = await _vehicleTypeRepository.GetAllAsync();

            return View(vehicleTypes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleType vehicleType) 
        {
            if (ModelState.IsValid)
            {
                var isExist = (await _vehicleTypeRepository.GetAllAsync()).Any(vt => vt.Name == vehicleType.Name);

                if (isExist)
                    throw new AlreadyExistException($"Vehicle type \"{vehicleType.Name}\" already exists.");

                await _vehicleTypeRepository.CreateAsync(vehicleType);
                return RedirectToAction(nameof(Index));
            }

            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleType = await _vehicleTypeRepository.GetAsync(id)
                ?? throw new NotFoundException($"No vehicle type found with ID {id}.");

            return View(vehicleType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleType vehicleType) 
        {
            if (ModelState.IsValid)
            {
                await _vehicleTypeRepository.UpdateAsync(vehicleType);
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vehType = await _vehicleTypeRepository.GetAsync(id)
                ?? throw new NotFoundException($"No vehicle type found with ID {id}.");

            return View(vehType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VehicleType item)
        {
            await _vehicleTypeRepository.DeleteAsync(item);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CheckVehicleTypeId(int VehicleTypeId)
        {
            var isValid = (await _vehicleTypeRepository.GetAllAsync()).Any(vt => vt.VehicleTypeId == VehicleTypeId);

            return Json(isValid);
        }
    }
}
