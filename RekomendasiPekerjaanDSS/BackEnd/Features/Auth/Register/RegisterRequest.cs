namespace BackEnd.Features.Auth.Register;

public record RegisterRequest(
    string Username,
    string Email,
    string Password,
    string Role
);