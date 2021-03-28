using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _pagesText = null;
    [Tooltip("Prefix that will appear near the currPage / totalPages")]
    [SerializeField] private string _pagesTextPrefix = "Page: ";
    [SerializeField] private Button _prevButton = null;
    [SerializeField] private Button _nextButton = null;
    [SerializeField] private SingleEntryView singleEntryView = null;
    
    private SceneController _sceneController = null;

    private void Awake() {
        _sceneController = FindObjectOfType<SceneController>();
    }

    private void OnEnable() {
        _sceneController.PrevPageEvent += ChangePage;
        _sceneController.NextPageEvent += ChangePage;
        _sceneController.OnEntryClickEvent += ShowSingleElement;
    }

    private void OnDisable() {
        _sceneController.PrevPageEvent -= ChangePage;
        _sceneController.NextPageEvent -= ChangePage;
        _sceneController.OnEntryClickEvent -= ShowSingleElement;
    }

    private void ChangePage(int currPage, int totalPages) {
        _prevButton.interactable = currPage != 0 ? true : false;
        _nextButton.interactable = currPage < totalPages - 1 ? true : false;
        _pagesText.text = _pagesTextPrefix + ++currPage + " / " + totalPages;
    }
    
    private void ShowSingleElement(int entryIndex) {
        singleEntryView.ShowEntry(_sceneController.GetEntriesHeaders(), _sceneController.GetEntry(entryIndex));
    }
}
