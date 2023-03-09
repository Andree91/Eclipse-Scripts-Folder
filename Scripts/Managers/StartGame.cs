using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class StartGame : MonoBehaviour
    {
        int startingLevel = 1;

        public void StartGameFromIndex()
        {
            SceneManager.LoadScene(startingLevel);
        }
    }
}
