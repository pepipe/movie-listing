using MovieListing.Parser;
using UnityEngine;
using TMPro;

namespace MovieListing {
    public class ScreenController : MonoBehaviour {
        [SerializeField]
        private GameObject _parent = null;

        private IParser _parser;
        private IFileData _fileData;

        void Start() {
            _parser = new CsvParser();
            _fileData = _parser.ParseFromStreamingAssets("movie_metadata.csv");
            CreateData();
        }

        void CreateData() {
            var headers = _fileData.GetHeaders();
            foreach (var header in headers.Keys) {
                var obj = new GameObject(header);
                obj.transform.SetParent(_parent.transform);
                obj.AddComponent<TextMeshProUGUI>().text = header;
            }
        }
        // Update is called once per frame
        void Update() { }
    }
}