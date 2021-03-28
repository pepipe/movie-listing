using System;
using System.Collections;
using System.Collections.Generic;
using Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace UI {
  public class SingleEntryView : MonoBehaviour {
    [SerializeField] private MovieDBRestApi restApi;
    
    [Header("References")]
    [SerializeField] private Image headerImage;
    [SerializeField] private Image posterImage;
    [SerializeField] private TextMeshProUGUI movieTitle;
    [SerializeField] private TextMeshProUGUI movieYear;
    [SerializeField] private TextMeshProUGUI movieRating;
    [SerializeField] private TextMeshProUGUI movieDuration;
    [SerializeField] private TextMeshProUGUI[] genresText;
    
    [Header("CSV keys mapping")] 
    [SerializeField] private string movieTitleKey = "movie_title";
    [SerializeField] private string movieYearKey = "title_year";
    [SerializeField] private string movieRatingKey = "content_rating";
    [SerializeField] private string movieDurationKey = "duration";
    [SerializeField] private string movieGenresKey = "genres";
    [SerializeField] private string movieImdbLinkKey = "movie_imdb_link";

    private void OnEnable() {
      restApi.OnRequestEndEvent += ChangeImages;
    }

    private void OnDisable() {
      restApi.OnRequestEndEvent -= ChangeImages;
    }

    public void ShowEntry(Dictionary<string, int> headers, List<string> entry) {
      headers.TryGetValue(movieTitleKey, out var valueIndex);
      movieTitle.text = entry[valueIndex];
      
      headers.TryGetValue(movieYearKey, out valueIndex);
      movieYear.text = entry[valueIndex];
      
      headers.TryGetValue(movieRatingKey, out valueIndex);
      movieRating.text = entry[valueIndex];
      
      headers.TryGetValue(movieDurationKey, out valueIndex);
      movieDuration.text = ConvertDuration(entry[valueIndex]);

      headers.TryGetValue(movieGenresKey, out valueIndex);
      SetupGenres(entry[valueIndex]);
      
      headers.TryGetValue(movieImdbLinkKey, out valueIndex);
      GetImages(entry[valueIndex]);
      
      gameObject.SetActive(true);
    }

    private string ConvertDuration(string duration) {
      if (int.TryParse(duration, out int durationInMinutes)) {
        var hours = durationInMinutes / 60;
        var minutes = (durationInMinutes % 60);
        return hours + "h " + minutes + "m";
      }

      return String.Empty;
    }

    private void SetupGenres(string genres) {
      foreach (var genreObj in genresText)
        genreObj.gameObject.SetActive(false);

      var genresArray = genres.Split('|');
      for (int i = 0; i < genresText.Length; ++i) {
        if(i == genresArray.Length) break;

        genresText[i].text = genresArray[i];
        genresText[i].gameObject.SetActive(true);
      }
    }

    private void GetImages(string imdbUrl) {
      headerImage.gameObject.SetActive(false);
      posterImage.gameObject.SetActive(false);
      restApi.GenerateRequest(imdbUrl);
    }
    
    private void ChangeImages(MovieDbResponse response) {
      if(response == null || response.movie_results.Length == 0) return;
      
      var movieResult = response.movie_results[0];
      StartCoroutine(DownloadImage(restApi.ImagesBaseUrl + movieResult.backdrop_path, true));
      StartCoroutine(DownloadImage(restApi.ImagesBaseUrl + movieResult.poster_path, false));
    }

    private IEnumerator DownloadImage(string imageUrl, bool isHeader) {
      UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
      var image = isHeader ? headerImage : posterImage;
      yield return request.SendWebRequest();
      if(request.isNetworkError || request.isHttpError) 
        Debug.Log(request.error);
      else {
        // image.material.mainTexture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        var texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), 
                                      image.rectTransform.pivot, image.pixelsPerUnit);
        
        image.gameObject.SetActive(true);
      }
    }
  }
}
