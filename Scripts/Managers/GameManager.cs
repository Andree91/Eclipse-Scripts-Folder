using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public float levelLoadDelay;
        public PlayerManager player;
        [SerializeField] ChessUIManager chessUIManager;
        [SerializeField] bool isPlayingChess = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            //DontDestroyOnLoad(gameObject); // HAVE TO DOUBLE CHCK TIS, ONLY TEMP FIX
            // Load all character slots/profiles
        }

        private void OnEnable()
        {
            Debug.Log("Inside OnEnable");
            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }
        }

        private void Update()
        {
            if (!isPlayingChess) { return; }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                chessUIManager.TogglePauseScreen();
            }
        }

        public void ReloadLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
            // TODO LOADING SCREEN SCENE, WHICH IS FIRST LOADED (LoadSceneMode.Additive) SO GAMEPLAY TIME DON'T STOP
            // TODO LOADING THE SAME SCENE AGAIN WHILE LOADING SCREEN IS ON DISPLAY, SOMEKIND TIMER WHICH SHOWS HOW FAR LOADING IS
        }

        public void SaveGameFromPlayerManager()
        {
            player.saveGameManager.SaveGame();
        }

        public void LoadGameFromPlayerManager()
        {
            if (player.saveGameManager == null)
            {
                player.saveGameManager = FindObjectOfType<WorldSaveGameManager>(); // Check this later
            }
            // if (player.uIManager.deathPopUp != null)
            // {
            //     player.uIManager.deathPopUp.gameObject.SetActive(false);
            // }
            //player.uIManager.DeactiveDeathPopUp();
            player.saveGameManager.LoadGame();
            //StartCoroutine(YieldLoadingGame());
        }

        public void LoadNewScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        IEnumerator YieldLoadingGame()
        {
            yield return new WaitForSeconds(1f);
            player.saveGameManager.LoadGame();
        }

        public void QuitGame()
        {
            Debug.Log("Hit Exit - Closing application");
            StartCoroutine(YieldQuitGame());
        }

        public void QuitGameAfterSaving()
        {
            StartCoroutine(YieldQuitGameAfterSaving());
        }

        IEnumerator YieldQuitGame()
        {
            yield return new WaitForSeconds(0.2f);
            Application.Quit();
        }

        IEnumerator YieldQuitGameAfterSaving()
        {
            yield return new WaitForSeconds(5f);
            Application.Quit();
        }
    }
}