using System.Collections.Generic;

namespace Parser {
  public interface IFileData {
    /// <summary>
    /// Headers is a map with the name of header and the index in data
    /// </summary>
    /// <returns>Map of <string(header name), int(header position in data)></returns>
    Dictionary<string, int> GetHeaders();
    List<List<string>> GetEntries();
    List<string> GetEntry(int index);
    string GetValue(string header, int entryIndex);

    int EntriesCount();
  }
}
