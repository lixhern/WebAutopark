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
        public async Task<IActionResult> MakeOrder(int id, string[] names, int[] quantities)
        {
            if (names.Length != quantities.Length)
                throw new InvalidOperationException();

            var vehicle = await _vehicleRepository.Get(id) ?? throw new NotFoundException($"There is not vehicle with such id - {id}");
            var order = new Order
            {
                VehicleId = id,
                Date = DateTime.Now,
            };

            var orderId = await _orderRepository.Create(order);

            var componentIds = new List<int>();
            var orderItems = new List<OrderItem>();


            for(int i = 0; i < names.Length; i++)
            {
                var component = new Models.Component
                {
                    Name = names[i],
                };

                var componentId = await _componentRepository.Create(component);

                componentIds.Add(componentId);

                orderItems.Add(new OrderItem
                {
                    OrderId = orderId,
                    ComponentId = componentId,
                    Quantity = quantities[i]
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
                throw;
            }
            

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _orderRepository.Get(id);

            if (order == null)
                throw new NotFoundException($"There is no order with such id - {id}");

            return View(order);
        }


        [HttpPost]
        public async Task<ActionResult> Delete(Order order)
        {
            await _orderRepository.Delete(order);

            return RedirectToAction(nameof(Index));

        }
    }
}
