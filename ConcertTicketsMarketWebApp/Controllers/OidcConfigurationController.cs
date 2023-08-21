using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace ConcertTicketsMarketWebApp.Controllers;

public class OidcConfigurationController : Controller
{
    private readonly ILogger<OidcConfigurationController> _logger;
    private readonly IClientRequestParametersProvider _clientRequestParametersProvider;
    
    public OidcConfigurationController(
        IClientRequestParametersProvider parametersProvider,
        ILogger<OidcConfigurationController> logger, IClientRequestParametersProvider clientRequestParametersProvider)
    {
        _clientRequestParametersProvider = clientRequestParametersProvider;
        _logger = logger;
    }

    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        var parameters = _clientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
        _logger.LogInformation("Client with Id {ClientId} issued OIDC configs. Next data was given: {Data}",
            clientId, parameters);
        return Ok(parameters);
    }
}