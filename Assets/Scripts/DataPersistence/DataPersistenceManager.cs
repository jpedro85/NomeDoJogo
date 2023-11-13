using System.Collections.Generic;
using System.Linq;
using DataPersistence.Data;
using UnityEngine;

namespace DataPersistence
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [Header("File Storage Config")]
        [SerializeField] private string fileName;
        
        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileHandler dataHandler;
        public static DataPersistenceManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.Log("Found moore than one Data Persistence Manager in the scene.");
            }

            instance = this;
        }

        public void Start()
        {
            this.dataHandler = new FileHandler(Application.persistentDataPath, fileName);
            this.dataPersistenceObjects = findAllDataPersistenceObjects();
            loadGame();
        }


        public void newGame()
        {
            this.gameData = new GameData();
        }

        public void loadGame()
        {
            // Load any saved data from a file using the data handler
            this.gameData = dataHandler.load();
            
            // if no data can be loaded, initialize to a new game
            if (this.gameData == null)
            {
                Debug.Log("No Data was found. Initializing to the defaults.");
                newGame();
            }
            // push the loaded data to all oth er scripts that need it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.loadData(gameData);
            }

        }

        public void saveGame()
        {
            //  pass the data to other scripts so they can update it
            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.saveData(ref gameData);
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
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>()
                .OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }
    }
}