using System.Net;

namespace OfferNegotiatorLogic.DTOs.Exception;

public record ExceptionOccuredReadDTO
(
    string Msg,
    IEnumerable<string> Errors,
    HttpStatusCode StatusCode
);

