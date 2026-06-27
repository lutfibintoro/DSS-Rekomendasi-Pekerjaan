using FluentValidation;

namespace BackEnd.Features.Loker.Create;

public class CreateLokerValidator : AbstractValidator<CreateLokerRequest>
{
    private static readonly HashSet<string> ValidKriteriaIds = ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10"];

    public CreateLokerValidator()
    {
        RuleFor(x => x.NamaPerusahaan).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PosisiTersedia).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Lokasi).NotEmpty().MaximumLength(100);

        RuleFor(x => x.NilaiPerKriteria).NotEmpty().WithMessage("Minimal satu nilai kriteria");

        RuleForEach(x => x.NilaiPerKriteria).ChildRules(item =>
        {
            item.RuleFor(x => x.KriteriaId)
                .NotEmpty()
                .Must(id => ValidKriteriaIds.Contains(id))
                .WithMessage("Kriteria id tidak valid");

            item.RuleFor(x => x.NilaiRiil).NotEmpty().MaximumLength(255);
        });
    }
}