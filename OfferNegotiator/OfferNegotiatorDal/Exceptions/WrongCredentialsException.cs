namespace OfferNegotiatorDal.Exceptions;

public class WrongCredentialsException : BaseException
{
    public WrongCredentialsException(string msg) : base(msg) { }
    public WrongCredentialsException(string msg, IEnumerable<string> errors) : base(msg, errors) { }
}