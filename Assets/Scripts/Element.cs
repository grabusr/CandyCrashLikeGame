using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace view
{
    public class Element : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Color destinationColor;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void SetColor(Color color)
        {
            spriteRenderer.color = color;
        }
    }
}