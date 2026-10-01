namespace UGetMore.Application.Common;

// Mirrors the frontend's existing success envelope (interfaces/product/response.ts:
// CustomResponse<T> { data, message, error }) so the ~25 endpoints/rest-api/*.ts wrapper files'
// success-path call sites need minimal rework. Failures use real HTTP status codes + ProblemDetails
// instead of this shape — see the API's ProblemDetails configuration in Program.cs.
public class CustomResponse<T>
{
    public required T Data { get; init; }
    public string Message { get; init; } = string.Empty;
    public bool Error { get; init; }

    public static CustomResponse<T> Ok(T data, string message = "") =>
        new() { Data = data, Message = message, Error = false };
}
