using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Statistics.Queries.GetStatisticsByUrl;

public class GetStatisticsByUrlQuery : IRequest<StatisticsItems>
{
    public string Url { get; set; }
}

public class GetStatisticsByUrlQueryHandler : IRequestHandler<GetStatisticsByUrlQuery, StatisticsItems>
{
    private readonly IApplicationDbContext _context;

    public GetStatisticsByUrlQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StatisticsItems> Handle(GetStatisticsByUrlQuery request, CancellationToken cancellationToken)
    {
        var url = request.Url.ToLower();

        var items = await _context.StatisticSiteWords
            .Include(x => x.Site)
            .Where(x => x.Site.Url == url)
            .OrderByDescending(x => x.Count)
            .Select(x => new StatisticsItemDto() {Word = x.Word, Count = x.Count})
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new StatisticsItems() {Items = items};
    }
}