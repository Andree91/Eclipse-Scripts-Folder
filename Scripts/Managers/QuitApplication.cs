using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class QuitApplication : MonoBehaviour
    {
        public void QuitGame()
        {
            Debug.Log("Hit Exit - Closing application");
            Application.Quit();
        }
    }
}