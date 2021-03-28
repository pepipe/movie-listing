namespace Net {
  [System.Serializable]
  public class MovieResult {
    public string backdrop_path;
    public string poster_path;
  }
  
  [System.Serializable]
  public class MovieDbResponse {
    public MovieResult[] movie_results;
  }
}