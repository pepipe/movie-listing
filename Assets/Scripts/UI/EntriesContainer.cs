using UnityEngine;

namespace MovieListing.UI {
    public class EntriesContainer : MonoBehaviour {
        private float _containerWidth;
        
        void Start() {
            var rt = transform.parent.GetComponent<RectTransform>();
            _containerWidth = Screen.width + rt.sizeDelta.x * rt.localScale.x;
        }
        
        public float GetWidth(float widthPercentage) {
            return _containerWidth * widthPercentage / 100f;
        }
    }
}