using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebAutopark.Data.Repositories;
using WebAutopark.Models;
using WebAutopark.Exceptions;

namespace WebAutopark.Controllers
{
    public class OrderController : Controller
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Models.Component> _componentRepository;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;

        public OrderController(IRepository<Order> orderRepository, IRepository<Models.Component> componentRepository, IRepository<OrderItem> orderItemRepository, IRepository<Vehicle> vehicleRepositroy)
        {
            _orderRepository = orderRepository;
            _componentRepository = componentRepository;
            _orderItemRepository = orderItemRepository;
            _vehicleRepository = vehicleRepositroy;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAll();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> MakeOrder(int id)
        {
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> MakeOrder(int id, List<(string, int)> components)
        {
            var vehicle = await _vehicleRepository.Get(id) ?? throw new NotFoundException($"There is not vehicle with such id - {id}");

            var order = new Order
            {
                VehicleId = id,
                Date = DateTime.Now,
            };

            var orderId = await _orderRepository.Create(order);

            var componentIds = new List<int>();
            var orderItems = new List<OrderItem>();

            foreach (var(name, quantity) in components)
            {
                var component = new Models.Component
                {
                    Name = name,
                };

                var componentId = await _componentRepository.Create(component);

                componentIds.Add(componentId);

                orderItems.Add(new OrderItem
                {
                    OrderId = orderId,
                    ComponentId = componentId,
                    Quantity = quantity
                });
            }

            try
            {
                foreach(var orderItem in orderItems)
                {
                    await _orderItemRepository.Create(orderItem);
                }
                
            }
            catch(Exception ex)
            {
                foreach(var componentId in componentIds)
                {
                    await _componentRepository.Delete(componentId);
                }
                await _orderItemRepository.Delete(orderId);
                throw ex;
            }
            

            return RedirectToAction(nameof(Index));
        }

/*        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
