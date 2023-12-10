using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataPersistence.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

// if you're going to change scene make sure to save the game data before changing the scene
// ! on new scene on button that changes save the game before the scene change

namespace DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("Debugging")] [SerializeField] private bool initializeDataIfNull = false;

        [Header("File Storage Config")] [SerializeField]
        private string fileName;

        [SerializeField] private bool useEncryption;

        [Header("Auto Save Configuration")] [SerializeField]
        private float autoSaveTimeSeconds = 60f;

        private Coroutine autoSaveCoroutine;
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileHandler dataHandler;


        public static DataPersistenceManager instance { get; private set; }

        private void Start()
        {
            if (!DataPersistenceManager.instance.hasGameData())
            {
                Debug.LogWarning("Doesn't have Game Data");
                newGame();
            }
        }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one");
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);
            this.dataHandler = new FileHandler(Application.persistentDataPath, fileName, useEncryption);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += onSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded += onSceneLoaded;
        }

        public void onSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.dataPersistenceObjects = findAllDataPersistenceObjects();
            loadGame();
            
            // starting the auto save coroutine
            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
            }

            autoSaveCoroutine = StartCoroutine(autoSave());
        }

        private void newGame()
        {
            this.gameData = new GameData();
        }

        private void loadGame()
        {
            // Load any saved data from a file using the data handler
            this.gameData = dataHandler.load();

            // start a game if game data is null and we configured to initialize data for debugging purposes
            if (this.gameData == null &&  initializeDataIfNull)
            {
                newGame();
            }

            // if no data can be loaded, initialize to a new game
            if (this.gameData == null)
            {
                Debug.Log("No Data was found. A New game needs to be started before the data can be loaded.");
                return;
            }

            // push the loaded data to all oth er scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.loadData(gameData);
            }
        }

        private void saveGame()
        {
            // if  we don't have any data to save, log a warning
            if (this.gameData == null)
            {
                Debug.LogWarning("No Data was found. A New game needs to be started before the data can be loaded.");
                return;
            }

            //  pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.saveData(gameData);
            }

            // save that to a file using the data handler
            this.dataHandler.save(this.gameData);
        }

        public void OnApplicationQuit()
        {
            saveGame();
        }

        private List<IDataPersistence> findAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
                .OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        private bool hasGameData()
        {
            return this.gameData != null;
        }

        private IEnumerator autoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoSaveTimeSeconds);
                saveGame();
                Debug.Log("Game Saved");
            }
        }
    }
}