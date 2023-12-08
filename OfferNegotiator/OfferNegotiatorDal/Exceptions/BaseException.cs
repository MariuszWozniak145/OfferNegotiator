namespace HypeHubDAL.Exeptions;
public class BaseException : Exception
{
    public List<string> Errors { get; set; } = new();
    public BaseException(string msg) : base(msg) { }
    public BaseException(string msg, IEnumerable<string> errors) : base(msg)
    {
        foreach (var error in errors)
        {
            Errors.Add(error);
        }
    }
}
