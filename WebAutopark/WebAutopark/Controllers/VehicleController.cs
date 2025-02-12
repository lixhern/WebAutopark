using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using WebAutopark.Data.Repositories.IRepositories;
using WebAutopark.Exceptions;
using WebAutopark.Models;


namespace WebAutopark.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string sortBy = "id")
        {
            ViewBag.CurrentSort = sortBy;

            var sortExpression = new Dictionary<string, Func<IQueryable<Vehicle>, IOrderedQueryable<Vehicle>>>
            {
                {"id", data => data.OrderBy(d => d.VehicleId)},
                {"id_desc", data => data.OrderByDescending(d => d.VehicleId) },
                {"model", data => data.OrderBy(d => d.Model) },
                {"model_desc", data => data.OrderByDescending(d => d.Model) },
                {"vehicleType", data => data.OrderBy(d => d.VehicleTypeId) },
                {"vehicleType_desc", data => data.OrderByDescending(d => d.VehicleTypeId) },
                {"regNumber", data => data.OrderBy(d => d.RegistrationNumber) },
                {"regNumber_desc", data => data.OrderByDescending(d => d.RegistrationNumber) },
                {"weight", data => data.OrderBy(d => d.Weight) },
                {"weight_desc", data => data.OrderByDescending(d => d.Weight) },
                {"year", data => data.OrderBy(d => d.Year) },
                {"year_desc", data => data.OrderByDescending(d => d.Year) },
                {"mileage", data => data.OrderBy(d => d.Mileage) },
                {"mileage_desc", data => data.OrderByDescending(d => d.Mileage) },
                {"color", data => data.OrderBy(d => d.Color) },
                {"color_desc", data => data.OrderByDescending(d => d.Color) },
                {"fuelConsumption", data => data.OrderBy(d => d.FuelConsumption) },
                {"fuelConsumption_desc", data => data.OrderByDescending(d => d.FuelConsumption) }
            };

            var vehicle = (await _vehicleRepository.GetAllAsync()).AsQueryable();

            if (sortExpression.ContainsKey(sortBy))
            {
                vehicle = sortExpression[sortBy](vehicle);
            }


            return View(vehicle);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                if(!(await IsExist(vehicle.RegistrationNumber, vehicle.VehicleId)))
                    throw new AlreadyExistException($"Vehicle with registration number {vehicle.RegistrationNumber} already exists.");

                await _vehicleRepository.CreateAsync(vehicle);
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var vehicle = await _vehicleRepository.GetAsync(id)
                ?? throw new NotFoundException($"Vehicle with ID {id} not found.");

            return View(vehicle);
        }

        [HttpPost]
        public async  Task<ActionResult> Edit(Vehicle vehicle)
        {
            if (!(await IsExist(vehicle.RegistrationNumber, vehicle.VehicleId)))
                throw new AlreadyExistException($"Vehicle with registration number {vehicle.RegistrationNumber} already exists.");

            await _vehicleRepository.UpdateAsync(vehicle);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var vehicle = await _vehicleRepository.GetAsync(id)
                ?? throw new NotFoundException($"Vehicle with ID {id} not found.");

            return View(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Vehicle vehicle)
        {
            await _vehicleRepository.DeleteAsync(vehicle);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IsExist(string registrationNumber, int vehicleId)
        {
            if (string.IsNullOrWhiteSpace(registrationNumber))
                return false;

            var existingVehicle = await _vehicleRepository.GetByRegistrationNumberAsync(registrationNumber);
            bool isAvailable = existingVehicle == null || existingVehicle.VehicleId == vehicleId;

            return isAvailable;
        }

        [HttpGet]
        public async Task<IActionResult> IsRegistrationNumberAvaliable(string RegistrationNumber, int VehicleId)
        {
            return Json(await IsExist(RegistrationNumber, VehicleId));
        }
    }
}
