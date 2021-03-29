using System;
using UnityEngine;

namespace Settings
{
  [Serializable]
  public struct HeaderSetting {
    [Tooltip("CSV header key")]
    public string headerName;
    [Tooltip("Width of the header in percentage.")]
    public float width;
  }

  [Serializable]
  public struct SplitSetting {
    public char splitChar;
    public int wordsToShow;
  }
  
  [CreateAssetMenu(menuName = "MovieListings/Entry Item Settings", order = 2, fileName = "EntryItemSettings")]
  public class EntryItemSettings : ScriptableObject {
    [SerializeField] private HeaderSetting header;
    [SerializeField] private GameObject customEntryItemPrefab;
    [SerializeField] private bool splitEntryValue = false; 
    [SerializeField] private SplitSetting splitSetting;

    public HeaderSetting Header => header;
    public GameObject CustomEntryItemPrefab => customEntryItemPrefab;
    public bool SplitEntryValue => splitEntryValue;
    public SplitSetting SplitSetting => splitSetting;
  }
}
