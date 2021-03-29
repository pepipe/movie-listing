using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Settings
{
    
    
    [CreateAssetMenu(menuName = "MovieListings/General Settings", order = 1, fileName = "GeneralSettings")]
    public class GeneralSettings : ScriptableObject {
        [Tooltip("CSV file to load from Resources folder (without extension)")]
        [SerializeField] private string fileToLoad;
        [SerializeField] private EntrySettings entrySettings;
        [SerializeField] private int entriesPerPage = 50;

        public string FileToLoad => fileToLoad;
        public EntrySettings EntrySettings => entrySettings;
        public int EntriesPerPage => entriesPerPage;
    }
}