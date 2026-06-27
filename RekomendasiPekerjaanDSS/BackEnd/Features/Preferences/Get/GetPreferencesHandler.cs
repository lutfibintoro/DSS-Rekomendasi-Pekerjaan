using BackEnd.Domain;
using BackEnd.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Features.Preferences.Get;

public record GetPreferencesQuery(int UserId) : IRequest<List<PreferensiDto>>;

public record PreferensiDto(int PreferenceId, int UserId, string KriteriaId, string NilaiAsli, short NilaiBobot);

public class GetPreferencesHandler : IRequestHandler<GetPreferencesQuery, List<PreferensiDto>>
{
    private readonly AppDbContext _db;
    public GetPreferencesHandler(AppDbContext db) => _db = db;

    public async Task<List<PreferensiDto>> Handle(GetPreferencesQuery req, CancellationToken ct)
    {
        var list = await _db.PreferensiUser.AsNoTracking()
            .Where(p => p.UserId == req.UserId)
            .OrderBy(p => p.KriteriaId)
            .ToListAsync(ct);

        if (list.Count == 0)
            throw new KeyNotFoundException("User belum memiliki preferensi");

        return list.Select(p => new PreferensiDto(
            p.PreferenceId, p.UserId, p.KriteriaId, p.NilaiAsli, p.NilaiBobot)).ToList();
    }
}