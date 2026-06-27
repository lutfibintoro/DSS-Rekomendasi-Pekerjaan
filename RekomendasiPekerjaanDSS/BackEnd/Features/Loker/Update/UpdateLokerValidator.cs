using FluentValidation;

namespace BackEnd.Features.Loker.Update;

public class UpdateLokerValidator : AbstractValidator<UpdateLokerRequest>
{
    private static readonly HashSet<string> ValidKriteriaIds = ["C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10"];

    public UpdateLokerValidator()
    {
        RuleFor(x => x.NamaPerusahaan).MaximumLength(100).When(x => x.NamaPerusahaan is not null);
        RuleFor(x => x.PosisiTersedia).MaximumLength(100).When(x => x.PosisiTersedia is not null);
        RuleFor(x => x.Lokasi).MaximumLength(100).When(x => x.Lokasi is not null);

        When(x => x.NilaiPerKriteria is not null, () =>
        {
            RuleFor(x => x.NilaiPerKriteria!).NotEmpty().WithMessage("Minimal satu nilai kriteria");
            RuleForEach(x => x.NilaiPerKriteria!).ChildRules(item =>
            {
                item.RuleFor(x => x.KriteriaId)
                    .NotEmpty()
                    .Must(id => ValidKriteriaIds.Contains(id));
                item.RuleFor(x => x.NilaiRiil).NotEmpty().MaximumLength(255);
            });
        });
    }
}