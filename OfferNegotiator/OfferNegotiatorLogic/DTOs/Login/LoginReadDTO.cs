namespace OfferNegotiatorLogic.DTOs.Login;

public record LoginReadDTO
(
    Guid UserId,
    string UserName,
    string AccessToken
);