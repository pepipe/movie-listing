using System;
using System.Collections.Generic;
using UnityEngine;

namespace MovieListing.UI
{
    [Serializable]
    public struct HeaderSetting {
        public string HeaderName;
        [Tooltip("Width of the header in percentage.")]
        public float Width;
    }
    
    [CreateAssetMenu(menuName = "MovieListings/Entry Settings", order = 1, fileName = "EntrySettings")]
    public class EntrySettings : ScriptableObject {
        [SerializeField] private GameObject _entryPrefab = null;
        [SerializeField] private EntryItemSettings _itemSettings = null;
        [SerializeField] private List<HeaderSetting> _headersToUse = null;
        
        public GameObject EntryPrefab => _entryPrefab;
        public EntryItemSettings ItemSettings => _itemSettings;
        public IEnumerable<HeaderSetting> HeadersToUse => _headersToUse;
    }
}