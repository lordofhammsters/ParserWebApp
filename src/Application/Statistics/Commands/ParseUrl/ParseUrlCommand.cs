using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Statistics.Queries.GetStatisticsByUrl;
using Domain.Entities.Statistics;
using HtmlAgilityPack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Statistics.Commands.ParseUrl;

public class ParseUrlCommand : IRequest<StatisticsItems>
{
    public string Url { get; set; }

    public class ParseUrlCommandHandler : IRequestHandler<ParseUrlCommand, StatisticsItems>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<ParseUrlCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        private readonly string[] _separators =
            {" ", ",", ".", "! ", "?", "\"", ";", ":", "[", "]", "(", ")", "\n", "\r", "\t"};

        public ParseUrlCommandHandler(IHttpClientFactory clientFactory, 
                                      ILogger<ParseUrlCommandHandler> logger,
                                      IMediator mediator,
                                      IApplicationDbContext context)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _mediator = mediator;
            _context = context;
        }
        
        public async Task<StatisticsItems> Handle(ParseUrlCommand command, CancellationToken cancellationToken)
        {
            var url = command.Url.ToLower();

            var statistics = await _mediator.Send(new GetStatisticsByUrlQuery() {Url = url}, cancellationToken);
            if (statistics != null && statistics.Items != null && statistics.Items.Count > 0)
                return statistics;
            
            var dictionary = await ParseSiteToWordsAsync(url, cancellationToken);
            if (dictionary == null || dictionary.Count <= 0) 
                return new StatisticsItems();
            
            var items = new List<StatisticsItemDto>();
                
            var site = new StatisticSite() {Url = url, Created = DateTime.Now};
            
            foreach (var item in dictionary)
            {
                var siteWord = new StatisticSiteWord()
                {
                    StatisticSiteId = site.Id,
                    Word = item.Key,
                    Count = item.Value
                };
                site.SiteWords.Add(siteWord);
                
                items.Add(new StatisticsItemDto(){Word = item.Key, Count = item.Value});
            }
            _context.StatisticSites.Add(site);
            await _context.SaveChangesAsync(cancellationToken);

            return new StatisticsItems() {Items = items};
        }

        private async Task<Dictionary<string, int>?> ParseSiteToWordsAsync(string url, CancellationToken cancellationToken)
        {
            var html = await GetSiteHtmlAsync(url, cancellationToken);
            if (string.IsNullOrWhiteSpace(html))
                return null;
            
            var text = GetOnlyTextWithoutHtml(html);

            var words = text.Split(_separators, StringSplitOptions.RemoveEmptyEntries);
            var dictionary = new Dictionary<string, int>();

            foreach (var word in words)
            {
                var wordLowerCase = word.ToLower();

                if (dictionary.TryGetValue(wordLowerCase, out var value))
                    dictionary[wordLowerCase] = value + 1;
                else
                    dictionary.Add(wordLowerCase, 1);
            }

            return dictionary;
        }

        private async Task<string> GetSiteHtmlAsync(string url, CancellationToken cancellationToken)
        {
            try
            {
                var client = _clientFactory.CreateClient("parser");

                var response = await client.GetAsync(url, cancellationToken);

                response.EnsureSuccessStatusCode();
            
                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Can't get {url}");
                throw new BlException($"Возникла ошибка при запросе к {url}.");
            }
        }
        
        private string GetOnlyTextWithoutHtml(string html)
        {
            try
            {
                //HtmlDocument.MaxDepthLevel = Int32.MaxValue;
                
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc.DocumentNode.InnerText;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Can't parse html to text");
                throw new BlException($"Не удалось распарсить html в обычный текст");
            }
        }
    }
}