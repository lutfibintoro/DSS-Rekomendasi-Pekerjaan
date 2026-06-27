namespace BackEnd.Features.Preferences.Upsert;

public record PreferensiInputItem(string KriteriaId, string NilaiAsli);

public record UpsertPreferencesRequest(List<PreferensiInputItem> Preferences);