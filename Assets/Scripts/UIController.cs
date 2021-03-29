using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI pagesText;
    [Tooltip("Prefix that will appear near the currPage / totalPages")]
    [SerializeField] private string pagesTextPrefix = "Page: ";
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private SingleEntryView singleEntryView;
    
    private SceneController _sceneController;

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
        prevButton.interactable = currPage != 0;
        nextButton.interactable = currPage < totalPages - 1;
        pagesText.text = pagesTextPrefix + ++currPage + " / " + totalPages;
    }
    
    private void ShowSingleElement(int entryIndex) {
        singleEntryView.ShowEntry(_sceneController.GetEntriesHeaders(), _sceneController.GetEntry(entryIndex));
    }
}
