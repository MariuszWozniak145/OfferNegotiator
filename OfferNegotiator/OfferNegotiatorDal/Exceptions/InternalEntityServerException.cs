namespace OfferNegotiatorDal.Exceptions;

public class InternalEntityServerException : BaseException
{
    public InternalEntityServerException(string msg) : base(msg) { }
    public InternalEntityServerException(string msg, IEnumerable<string> errors) : base(msg, errors) { }
}