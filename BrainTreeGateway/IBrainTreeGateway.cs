using Braintree;

namespace BrainTreeGateway;

public interface IBrainTreeGateway
{
    IBraintreeGateway CreateGateway();
    IBraintreeGateway GetGateway();
}