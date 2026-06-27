using MediatR;

namespace BackEnd.Features.Preferences.Reset;

public record ResetPreferencesCommand(int UserId) : IRequest<ResetPreferencesResult>;

public record ResetPreferencesResult(int DeletedCount);