using UnityEngine;

namespace view
{
    public class Element : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        private Color destinationColor;

        public void SetColor(Color color)
        {
            if (null == spriteRenderer)
            {
                return;
            }
            spriteRenderer.color = color;
        }
    }
}