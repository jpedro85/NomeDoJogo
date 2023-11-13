using UnityEngine;
using System;
using System.IO;
using DataPersistence.Data;
using UnityEditor.ShaderGraph.Serialization;


namespace DataPersistence
{
    public class FileHandler
    {
        private string dirPath = "";
        private string dataFileName = "";

        public FileHandler(string dirPath, string dataFileName)
        {
            this.dirPath = dirPath;
            this.dataFileName = dataFileName;
        }

        public GameData load()
        {
            string fullPath = Path.Combine(dirPath, dataFileName);
            GameData loadedData = null;
            Debug.Log(fullPath);
            if (File.Exists(fullPath))
            {
                try
                {
                    // load serialized data from file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // deserialize the data from the JSON into a C# object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.Log("Error occured when trying to load data from file: " + fullPath + "\n" + e);
                }
            }

            return loadedData;
        }

        public void save(GameData gameData)
        {
            // use Path.Combine to account for different OS's having  different path separators
            string fullPath = Path.Combine(dirPath, dataFileName);
            try
            {
                // create the directory the file wil be written to if doesnt already exist 
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Serialize the C# game data object to into a JSON
                string dataToStore = JsonUtility.ToJson(gameData, true);
                // write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error occurred when trying to save data to file" + "\n" + e);
            }
        }
    }
}