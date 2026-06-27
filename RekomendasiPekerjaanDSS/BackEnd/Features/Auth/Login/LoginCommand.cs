using MediatR;

namespace BackEnd.Features.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;

public record LoginResult(int UserId, string Username, string Email, string Role);