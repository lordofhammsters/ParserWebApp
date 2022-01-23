using FluentValidation;

namespace ParserWebApp.Application.Statistics.Commands.ParseUrl;

public class ParseUrlCommandValidator : AbstractValidator<ParseUrlCommand>
{
    public ParseUrlCommandValidator()
    {
        CascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Url).NotEmpty().WithMessage("Урл адрес не указан");
        RuleFor(x => x.Url)
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Укажите правильный урл адрес. Например, https://www.google.com");
        RuleFor(x => x.Url)
            .MaximumLength(2000)
            .WithMessage("Урл адрес не должен превышать 2000 знаков");
    }
}