namespace HypeHubDAL.Exeptions;
public class InternalIdentityServerException : BaseException
{
    public InternalIdentityServerException(string msg) : base(msg) { }
    public InternalIdentityServerException(string msg, IEnumerable<string> errors) : base(msg, errors) { }
}
