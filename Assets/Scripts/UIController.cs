using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _pagesText = null;
    [Tooltip("Suffix that will appear near the currPage / totalPages")]
    [SerializeField] private string _pagesTextSuffix = "Page: ";
    [SerializeField] private Button _prevButton = null;
    [SerializeField] private Button _nextButton = null;
    [SerializeField] private GameObject singleElementView = null;
    
    private SceneController _sceneController = null;

    private void Awake() {
        _sceneController = FindObjectOfType<SceneController>();
    }

    private void OnEnable() {
        _sceneController.PrevPageEvent += ChangePage;
        _sceneController.NextPageEvent += ChangePage;
    }

    private void OnDisable() {
        _sceneController.PrevPageEvent -= ChangePage;
        _sceneController.NextPageEvent -= ChangePage;
    }

    private void ChangePage(int currPage, int totalPages) {
        _prevButton.interactable = currPage != 0 ? true : false;
        _nextButton.interactable = currPage < totalPages - 1 ? true : false;
        _pagesText.text = _pagesTextSuffix + ++currPage + " / " + totalPages;
    }
}
