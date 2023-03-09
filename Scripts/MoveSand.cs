using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class MoveSand : MonoBehaviour
    {
        Renderer sandRender;
        [SerializeField] float moveSpeedX = 0.1f;
        [SerializeField] float moveSpeedY = 0.1f;
        float offsetX;
        float offsetY;

        void Awake() 
        {
            sandRender = GetComponent<Renderer>();
        }

        void Update()
        {
            offsetX += Time.deltaTime * moveSpeedX;
            if (offsetX >= 1) { offsetX = 0; }

            offsetY += Time.deltaTime * moveSpeedY;
            sandRender.material.mainTextureOffset = new Vector2 (offsetX, offsetY);
        }
    }
}
