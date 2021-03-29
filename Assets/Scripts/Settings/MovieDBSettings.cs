using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(menuName = "MovieListings/MovieDB Settings", order = 4, fileName = "MovieDBSettings")]
    public class MovieDBSettings : ScriptableObject {
        [SerializeField] private string apiKey = "2e6fc2dfd8039d7ff71f16631fe23c75";
        [SerializeField] private string baseUrl = "https://api.themoviedb.org/3/find/";
        [SerializeField] private string suffixUrl = "&language=en-US&external_source=imdb_id";
        [SerializeField] private string imagesBaseUrl = "https://image.tmdb.org/t/p/w300";

        public string APIKey => apiKey;
        public string BaseUrl => baseUrl;
        public string SuffixUrl => suffixUrl;
        public string ImagesBaseUrl => imagesBaseUrl;
    }
}