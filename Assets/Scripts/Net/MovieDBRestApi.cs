using System.Collections;
using Settings;
using UnityEngine;
using UnityEngine.Networking;

namespace Net {
  public class MovieDBRestApi : MonoBehaviour
  {
    public delegate void OnRequestEnd(MovieDbResponse response);
    
    public event OnRequestEnd OnRequestEndEvent;
    
    public string ImagesBaseUrl => apiSettings.ImagesBaseUrl;

    [SerializeField] private MovieDBSettings apiSettings;

    public void GenerateRequest(string movieImdbUrl) {
      var movieId = GetMovieId(movieImdbUrl);
      if(movieId != null)
        StartCoroutine(ProcessRequest(BuildURL(movieId)));
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
      return apiSettings.BaseUrl + moviedImdbId + "?api_key=" + apiSettings.APIKey + apiSettings.SuffixUrl;
    }
  }
}
