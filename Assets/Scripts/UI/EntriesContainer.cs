using UnityEngine;

namespace UI {
    public class EntriesContainer : MonoBehaviour {
        private float _containerWidth;
        
        private void Awake() {
            var rt = transform.parent.GetComponent<RectTransform>();
            _containerWidth = Screen.width + rt.sizeDelta.x * rt.localScale.x;
        }
        
        public float GetWidth(float widthPercentage) {
            return _containerWidth * widthPercentage / 100f;
        }
    }
}