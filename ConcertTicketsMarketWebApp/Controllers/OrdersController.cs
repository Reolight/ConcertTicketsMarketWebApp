using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketsMarketWebApp.Controllers;

/// <summary>
/// Controller for buying
/// </summary>

[ApiController]
[Authorize]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    
}