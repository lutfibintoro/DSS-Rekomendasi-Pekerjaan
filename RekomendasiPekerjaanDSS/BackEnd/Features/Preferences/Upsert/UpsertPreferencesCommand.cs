using MediatR;

namespace BackEnd.Features.Preferences.Upsert;

public record UpsertPreferencesCommand(int UserId, List<PreferensiInputItem> Preferences) : IRequest<List<PreferensiDto>>;

public record PreferensiDto(int PreferenceId, int UserId, string KriteriaId, string NilaiAsli, short NilaiBobot);