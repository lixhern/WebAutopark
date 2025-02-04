using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using WebAutopark.Data.Repositories.Interfaces;
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

            var vehicle = (await _vehicleRepository.GetAll()).AsQueryable();

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
                if (await IsExist(vehicle.RegistrationNumber, vehicle.Model))
                    throw new AlreadyExistException($"There is already exist vehicle with this registration number - {vehicle.RegistrationNumber}");

                await _vehicleRepository.Create(vehicle);
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var vehicle = await _vehicleRepository.Get(id);

            if (vehicle == null)
                throw new NotFoundException($"There is no vehicle with such id - {id}");

            return View(vehicle);
        }

        [HttpPost]
        public async  Task<ActionResult> Edit(Vehicle vehicle)
        {
            if (!(await IsExist(vehicle.RegistrationNumber, vehicle.Model)))
                throw new AlreadyExistException($"There is already exist vehicle with this registration number - {vehicle.RegistrationNumber}");
            
            await _vehicleRepository.Update(vehicle);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var vehicle = await _vehicleRepository.Get(id);

            if (vehicle == null)
                throw new NotFoundException($"There is no vehicle with such id - {id}");

            return View(vehicle);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Vehicle vehicle)
        {
            await _vehicleRepository.Delete(vehicle);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> IsExist(string RegistrationNumber, string model) //this mb need to optimized //to DB regNumber write unique field
        {
            if (string.IsNullOrEmpty(RegistrationNumber))
                return true;

            var vehicle = await _vehicleRepository.GetByRegNumber(RegistrationNumber);

            Console.WriteLine(model);

            if (vehicle.Model.Equals(model))
            {
                return true;
            }
            
            return false;
        }

        [HttpGet]
        public async Task<IActionResult> CheckForRegNumber(string RegistrationNumber, string Model)
        {
            return Json(await IsExist(RegistrationNumber, Model));
        }
    }
}
