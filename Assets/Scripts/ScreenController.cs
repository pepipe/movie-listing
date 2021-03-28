using System.Linq;
using Parser;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour {
    [Tooltip("CSV file to load from Resources folder (without extension)")]
    [SerializeField] private string fileToLoad = "movie_metadata";
    [Tooltip("Container for the rows of data")]
    [SerializeField] private EntriesContainer parent = null;
    [SerializeField] private GameObject singleElementView = null;
    [SerializeField] private EntrySettings entrySettings = null;
    [SerializeField] private int numberOfEntriesPerPage = 50;

    private IParser _parser;
    private IFileData _fileData;
    private EntriesPool _entriesPool;
    private int _currStartIdx = 0;
        
    private void Awake() {
        Debug.Assert(entrySettings != null, "Please assign an EntrySettings to SceneController");
        _entriesPool = new EntriesPool(entrySettings, parent, singleElementView, numberOfEntriesPerPage);
    }

    private void Start() {
        //Read data 
        _parser = new CsvParser();
        _fileData = _parser.ParseFromResources(fileToLoad);
             
        // //Build UI elements
        _entriesPool.SetEntriesData(_fileData.GetHeaders(),
                                    _fileData.GetEntries()
                                                .Where((v, i) => 
                                                        _currStartIdx <= i && i < _currStartIdx + numberOfEntriesPerPage)
                                                .ToList());
        _entriesPool.SetPoolActive(true);
        // CreateData();
    }   
        
    /// <summary>
    /// Used in button event
    /// </summary>
    public void HideSelectedEntry() {
        singleElementView.SetActive(false);
    }

    private void CreateData() {
        int headerIdx;
        string entryValue;
        GameObject entryGo;
        for(var i = 0; i < numberOfEntriesPerPage; ++i){
            entryGo = Instantiate(entrySettings.EntryPrefab, parent.transform);
            SetEntryButtonAction(entryGo.GetComponent<Button>());
            foreach (var header in entrySettings.HeadersToUse) {
                if (_fileData.GetHeaders().TryGetValue(header.headerName, out headerIdx)) {
                    entryValue = _fileData.GetEntry(i)[headerIdx];
                    CreateEntryItem(entryGo, entryValue, header.width);
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
        var itemSettings = entrySettings.ItemSettings;
        if (itemSettings != null) {
            textMesh.font = itemSettings.Font;
            textMesh.fontSize = itemSettings.FontSize;
            textMesh.alignment = itemSettings.Alignment;
            textMesh.color = itemSettings.Color;
        }
            
        var entryRect = go.GetComponent<RectTransform>(); 
        entryRect.sizeDelta = new Vector2( parent.GetWidth(componentWidth), entryRect.sizeDelta.y);
    }

    private void SetEntryButtonAction(Button entryButton) {
        entryButton.onClick.AddListener(() => singleElementView.SetActive(true));
    }
}