using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UI
{
    public class BulkInfoPool {
        private readonly GameObject _entryPrefab;
        private readonly EntriesContainer _container;
        private readonly List<GameObject> _entries;

        public BulkInfoPool(GameObject bulkEntryPrefab,
                                EntriesContainer container,
                                int entriesNumber) {
            _entryPrefab = bulkEntryPrefab;
            _container = container;
            _entries = new List<GameObject>(entriesNumber);
            CreateEntries(entriesNumber);
        }

        public void SetEntriesData(List<string> headers, List<string> entry) {
            for(var i = 0; i < entry.Count; ++i) {
                try
                {
                    var entryGo = _entries[i];
                    entryGo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = headers[i];
                    entryGo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = entry[i];
                    entryGo.SetActive(true);
                }
                catch(Exception e)
                {
                    Debug.LogWarning("Problem setting entry. " + e.Message);
                }
            }
        }

        public void SetPoolActive(bool isActive) {
            foreach (var entry in _entries) {
                entry.SetActive(isActive);
            }
        }
        
        private void CreateEntries(int entriesNumber) {
            for (var i = 0; i < entriesNumber; ++i) {
                var entryGo = Object.Instantiate(_entryPrefab, _container.transform);
                entryGo.name = "Entry_" + i;
                entryGo.SetActive(false);
                _entries.Add(entryGo);
            }
        }
    }
}