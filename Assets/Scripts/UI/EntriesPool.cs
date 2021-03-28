using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EntriesPool
    {
        private readonly EntrySettings _entrySettings;
        private readonly EntriesContainer _container;
        private readonly GameObject _singleElementView;
        private readonly List<GameObject> _entries;

        public EntriesPool(EntrySettings settings, EntriesContainer container, 
                                GameObject singleElementView, int entriesNumber) {
            _entrySettings = settings;
            _container = container;
            _singleElementView = singleElementView;
            _entries = new List<GameObject>(entriesNumber);
            CreateEntries(entriesNumber);
        }

        public void SetPoolActive(bool isActive) {
            foreach (var entry in _entries) {
                entry.SetActive(isActive);
            }
        }

        public void SetEntriesData(Dictionary<string, int> headers, List<List<string>> entriesData) {
            int headerIdx;
            string entryValue;
            GameObject entryGo;
            int headerEntryIndex;
            for (var i = 0; i < entriesData.Count; ++i) {
                headerEntryIndex = 0;
                entryGo = _entries[i];
                foreach (var header in _entrySettings.HeadersToUse) {
                    if (headers.TryGetValue(header.headerName, out headerIdx)) {
                        entryValue = entriesData[i][headerIdx];
                        SetEntryItemValue(entryGo.transform.GetChild(headerEntryIndex).gameObject,
                                            header.headerName, 
                                            entryValue);
                        ++headerEntryIndex;
                    }
                }
                entryGo.SetActive(true);
            }
        }

        private void SetEntryItemValue(GameObject entry, string headerName, string value) {
            var textMesh = entry.GetComponent<TextMeshProUGUI>();
            textMesh.text = value;
        }

        private void CreateEntries(int entriesNumber) {
            for (var i = 0; i < entriesNumber; ++i) {
                var entryGo = Object.Instantiate(_entrySettings.EntryPrefab, _container.transform);
                entryGo.name = "Entry_" + i;
                entryGo.SetActive(false);
                SetEntryButtonAction(entryGo.GetComponent<Button>());
                foreach (var header in _entrySettings.HeadersToUse) 
                    CreateEntryItem(entryGo, header);

                _entries.Add(entryGo);
            }
        }
        
        private void SetEntryButtonAction(Button entryButton) {
            entryButton.onClick.AddListener(() => _singleElementView.SetActive(true));
        }

        private void CreateEntryItem(GameObject entry, HeaderSetting header) {
            var go = new GameObject(header.headerName);
            go.transform.SetParent(entry.transform);
            var textMesh = go.AddComponent<TextMeshProUGUI>();
            textMesh.text = header.headerName;
            
            var itemSettings = _entrySettings.ItemSettings;
            if (itemSettings != null) {
                textMesh.font = itemSettings.Font;
                textMesh.fontSize = itemSettings.FontSize;
                textMesh.alignment = itemSettings.Alignment;
                textMesh.color = itemSettings.Color;
                textMesh.enableWordWrapping = true;//TODO: maybe we want a prefab
            }
            
            var entryRect = go.GetComponent<RectTransform>(); 
            entryRect.sizeDelta = new Vector2( _container.GetWidth(header.width), entryRect.sizeDelta.y);
        }
    }
}