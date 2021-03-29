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

    [SerializeField] private GeneralSettings _settings;
    [Tooltip("Container for the rows of data")]
    [SerializeField] private EntriesContainer parent;
    [SerializeField] private GameObject singleEntryView;
    [SerializeField] private int startPage;

    private IParser _parser;
    private IFileData _fileData;
    private EntriesPool _entriesPool;
    private int _currPage;
    private int _totalPages;

    private void Awake() {
        Debug.Assert(_settings != null, "Please assign Settings to SceneController");
        _entriesPool = new EntriesPool(_settings.EntrySettings, parent, this, _settings.EntriesPerPage);
        
        _parser = new CsvParser();
        _fileData = _parser.ParseFromResources(_settings.FileToLoad);
        _totalPages = _fileData.EntriesCount() % _settings.EntriesPerPage == 0 ?
            _fileData.EntriesCount() / _settings.EntriesPerPage : 
            (_fileData.EntriesCount() / _settings.EntriesPerPage) + 1;
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
        var pageStartIdx = _currPage * _settings.EntriesPerPage;
        var pageEndIdx = Math.Min(pageStartIdx + _settings.EntriesPerPage, _fileData.EntriesCount() - 1);
        
        _entriesPool.SetEntriesData(_fileData.GetHeaders(),
            pageStartIdx,
            _fileData.GetEntries()
                .Where((v, i) =>
                    i >= pageStartIdx && i < pageEndIdx)
                .ToList());
    }
}