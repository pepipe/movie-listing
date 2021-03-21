using MovieListing.Parser;
using MovieListing.UI;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace MovieListing {
    public class ScreenController : MonoBehaviour {
        [SerializeField] private EntriesContainer _parent = null;
        [SerializeField] private GameObject _selectedEntry = null;
        [SerializeField] private EntrySettings _entrySettings = null;
        [SerializeField] private int _numberOfEntriesPerPage = 50;

        private IParser _parser;
        private IFileData _fileData;
        
        void Awake() {
            Debug.Assert(_entrySettings != null, "Please assign an EntrySettings to SceneController");
        }

        void Start() {
            _parser = new CsvParser();
            _fileData = _parser.ParseFromResources("movie_metadata");
            CreateData();
        }
        
        /// <summary>
        /// Used in button event
        /// </summary>
        public void HideSelectedEntry() {
            _selectedEntry.SetActive(false);
        }

        private void CreateData() {
            int headerIdx;
            string entryValue;
            GameObject entryGo;
            for(var i = 0; i < _numberOfEntriesPerPage; ++i){
                entryGo = GameObject.Instantiate(_entrySettings.EntryPrefab, _parent.transform);
                SetEntryButtonAction(entryGo.GetComponent<Button>());
                foreach (var header in _entrySettings.HeadersToUse) {
                    if (_fileData.GetHeaders().TryGetValue(header.HeaderName, out headerIdx)) {
                        entryValue = _fileData.GetEntry(i)[headerIdx];
                        CreateEntryItem(entryGo, entryValue, header.Width);
                    }else
                        Debug.LogWarning("Header '" + header + "' doesn't exist in the data headers.");
                }
            }
        }

        private void CreateEntryItem(GameObject entry, string value, float componentWidth) {
            var go = new GameObject(value);
            go.transform.SetParent(entry.transform);
            var textMesh = go.AddComponent<TextMeshProUGUI>();
            textMesh.text = value;
            var itemSettings = _entrySettings.ItemSettings;
            if (itemSettings != null) {
                textMesh.font = itemSettings.Font;
                textMesh.fontSize = itemSettings.FontSize;
                textMesh.alignment = itemSettings.Alignment;
                textMesh.color = itemSettings.Color;
            }
            
            var entryRect = go.GetComponent<RectTransform>(); 
            entryRect.sizeDelta = new Vector2( _parent.GetWidth(componentWidth), entryRect.sizeDelta.y);
        }

        private void SetEntryButtonAction(Button entryButton) {
            entryButton.onClick.AddListener(() => _selectedEntry.SetActive(true));
        }
    }
}