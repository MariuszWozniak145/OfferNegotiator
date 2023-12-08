namespace OfferNegotiatorDal.Exceptions;

public class ValidationFailedException : BaseException
{
    public ValidationFailedException(string msg) : base(msg) { }
    public ValidationFailedException(string msg, IEnumerable<string> errors) : base(msg, errors) { }
}
