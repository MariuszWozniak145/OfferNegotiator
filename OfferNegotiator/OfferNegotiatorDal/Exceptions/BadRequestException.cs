namespace OfferNegotiatorDal.Exceptions;

public class BadRequestException : BaseException
{
    public BadRequestException(string msg) : base(msg) { }
    public BadRequestException(string msg, IEnumerable<string> errors) : base(msg, errors) { }
}