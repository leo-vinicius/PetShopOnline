namespace PetShopPro.Api.Shared.DTOs;

public record LoginRequest(string Email, string Senha);

public record LoginResponse(
    string Token,
    string TipoUsuario,
    int UserId,
    string Nome,
    string Email
);

public record ApiResponse<T>(
    bool Success,
    T? Data = default,
    string? Message = null,
    List<string>? Errors = null
);

public static class ApiResponseExtensions
{
    public static ApiResponse<T> ToSuccessResponse<T>(this T data, string? message = null)
        => new(true, data, message);

    public static ApiResponse<T> ToErrorResponse<T>(string message, List<string>? errors = null)
        => new(false, default, message, errors);
}
