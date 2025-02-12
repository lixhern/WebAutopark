using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using WebAutopark.Models;
using WebAutopark.Exceptions;
using System.Net.Http.Headers;
using WebAutopark.Data.Repositories.IRepositories;

namespace WebAutopark.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public OrderController(
            IOrderRepository orderRepository, 
            IComponentRepository componentRepository, 
            IOrderItemRepository orderItemRepository, 
            IVehicleRepository vehicleRepositroy)
        {
            _orderRepository = orderRepository;
            _componentRepository = componentRepository;
            _orderItemRepository = orderItemRepository;
            _vehicleRepository = vehicleRepositroy;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            //var orders = await _orderRepository.GetAllInDetails();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetInDetailsAsync(id);
            if (order.Count() == 0)
                throw new NotFoundException($"Order with ID {id} not found.");

            return View(order);
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
                throw new ArgumentException($"Mismatch between component names ({names.Length}) and quantities ({quantities.Length}).");

            var vehicle = await _vehicleRepository.GetAsync(id) 
                ?? throw new NotFoundException($"There is not vehicle with such id - {id}");

            var orderId = await CreateOrder(id);

            List<int> componentIds = null;

            try
            {
                componentIds = await CreateOrderItems(orderId, names, quantities);
                
            }
            catch(Exception ex)
            {
                if(componentIds != null)
                {
                    await RollbackOrder(orderId, componentIds);
                }
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetAsync(id)
                ?? throw new NotFoundException($"There is no order with such id - {id}");

            return View(order);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Order order)
        {
            await _orderRepository.DeleteAsync(order);

            return RedirectToAction(nameof(Index));

        }

        private async Task<int> CreateOrder(int vehicleId)
        {
            var order = new Order
            {
                VehicleId = vehicleId,
                Date = DateTime.Now,
            };

            return await _orderRepository.CreateAsync(order);
        }

        private async Task<int> CreateComponent(string name)
        {
            WebAutopark.Models.Component component = new WebAutopark.Models.Component
            {
                Name = name,
            };

            return await _componentRepository.CreateAsync(component);
        }

        private async Task<List<int>> CreateOrderItems(int orderId, string[] names, int[] quantities)
        {
            var componentIds = new List<int>();
            var orderItems = new List<OrderItem>();

            for (int i = 0; i < names.Length; i++)
            {
                var componentId = await CreateComponent(names[i]);
                componentIds.Add(componentId);

                orderItems.Add(new OrderItem
                {
                    OrderId = orderId,
                    ComponentId = componentId,
                    Quantity = quantities[i]
                });
            }

            foreach(var orderItem in orderItems)
            {
                await _orderItemRepository.CreateAsync(orderItem);
            }

            return componentIds;
        }

        private async Task RollbackOrder(int orderId, List<int> componentIds)
        {
            foreach (var componentId in componentIds)
            {
                await _componentRepository.DeleteAsync(componentId);
            }

            await _orderItemRepository.DeleteAsync(orderId);
        }
    }
}
