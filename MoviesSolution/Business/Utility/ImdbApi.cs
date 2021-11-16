using Business.Dtos.Imdb.Models;
using Infrastructure.Services;

namespace Business.Utility
{
    public static class ImdbApi
    {
        private static readonly string _mainUrl = AppSettings.GetAppConstant("ImdbApiUrl");
        private static readonly string _apiKey = AppSettings.GetAppConstant("ImdbApiKey");
        private static readonly string _lang = AppSettings.GetAppConstant("ImdbApiLang");

        public static SearchData SearchTitle(string expression)
        {
            string apiUrl = $"/{_lang}/API/SearchTitle/{_apiKey}/{expression}";

            var result = ApiUtils.SendGetRequestAsync<SearchData>(_mainUrl + apiUrl, null).Result;
            return result;
        }

        public static TitleData GetFilmDetails(string movieId, params string[] options)
        {
            var optionsString = string.Join(',', options);
            string apiUrl = $"/{_lang}/API/Title/{_apiKey}/{movieId}/{optionsString}";

            var result = ApiUtils.SendGetRequestAsync<TitleData>(_mainUrl + apiUrl, null).Result;
            return result;
        }
    }
}
