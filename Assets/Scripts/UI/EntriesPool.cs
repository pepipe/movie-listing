using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EntriesPool
    {
        private readonly SceneController _controller;
        private readonly EntrySettings _entrySettings;
        private readonly EntriesContainer _container;
        private readonly List<GameObject> _entries;

        public EntriesPool(EntrySettings settings, EntriesContainer container, 
                                SceneController controller, int entriesNumber) {
            _entrySettings = settings;
            _container = container;
            _controller = controller;
            _entries = new List<GameObject>(entriesNumber);
            CreateEntries(entriesNumber);
        }

        public void SetPoolActive(bool isActive) {
            foreach (var entry in _entries) {
                entry.SetActive(isActive);
            }
        }

        public void SetEntriesData(Dictionary<string, int> headers, int startEntryIndex, List<List<string>> entriesData) {
            int headerIdx;
            string entryValue;
            GameObject entryGo;
            int headerEntryIndex;
            for (var i = 0; i < entriesData.Count; ++i) {
                headerEntryIndex = 0;
                entryGo = _entries[i];
                foreach (var itemSettings in _entrySettings.ItemsSettings) {
                    if (headers.TryGetValue(itemSettings.Header.headerName, out headerIdx)) {
                        entryValue = entriesData[i][headerIdx];
                        if (itemSettings.SplitEntryValue) {
                            entryValue = SplitValue(entryValue, itemSettings.SplitSetting);
                        }
                        SetEntryItemValue(entryGo.transform.GetChild(headerEntryIndex).gameObject, entryValue);
                        ++headerEntryIndex;
                    }
                }
                SetEntryButtonAction(entryGo.GetComponent<Button>(), startEntryIndex + i);
                entryGo.SetActive(true);
            }
        }

        private string SplitValue(string entryValue, SplitSetting itemSettingsSplitSetting) {
            var splits = entryValue.Split(itemSettingsSplitSetting.splitChar);
            if (splits.Length == 1) return entryValue;
            var result = "";
            for (var i = 0; i < itemSettingsSplitSetting.wordsToShow; ++i) {
                result += splits[i];
                if (i < itemSettingsSplitSetting.wordsToShow - 1)
                    result += " " + itemSettingsSplitSetting.splitChar + " ";
            }

            return result;
        }

        private void SetEntryItemValue(GameObject entry, string value) {
            var textMesh = entry.GetComponent<TextMeshProUGUI>();
            textMesh.text = value;
        }

        private void CreateEntries(int entriesNumber) {
            for (var i = 0; i < entriesNumber; ++i) {
                var entryGo = Object.Instantiate(_entrySettings.EntryPrefab, _container.transform);
                entryGo.name = "Entry_" + i;
                entryGo.SetActive(false);
                foreach (var itemSettings in _entrySettings.ItemsSettings) 
                    CreateEntryItem(entryGo, itemSettings);

                _entries.Add(entryGo);
            }
        }

        private void CreateEntryItem(GameObject entry, EntryItemSettings itemSettings) {
            var objPrefab = itemSettings.CustomEntryItemPrefab == null ? 
                                        _entrySettings.DefaultEntryItemPrefab : 
                                        itemSettings.CustomEntryItemPrefab;
            var go = GameObject.Instantiate(objPrefab, entry.transform, true);
            go.name = itemSettings.Header.headerName;
            var textMesh = go.GetComponent<TextMeshProUGUI>();
            textMesh.text = itemSettings.Header.headerName;

            var entryRect = go.GetComponent<RectTransform>(); 
            entryRect.sizeDelta = new Vector2( _container.GetWidth(itemSettings.Header.width), entryRect.sizeDelta.y);
        }
        
        private void SetEntryButtonAction(Button entryButton, int entryIndex) {
            entryButton.onClick.RemoveAllListeners();
            entryButton.onClick.AddListener(() => _controller.CallEntryClick(entryIndex));
        }
    }
}