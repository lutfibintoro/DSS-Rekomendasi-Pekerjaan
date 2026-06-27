using FluentValidation;

namespace BackEnd.Features.Preferences.Upsert;

public class UpsertPreferencesValidator : AbstractValidator<UpsertPreferencesRequest>
{
    private static readonly HashSet<string> ValidKriteriaIds = ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10"];

    public UpsertPreferencesValidator()
    {
        RuleFor(x => x.Preferences)
            .NotEmpty().WithMessage("Minimal harus ada satu preferensi");

        RuleForEach(x => x.Preferences).ChildRules(item =>
        {
            item.RuleFor(x => x.KriteriaId)
                .NotEmpty()
                .Must(id => ValidKriteriaIds.Contains(id))
                .WithMessage($"Kriteria id harus salah satu dari: {string.Join(", ", ValidKriteriaIds)}");

            item.RuleFor(x => x.NilaiAsli)
                .NotEmpty().WithMessage("Nilai asli wajib diisi");
        });
    }
}