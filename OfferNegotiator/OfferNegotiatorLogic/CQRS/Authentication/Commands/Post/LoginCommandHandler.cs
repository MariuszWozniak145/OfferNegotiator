using MediatR;
using Microsoft.AspNetCore.Identity;
using OfferNegotiatorDal.Exceptions;
using OfferNegotiatorLogic.DTOs.Login;
using OfferNegotiatorLogic.Services.Interfaces;

namespace OfferNegotiatorLogic.CQRS.Authentication.Commands.Post;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginReadDTO>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(UserManager<IdentityUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<LoginReadDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var username = request.LoginCreateDTO.Username;
        var password = request.LoginCreateDTO.Password;
        var managedUser = await _userManager.FindByNameAsync(username) ?? throw new WrongCredentialsException("Wrong credentials", new List<string>() { $"There is no user with the given username: {username}." });
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, password);
        if (!isPasswordValid) throw new WrongCredentialsException("Wrong credentials", new List<string>() { "Wrong password." });
        var roles = await _userManager.GetRolesAsync(managedUser);
        var accessToken = _tokenService.CreateToken(managedUser, roles);
        var userId = Guid.Parse(managedUser.Id);
        return new LoginReadDTO(userId, managedUser.UserName, accessToken);
    }
}