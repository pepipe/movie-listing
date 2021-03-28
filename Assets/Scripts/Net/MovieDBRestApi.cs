using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Net {
  public class MovieDBRestApi : MonoBehaviour
  {
    public delegate void OnRequestEnd(MovieDbResponse response);
    
    public event OnRequestEnd OnRequestEndEvent;
    
    public string ImagesBaseUrl => imagesBaseUrl;

    private string apiKey = "2e6fc2dfd8039d7ff71f16631fe23c75";
    [SerializeField] private string baseUrl = "https://api.themoviedb.org/3/find/";
    [SerializeField] private string suffixUrl = "&language=en-US&external_source=imdb_id";
    [SerializeField] private string imagesBaseUrl = "https://image.tmdb.org/t/p/w300";

    private SceneController _sceneController;

    public void GenerateRequest(string movieImdbUrl) {
      var movieId = GetMovieId(movieImdbUrl);
      if(movieId != null)
        StartCoroutine(ProcessRequest(BuildURL(movieId)));
    }

    private void Awake() {
      _sceneController = FindObjectOfType<SceneController>();
    }

    private string GetMovieId(string movieImdbUrl) {
      var split = movieImdbUrl.Split('/');

      return split.Length > 4 ? split[4] : null;
    }

    private IEnumerator ProcessRequest(string uri)
    {
      using (var request = UnityWebRequest.Get(uri)) {
        yield return request.SendWebRequest();

        if (request.isNetworkError)
        {
          Debug.Log(request.error);
        }
        else {
          OnRequestEndEvent?.Invoke(JsonUtility.FromJson<MovieDbResponse>(request.downloadHandler.text));
        }
      }
    }
  
    private string BuildURL(string moviedImdbId) {
      return baseUrl + moviedImdbId + "?api_key=" + apiKey + suffixUrl;
    }
  }
}
