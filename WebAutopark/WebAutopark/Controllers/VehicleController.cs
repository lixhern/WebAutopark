using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebAutopark.Data.Repositories;
using WebAutopark.Exceptions;
using WebAutopark.Models;


namespace WebAutopark.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IRepository<Vehicle> _vehicleRepository;

        public VehicleController(IRepository<Vehicle> vehicleRepository)
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
        public async Task<ActionResult> Create(Vehicle vehicle) //return ID Reposiroty
        {
            if (ModelState.IsValid)
            {
                if (await IsExist(vehicle))
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
            if (await IsExist(vehicle))
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

        private async Task<bool> IsExist(Vehicle vehicle) //this mb need to optimized
        {
            var isValid = (await _vehicleRepository.GetAll()).Any(v => v.RegistrationNumber == vehicle.RegistrationNumber);

            return isValid;
        }

        [HttpGet]
        public async Task<IActionResult> CheckForRegNumber(string RegistrationNumber)
        {
            var isValid = (await _vehicleRepository.GetAll()).Any(v => v.RegistrationNumber.Equals(RegistrationNumber));

            return Json(!isValid);
        }
    }
}
