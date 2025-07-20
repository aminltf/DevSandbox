namespace DevSandbox.Shared.Kernel.Results;

public interface IResult<out T>
{
    bool Succeeded { get; }
    string[] Messages { get; }
    T? Data { get; }
}
