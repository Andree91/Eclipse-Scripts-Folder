using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class BonfireLitPopUpUI : MonoBehaviour
    {
        CanvasGroup canvas;

        void Awake() 
        {
            canvas = GetComponent<CanvasGroup>();
        }

        public void DisplayBonfireLitPopUp()
        {
            gameObject.SetActive(true);
            StartCoroutine(FadeInPopUp());
        }

        IEnumerator FadeInPopUp()
        {
           // gameObject.SetActive(true);

            for (float fade = 0.05f; fade < 1; fade += 0.05f)
            {
                canvas.alpha = fade;

                if (fade > 0.9f)
                {
                    StartCoroutine(FadeOutPopUp());
                }

                yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator FadeOutPopUp()
        {
            //Wait two seconds before starting to fade out
            yield return new WaitForSeconds(2f);

            for (float fade = 1f; fade > 0; fade -= 0.05f)
            {
                canvas.alpha = fade;

                if (fade <= 0.05f)
                {
                    gameObject.SetActive(false);
                }

                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}
