using System.Collections.Generic;
using UnityEngine;

namespace MovieListing.UI
{
    [CreateAssetMenu(menuName = "MovieListings/Entry Settings", order = 1, fileName = "EntrySettings")]
    public class EntrySettings : ScriptableObject {
        [SerializeField] private GameObject _entryPrefab = null;
        [SerializeField] private EntryItemSettings _itemSettings = null;
        [SerializeField] private List<string> _headersToUse = null;
        
        public GameObject EntryPrefab => _entryPrefab;
        public EntryItemSettings ItemSettings => _itemSettings;
        public List<string> HeadersToUse => _headersToUse;
    }
}