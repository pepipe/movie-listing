using TMPro;
using UnityEngine;

namespace UI
{
  [CreateAssetMenu(menuName = "MovieListings/Entry Item Settings", order = 2, fileName = "EntryItemSettings")]
  public class EntryItemSettings : ScriptableObject {
    [SerializeField] private TMP_FontAsset font = null;
    [SerializeField] private float  fontSize;
    [SerializeField] private TextAlignmentOptions alignment;
    [SerializeField] private Color color;

    public TMP_FontAsset Font => font;
    public float FontSize => fontSize;
    public TextAlignmentOptions Alignment => alignment;
    public Color Color => color;
  }
}
