using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        public PlayerManager player;

        [Header("Save Data Writer")]
        SaveGameDataWriter saveGameDataWriter;

        [Header("Current Character Data")]
        // Character Slot #
        public CharacterSaveData currentCharacterSaveData;
        [SerializeField] string fileName;
        [SerializeField] bool isMasterManager;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        void Awake()
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

        void Start()
        {
            player = FindObjectOfType<PlayerManager>();

            if (isMasterManager)
            {
                DontDestroyOnLoad(gameObject);
            }
            // Load all character slots/profiles
        }

        void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                // SAVE THE GAME
                SaveGame();
                Debug.Log("saveGame bool is " + saveGame);
            }

            else if (loadGame)
            {
                loadGame = false;
                // LOAD THE SAVE DATA
                LoadGame();
            }
        }

        // NEW GAME

        // SAVE GAME
        public void SaveGame()
        {
            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            Debug.Log("Save Data Directory Path is " + Application.persistentDataPath);
            saveGameDataWriter.dataSaveFileName = fileName;

            // Pass along our characters data to our current save fle
            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }
            player.SaveCharacterDataToCurrentSaveData(ref currentCharacterSaveData);

            // Write the current character data to a json file and save it on this device
            saveGameDataWriter.WriteCharacterDataToSaveFile(currentCharacterSaveData);

            Debug.Log("SAVING GAME...");
            Debug.Log("FILE SAVED AS: " + fileName);
        }

        public void ChangeSaveGameBool()
        {
            saveGame = true;
        }

        public void ChangeLoadGameBool()
        {
            loadGame = true;
        }

        // LOAD GAME
        public void LoadGame()
        {
            // DECIDE LOAD FILE BASED ON CHARACTER SAVE SLOT

            saveGameDataWriter = new SaveGameDataWriter();
            saveGameDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveGameDataWriter.dataSaveFileName = fileName;
            currentCharacterSaveData = saveGameDataWriter.LoadCharacterDataFromJson();

            StartCoroutine(LoadWorldSceneAsynchronously());
        }

        IEnumerator LoadWorldSceneAsynchronously()
        {
            if (player == null)
            {
                player = FindObjectOfType<PlayerManager>();
            }

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);

            while (!loadOperation.isDone)
            {
                float loadingProgress = Mathf.Clamp01(loadOperation.progress / 0.9f);
                // Enable Loading Screen here and pass the loading progress to a slider / loading effect
                if (player != null)
                {
                    player.LoadLoadingScreen(loadingProgress);
                }
                yield return null;
            }

            player.LoadCharacterDataFromCurrentCharacterSaveData(ref currentCharacterSaveData);
            player.CloseLoadingScreen();
        }
    }
}
