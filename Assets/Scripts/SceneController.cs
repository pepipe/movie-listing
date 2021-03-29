using System;
using System.Collections.Generic;
using System.Linq;
using Parser;
using Settings;
using UI;
using UnityEngine;

public class SceneController : MonoBehaviour {
    public delegate void ChangePageHander(int currPage, int totalPages);
    public delegate void OnEntryClick(int entryIndex);
    public delegate void OnBackButtonClick();

    public event ChangePageHander PrevPageEvent;
    public event ChangePageHander NextPageEvent;
    public event OnEntryClick OnEntryClickEvent;
    public event OnBackButtonClick OnBackClickEvent;

    [Tooltip("CSV file to load from Resources folder (without extension)")]
    [SerializeField] private string fileToLoad = "movie_metadata";
    [Tooltip("Container for the rows of data")]
    [SerializeField] private EntriesContainer parent;
    [SerializeField] private GameObject singleEntryView;
    [SerializeField] private EntrySettings entrySettings;
    [SerializeField] private int startPage;
    [SerializeField] private int entriesPerPage = 50;

    private IParser _parser;
    private IFileData _fileData;
    private EntriesPool _entriesPool;
    private int _currPage;
    private int _totalPages;

    private void Awake() {
        Debug.Assert(entrySettings != null, "Please assign an EntrySettings to SceneController");
        _entriesPool = new EntriesPool(entrySettings, parent, this, entriesPerPage);
        
        _parser = new CsvParser();
        _fileData = _parser.ParseFromResources(fileToLoad);
        _totalPages = _fileData.EntriesCount() % entriesPerPage == 0 ?
            _fileData.EntriesCount() / entriesPerPage : 
            (_fileData.EntriesCount() / entriesPerPage) + 1;
    }

    private void Start() {
        _currPage = startPage;
        PrevPageEvent?.Invoke(_currPage, _totalPages);//to init page text and disable prev button
        GetPage();
    }
    
    /// Used in button event
    public void HideSelectedEntry() {
        singleEntryView.SetActive(false);
        OnBackClickEvent?.Invoke();
    }
    
    /// Used in button event
    public void PrevPage() {
        if (_currPage - 1 < 0) return;

        --_currPage;
        GetPage();
        PrevPageEvent?.Invoke(_currPage, _totalPages);
    }
    
    /// Used in button event
    public void NextPage() {
        if (_currPage + 1 >= _totalPages) return;

        ++_currPage;
        GetPage();
        NextPageEvent?.Invoke(_currPage, _totalPages);
    }

    public void CallEntryClick(int entryIndex) {
        OnEntryClickEvent?.Invoke(entryIndex);
    }

    public List<string> GetEntry(int entryIndex) {
        return _fileData.GetEntry(entryIndex);
    }

    public Dictionary<string, int> GetEntriesHeaders() {
        return _fileData.GetHeaders();
    }

    private void GetPage() {
        _entriesPool.SetPoolActive(false);
        var pageStartIdx = _currPage * entriesPerPage;
        var pageEndIdx = Math.Min(pageStartIdx + entriesPerPage, _fileData.EntriesCount() - 1);
        
        _entriesPool.SetEntriesData(_fileData.GetHeaders(),
            pageStartIdx,
            _fileData.GetEntries()
                .Where((v, i) =>
                    i >= pageStartIdx && i < pageEndIdx)
                .ToList());
    }
}