using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ConcertTicketsMarket.Controllers;

/// <summary>
/// Controller for buying
/// </summary>

[ApiController]
[Authorize]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private IBraintreeGateway _braintree { get; set; }
    public OrdersController(IBraintreeGateway braintree)
    {
        _braintree = braintree;
    }
   
    
    [HttpGet]
    public async Task<IActionResult> GetClientToken()
    {
        var clientToken = await _braintree.ClientToken.GenerateAsync();
        return Ok(clientToken);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment(string jsonInfo)
    {
        try
        {
            dynamic orderInfo = JsonConvert.DeserializeObject(jsonInfo)
                          ?? throw new InvalidOperationException();
            var request = new TransactionRequest
            {
                Amount = orderInfo.Amount,
                PaymentMethodNonce = orderInfo.MethodNonce,
                DeviceData = orderInfo.DeviceData,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> transactionResult = await _braintree.Transaction.SaleAsync(request);
            if (transactionResult.IsSuccess())
                return Ok();
            return BadRequest(transactionResult.Message);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}