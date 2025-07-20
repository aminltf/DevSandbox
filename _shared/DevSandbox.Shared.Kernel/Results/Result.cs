namespace DevSandbox.Shared.Kernel.Results;

public class Result<T> : IResult<T>
{
    public bool Succeeded { get; private set; }
    public string[] Messages { get; private set; } = Array.Empty<string>();
    public T? Data { get; private set; }

    private Result(bool succeeded, T? data, string[] messages)
    {
        Succeeded = succeeded;
        Data = data;
        Messages = messages;
    }

    public static Result<T> Success(T data, params string[] messages)
        => new Result<T>(true, data, messages);

    public static Result<T> Failure(params string[] messages)
        => new Result<T>(false, default, messages);

    // async helpers (optional)
    public static Task<Result<T>> SuccessAsync(T data, params string[] messages)
        => Task.FromResult(Success(data, messages));
    public static Task<Result<T>> FailureAsync(params string[] messages)
        => Task.FromResult(Failure(messages));
}
