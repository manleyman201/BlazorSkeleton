using BlazorSkeleton.Data.Models;
using BlazorSkeleton.Data.Services;
using BlazorSkeleton.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlazorSkeleton.Server.Controllers
{
    /// <summary>
    /// REST endpoints for orders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        private readonly ICustomerService _customerService;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger, ICustomerService customerService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _customerService = customerService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET api/orders
        [HttpGet]
        [Route("GetOrders")]
        public async Task<ActionResult<List<Order>>> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetOrders();
                return Ok(orders);

                // return Ok(await _orderService.GetOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} -> Unable to get orders: {Message}", nameof(GetOrders), ex.Message);
                return Problem("Unable to get orders.");
            }
        }

        // GET api/orders/1001
        [HttpGet]
        [Route("GetOrderById/{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return order is null ? NotFound() : Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method} -> Unable to get order {OrderId}: {Message}", nameof(GetOrderById), id, ex.Message);
                return Problem($"Unable to get order {id}.");
            }
        }
        [HttpGet]
        [Route("GetCustomersAll")]
        public async Task<ActionResult<Customer>> GetCustomersAll()


        {

            var customers = await _customerService.GetAll();
            return customers is null ? NotFound() : Ok(customers);
        }

        [HttpPost]
        [Route("SaveOrder")]
        public async Task<ActionResult> SaveOrder([FromBody] Order order)
        {
            var saveOrder = await _orderService.SaveOrder(order);
            return Ok("Succesfully Saved");
        }
        


    }
}
