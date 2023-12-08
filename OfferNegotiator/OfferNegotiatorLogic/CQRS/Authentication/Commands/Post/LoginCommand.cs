using MediatR;
using OfferNegotiatorLogic.DTOs.Login;

namespace OfferNegotiatorLogic.CQRS.Authentication.Commands.Post;

public class LoginCommand : IRequest<LoginReadDTO>
{
    public LoginCreateDTO LoginCreateDTO { get; init; }

    public LoginCommand(LoginCreateDTO loginCreateDTO)
    {
        LoginCreateDTO = loginCreateDTO;
    }
}
