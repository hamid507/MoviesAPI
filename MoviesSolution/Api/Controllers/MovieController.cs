using Api.Extensions;
using Business.Abstractions.Services;
using Business.Dtos;
using Business.Dtos.Imdb.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private IMovieService _service;

        public MovieController(IMovieService movieService)
        {
            _service = movieService;
        }

        [Route("SearchByTitle")]
        [HttpGet]
        public IActionResult SearchFilmByTitle(string title)
        {
            var serviceResult = _service.SearchFilmByTitle(title);

            return this.GenerateActionResult(serviceResult);
        }

        [Route("AddNew")]
        [HttpPost]
        public IActionResult AddNewMovie(string imdbMovieId)
        {
            var serviceResult = _service.AddNewMovie(imdbMovieId);

            return this.GenerateActionResult(serviceResult);
        }

        [Route("AddToWatchList")]
        [HttpPost]
        public IActionResult AddToWatchList(WatchItemDto watchlistDto)
        {
            var serviceResult = _service.AddToWatchList(watchlistDto);

            return this.GenerateActionResult(serviceResult);
        }

        [Route("GetById")]
        [HttpGet]
        public IActionResult GetMovieById(Guid movieId)
        {
            var serviceResult = _service.GetMovieById(movieId);

            return this.GenerateActionResult(serviceResult);
        }

        [Route("GetByImdbMovieId")]
        [HttpGet]
        public IActionResult GetMovieByImdbMovieId(string imdbMovieId)
        {
            var serviceResult = _service.GetMovieByImdbMovieId(imdbMovieId);

            return this.GenerateActionResult(serviceResult);
        }

        [Route("GetAllNames")]
        [HttpGet]
        public IActionResult GetAllMovies()
        {
            var serviceResult = _service.GetAllMovieNames();

            return this.GenerateActionResult(serviceResult);
        }

        [Route("MarkMovieWatched")]
        [HttpPost]
        public IActionResult MarkMovieWatched(Guid userId, Guid movieId)
        {
            var serviceResult = _service.MarkMovieAsWatched(userId, movieId);

            return this.GenerateActionResult(serviceResult);
        }

        [Route("GetImdbRatingByTitle")]
        [HttpPost]
        public IActionResult MarkMovieWatched(string title)
        {
            var serviceResult = _service.GetImdbRatingByTitle(title);

            return this.GenerateActionResult(serviceResult);
        }
    }
}
