using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Settings
{
    
    
    [CreateAssetMenu(menuName = "MovieListings/Entry Settings", order = 1, fileName = "EntrySettings")]
    public class EntrySettings : ScriptableObject {
        [SerializeField] private GameObject entryPrefab = null;
        [Tooltip("Default prefab for entries. This will be override for an entry if that entry sets it's own prefab")]
        [SerializeField] private GameObject defaultEntryItemPrefab = null;
        [Tooltip("List of each column settings")]
        [SerializeField] private List<EntryItemSettings> itemsSettings = null;
        
        
        public GameObject EntryPrefab => entryPrefab;
        public GameObject DefaultEntryItemPrefab => defaultEntryItemPrefab;
        public List<EntryItemSettings> ItemsSettings => itemsSettings;
        
    }
}