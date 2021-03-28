﻿using System;
using System.Collections.Generic;
using System.Linq;
using Parser;
using UI;
using UnityEngine;

public class SceneController : MonoBehaviour {
    public delegate void ChangePageHander(int currPage, int totalPages);
    public delegate void OnEntryClick(int entryIndex);

    public event ChangePageHander PrevPageEvent;
    public event ChangePageHander NextPageEvent;
    public event OnEntryClick OnEntryClickEvent;

    [Tooltip("CSV file to load from Resources folder (without extension)")]
    [SerializeField] private string fileToLoad = "movie_metadata";
    [Tooltip("Container for the rows of data")]
    [SerializeField] private EntriesContainer parent = null;
    [SerializeField] private GameObject singleEntryView = null;
    [SerializeField] private EntrySettings entrySettings = null;
    [SerializeField] private int startPage = 0;
    [SerializeField] private int entriesPerPage = 50;

    private IParser _parser;
    private IFileData _fileData;
    private EntriesPool _entriesPool;
    private int _currPage = 0;
    private int _totalPages;

    private void Awake() {
        Debug.Assert(entrySettings != null, "Please assign an EntrySettings to SceneController");
        _entriesPool = new EntriesPool(entrySettings, parent, this, entriesPerPage);
    }

    private void Start() {
        //Read data 
        _parser = new CsvParser();
        _fileData = _parser.ParseFromResources(fileToLoad);
        _totalPages = _fileData.EntriesCount() % entriesPerPage == 0 ?
                        _fileData.EntriesCount() / entriesPerPage : 
                        (_fileData.EntriesCount() / entriesPerPage) + 1;
        
        // //Build UI elements
        _currPage = startPage;
        PrevPageEvent?.Invoke(_currPage, _totalPages);//to init page text and disable prev button
        GetPage();
    }

    /// <summary>
    /// Used in button event
    /// </summary>
    public void HideSelectedEntry() {
        singleEntryView.SetActive(false);
    }

    /// <summary>
    /// Used in button event
    /// </summary>
    public void PrevPage() {
        if (_currPage - 1 < 0) return;

        --_currPage;
        GetPage();
        PrevPageEvent?.Invoke(_currPage, _totalPages);
    }
    
    /// <summary>
    /// Used in button event
    /// </summary>
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