using Business.Dtos;
using FluentValidation;

namespace Business.Validations
{
    public class MovieValidator : AbstractValidator<MovieDto>
    {
        public MovieValidator()
        {
            RuleFor(movie => movie)
                .NotNull();

            RuleFor(movie => movie.MovieId)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(180);

            RuleFor(movie => movie.PosterId)
                .NotEmpty();

            RuleFor(movie => movie.PosterUrl)
                .NotEmpty()
                .Must(s => s.Contains("imdb-api.com") && (s.EndsWith(".jpg") || s.EndsWith(".jpeg")));

            RuleFor(movie => movie.Title)
                .NotEmpty();

            RuleFor(movie => movie.ImdbRating)
                .NotEmpty();
        }
    }
}
