using AutoMapper;
using Business.Abstractions.Services;
using Business.Utility;
using Business.Dtos;
using Business.Dtos.Imdb.Models;
using Business.Models;
using Business.Validations;
using DataAccess.Abstractions;
using Domain.Entities.Data;
using Domain.Entities.Lookup;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Business.ServiceImplementations
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ServiceResult<IEnumerable<SearchResult>> SearchFilmByTitle(string title)
        {
            var searchData = ImdbApi.SearchTitle(title);

            if (!string.IsNullOrEmpty(searchData?.ErrorMessage))
            {
                return ServiceResult<IEnumerable<SearchResult>>.Error(searchData?.ErrorMessage);
            }

            List<SearchResult> searchResultMovies = searchData.Results;

            return ServiceResult<IEnumerable<SearchResult>>.Ok(searchResultMovies);
        }

        public ServiceResult<Guid> AddToWatchList(WatchItemDto watchItemDto)
        {
            var watchItemValidator = new WatchItemValidator();
            var validationResult = watchItemValidator.Validate(watchItemDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<Guid>.Error($"Validation error: '{string.Join(Environment.NewLine, validationResult.Errors)}'");
            }

            bool userExists = _unitOfWork.UserRepository.Any(user => user.Id == watchItemDto.UserId);

            if (!userExists)
            {
                return ServiceResult<Guid>.NotFound($"No user with id = '{watchItemDto.UserId}' found in the database");
            }

            bool movieExists = _unitOfWork.MovieRepository.Any(movie => movie.Id == watchItemDto.MovieId);

            if (!movieExists)
            {
                return ServiceResult<Guid>.NotFound($"No movie with id = '{watchItemDto.MovieId}' found in the database");
            }

            var watchItem = _mapper.Map<WatchItem>(watchItemDto);
            _unitOfWork.WatchItemRepository.Add(watchItem);
            _unitOfWork.Commit();

            return ServiceResult<Guid>.Ok(watchItemDto.UserId, $"A new watchItem with movie id = '{watchItemDto.MovieId}' is added for the user id = '{watchItemDto.UserId}'");
        }

        public ServiceResult<Guid> AddNewMovie(string movieId)
        {
            bool movieExistsInDb = _unitOfWork.MovieRepository.Any(m => m.MovieId == movieId);

            if(movieExistsInDb)
            {
                return ServiceResult<Guid>.AlreadyExists($"A movie with movieId = '{movieId}' already exists in the database.");
            }

            TitleData titleData = ImdbApi.GetFilmDetails(movieId, "Wikipedia", "Posters", "Ratings");

            if (!string.IsNullOrEmpty(titleData?.ErrorMessage))
            {
                return ServiceResult<Guid>.Error(titleData?.ErrorMessage);
            }

            var poster = titleData.Posters.Posters[0];

            var movieDto = new MovieDto()
            {
                MovieId = movieId,
                ImdbRating = titleData.IMDbRating,
                PosterId = poster.Id,
                PosterUrl = poster.Link,
                Title = titleData.Title,
                WikiShortDescription = titleData.Wikipedia.PlotShort.PlainText
            };

            var movieValidator = new MovieValidator();
            var validationResult = movieValidator.Validate(movieDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<Guid>.Error($"Validation error: '{string.Join(Environment.NewLine, validationResult.Errors)}'");
            }

            var movie = _mapper.Map<Movie>(movieDto);

            _unitOfWork.MovieRepository.Add(movie);
            _unitOfWork.Commit();

            return ServiceResult<Guid>.Ok(movie.Id, $"A new movie added with id = '{movie.Id}'");
        }

        public ServiceResult<MovieDto> GetMovieById(Guid movieId)
        {
            Expression<Func<Movie, bool>> predicate = m => m.Id == movieId;

            bool movieExistsInDb = _unitOfWork.MovieRepository.Any(predicate);

            if (!movieExistsInDb)
            {
                return ServiceResult<MovieDto>.AlreadyExists($"A movie with Id = '{movieId}' not found in the database.");
            }

            var movie = _unitOfWork.MovieRepository.GetFirstOrDefault(predicate);

            var movieDto = _mapper.Map<MovieDto>(movie);

            var movieValidator = new MovieValidator();
            var validationResult = movieValidator.Validate(movieDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<MovieDto>.Error($"Validation error: '{string.Join(Environment.NewLine, validationResult.Errors)}'");
            }

            return ServiceResult<MovieDto>.Ok(movieDto);
        }

        public ServiceResult<MovieDto> GetMovieByImdbMovieId(string imdbMovieId)
        {
            Expression<Func<Movie, bool>> predicate = m => m.MovieId == imdbMovieId;

            bool movieExistsInDb = _unitOfWork.MovieRepository.Any(predicate);

            if (!movieExistsInDb)
            {
                return ServiceResult<MovieDto>.AlreadyExists($"A movie with movieId = '{imdbMovieId}' not found in the database.");
            }

            var movie = _unitOfWork.MovieRepository.GetFirstOrDefault(predicate);

            var movieDto = _mapper.Map<MovieDto>(movie);

            var movieValidator = new MovieValidator();
            var validationResult = movieValidator.Validate(movieDto);

            if (!validationResult.IsValid)
            {
                return ServiceResult<MovieDto>.Error($"Validation error: '{string.Join(Environment.NewLine, validationResult.Errors)}'");
            }

            return ServiceResult<MovieDto>.Ok(movieDto);
        }

        public ServiceResult<bool> MarkMovieAsWatched(Guid userId, Guid movieId)
        {
            bool userExists = _unitOfWork.UserRepository.Any(user => user.Id == userId);

            if (!userExists)
            {
                return ServiceResult<bool>.NotFound($"No user with id = '{userId}' found in the database");
            }

            bool movieExists = _unitOfWork.MovieRepository.Any(movie => movie.Id == movieId);

            if (!movieExists)
            {
                return ServiceResult<bool>.NotFound($"No movie with id = '{movieId}' found in the database");
            }

            Expression<Func<WatchItem, bool>> watchItemPredicate = (wl => wl.MovieId == movieId && wl.UserId == userId);

            bool watchListExists = _unitOfWork.WatchItemRepository.Any(watchItemPredicate);
            if (!watchListExists)
            {
                return ServiceResult<bool>.NotFound($"Movie with id = '{movieId}' is not in WatchList of user id = '{userId}");
            }

            var watchItem = _unitOfWork.WatchItemRepository.GetFirstOrDefault(watchItemPredicate);
            watchItem.Watched = true;

            _unitOfWork.WatchItemRepository.Update(watchItem);
            _unitOfWork.Commit();

            return ServiceResult<bool>.Success();
        }

        public ServiceResult<List<string>> GetAllMovieNames()
        {
            bool movieExistsInDb = _unitOfWork.MovieRepository.Any();

            if (!movieExistsInDb)
            {
                return ServiceResult<List<string>>.AlreadyExists($"No any movie found in the database.");
            }

            var movies = _unitOfWork.MovieRepository.GetAll();
            var movieNames = movies.Select(m => m.Title).ToList();

            return ServiceResult<List<string>>.Ok(movieNames);
        }
    }
}
