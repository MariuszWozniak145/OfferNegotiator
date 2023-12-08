namespace OfferNegotiatorDal.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string msg) : base(msg) { }
    public NotFoundException(string msg, IEnumerable<string> errors) : base(msg, errors) { }
}
