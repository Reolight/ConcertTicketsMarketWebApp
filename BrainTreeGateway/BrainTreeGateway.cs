using System.Diagnostics.CodeAnalysis;
using Braintree;
using Microsoft.Extensions.Options;

namespace BrainTreeGateway;

public class BrainTreeGateway : IBrainTreeGateway
{
    private BrainTreeConfigurations _configurations { get; set; }
    private IBraintreeGateway? BraintreeGateway { get; set; }
    
    public BrainTreeGateway(IOptions<BrainTreeConfigurations> configurations)
    {
        _configurations = configurations.Value;
    }
    
    public IBraintreeGateway CreateGateway()
    {
        return new BraintreeGateway(
            _configurations.Environment,
            _configurations.MerchantId,
            _configurations.PublicKey,
            _configurations.PrivateKey
        );

    }

    public IBraintreeGateway GetGateway()
    {
        return BraintreeGateway ??= CreateGateway();
    }
}