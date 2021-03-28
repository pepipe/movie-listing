namespace Parser {
  public interface IParser {
    IFileData Parse(string pathToFile, string csvFileName);
    IFileData ParseFromStreamingAssets(string csvFileName);
    
    IFileData ParseFromResources(string csvFileName);
  }
}
