﻿using Microsoft.AspNetCore.Mvc;
using WebAutopark.Models;
using WebAutopark.Exceptions;
using WebAutopark.Data.Repositories.Interfaces;

namespace WebAutopark.Controllers
{
    public class VehicleTypeController : Controller
    {
        private readonly IVehicleTypeRepository _vehicleTypeRepository;

        public VehicleTypeController(IVehicleTypeRepository vehicleTypeRepository) //new primary constructor
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vehicleTypes = await _vehicleTypeRepository.GetAll();
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
                var isExist = (await _vehicleTypeRepository.GetAll()).Any(vt => vt.Name == vehicleType.Name);

                if (isExist)
                    throw new AlreadyExistException($"There is already exist vehicle type with thi name \"{vehicleType.Name}\"");

                await _vehicleTypeRepository.Create(vehicleType);
                return RedirectToAction(nameof(Index));
            }

            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicleType = await _vehicleTypeRepository.Get(id);

            if (vehicleType == null)
                throw new NotFoundException($"There is no veh with such id {id}");

            return View(vehicleType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VehicleType vehicleType) 
        {
            Console.WriteLine($"{vehicleType.Name} {vehicleType.TaxCoefficient}");
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                Console.WriteLine($"{vehicleType.Name} {vehicleType.TaxCoefficient}");
                Console.WriteLine(ModelState.IsValid);
                await _vehicleTypeRepository.Update(vehicleType);
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vehType = await _vehicleTypeRepository.Get(id);

            if (vehType == null)
                throw new NotFoundException($"There is no veh with such id {id}");

            return View(vehType);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VehicleType item)
        {
            await _vehicleTypeRepository.Delete(item);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CheckVehicleTypeId(int VehicleTypeId)
        {
            var isValid = (await _vehicleTypeRepository.GetAll()).Any(vt => vt.VehicleTypeId == VehicleTypeId);

            return Json(isValid);
        }
    }
}
