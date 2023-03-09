using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class LevelManager : MonoBehaviour
    {
        public GameObject player;
        int currentSceneIndex;

        public void LoadNextLevel()
        {
            DontDestroyOnLoad(player);
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        
    }
}
