
using MercadoPagoAPI.Entities;
using MercadoPagoAPI.Models.Order;
using MercadoPagoAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MercadoPagoAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _service;

    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Order order) =>
        Ok(await _service.Create(order));

    [HttpPost("refaund")]
    public async Task<IActionResult> Refaund([FromBody] RefaundRequest request)
    {
        await _service.Refaund(request);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAll());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id) =>
       Ok(await _service.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Order order)
    {
        await _service.Update(id, order);
        return NoContent();
    }
    [HttpPut("cancel/{orderId}")]
    public async Task<IActionResult> Cancel(string orderId)
    {
        await _service.Cancel(orderId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _service.Delete(id);
        return NoContent();
    }

}
