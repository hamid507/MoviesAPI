using Business.Dtos;
using Business.Dtos.Imdb.Models;
using Business.Models;
using Domain.Entities.Data;
using System;
using System.Collections.Generic;

namespace Business.Abstractions.Services
{
    public interface IMovieService
    {
        ServiceResult<IEnumerable<SearchResult>> SearchFilmByTitle(string title);
        ServiceResult<Guid> AddToWatchList(WatchItemDto watchItemDto);
        ServiceResult<Guid> AddNewMovie(string movieId);
        ServiceResult<MovieDto> GetMovieById(Guid movieId);
        ServiceResult<MovieDto> GetMovieByImdbMovieId(string imdbMovieId);
        ServiceResult<bool> MarkMovieAsWatched(Guid userId, Guid movieId);
        ServiceResult<List<string>> GetAllMovieNames();
        ServiceResult<double> GetImdbRatingByTitle(string title);
    }
}
