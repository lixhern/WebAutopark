using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAutopark.Data.Repositories;
using WebAutopark.Models;

namespace WebAutopark.Controllers
{
    public class OrderItemController : Controller
    {
        private readonly IRepository<OrderItem> _orderItemRepository;

        public OrderItemController(IRepository<OrderItem> orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }


        // GET: OrderItemController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderItemController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: OrderItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderItemController/Edit/5
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
        }

        // GET: OrderItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderItemController/Delete/5
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
