using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MovieListing.UI
{
  [CreateAssetMenu(menuName = "MovieListings/Entry Item Settings", order = 2, fileName = "EntryItemSettings")]
  public class EntryItemSettings : ScriptableObject {
    [SerializeField] private TMP_FontAsset _font = null;
    [SerializeField] private float  _fontSize;
    [SerializeField] private TextAlignmentOptions _alignment;
    [SerializeField] private Color _color;

    public TMP_FontAsset Font => _font;
    public float FontSize => _fontSize;
    public TextAlignmentOptions Alignment => _alignment;
    public Color Color => _color;
  }
}
