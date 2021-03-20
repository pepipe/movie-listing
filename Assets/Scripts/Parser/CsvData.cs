using System.Collections.Generic;
using System.Linq;

namespace MovieListing.Parser {

  public class CsvData : IFileData {
    private readonly Dictionary<string, int> _headers;
    private readonly List<List<string>> _data;

    public CsvData(IReadOnlyList<List<string>> data) {
      _headers = data[0].Select(
                          (value, idx) => (value, idx))
                            .ToDictionary(k => k.value, v => v.idx);
      _data = data.Skip(1).ToList();
    }

    public Dictionary<string, int> GetHeaders() {
      return _headers;
    }

    public List<string> GetEntry(int index) {
      return index > 0 && index < _data.Count ? 
        _data[index] : 
        new List<string>();
    }

    public string GetValue(string header, int entryIndex) {
      var headerExists = _headers.TryGetValue(header, out var headerIdx);
      return headerExists && headerIdx > 0 && headerIdx < _data.Count ? 
                _data[entryIndex][headerIdx] : 
                string.Empty;
    }
  }
}
