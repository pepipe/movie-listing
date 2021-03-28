using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [Serializable]
    public struct HeaderSetting {
        public string headerName;
        [Tooltip("Width of the header in percentage.")]
        public float width;
    }
    
    [CreateAssetMenu(menuName = "MovieListings/Entry Settings", order = 1, fileName = "EntrySettings")]
    public class EntrySettings : ScriptableObject {
        [SerializeField] private GameObject entryPrefab = null;
        [SerializeField] private EntryItemSettings itemSettings = null;
        [SerializeField] private List<HeaderSetting> headersToUse = null;
        
        public GameObject EntryPrefab => entryPrefab;
        public EntryItemSettings ItemSettings => itemSettings;
        public IEnumerable<HeaderSetting> HeadersToUse => headersToUse;
    }
}