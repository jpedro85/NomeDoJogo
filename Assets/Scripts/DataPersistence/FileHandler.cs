using UnityEngine;
using System;
using System.IO;
using DataPersistence.Data;

namespace DataPersistence
{
    public class FileHandler
    {
        private string dirPath = "";
        private string dataFileName = "";
        private bool useEncryption = false;
        private readonly string encryptionCodeWord = "naoseibyRicardo";
        private readonly string backupExtension = ".bak";

        public FileHandler(string dirPath, string dataFileName, bool useEncryption)
        {
            this.dirPath = dirPath;
            this.dataFileName = dataFileName;
            this.useEncryption = useEncryption;
        }

        public GameData load(bool allowRestoreFronBackup = true)
        {
            string fullPath = Path.Combine(dirPath, dataFileName);
            GameData loadedData = null;
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

                    // optionally encrypt the data
                    if (useEncryption)
                    {
                        dataToLoad = encryptDecrypt(dataToLoad);
                    }

                    // deserialize the data from the JSON into a C# object
                    loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    // Since we're calling load(..) recursively, we need to account for the case the rollback succeeds, 
                    // but data is still failing to load for some reason, which without this check may cause a infinite loop
                    if (allowRestoreFronBackup)
                    {
                        Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                        bool rollBackupSuccess = attemptRollBack(fullPath);
                        if (rollBackupSuccess)
                        {
                            // try to load recursively
                            loadedData = load(false);
                        }
                    }
                    else
                    {
                        Debug.LogError(" Error occurred when trying to load file at path: " + fullPath +
                                       "and backup did not work.\n" + e);
                    }
                }
            }

            return loadedData;
        }

        public void save(GameData gameData)
        {
            // use Path.Combine to account for different OS's having  different path separators
            string fullPath = Path.Combine(dirPath, dataFileName);
            string backupFilePath = fullPath + backupExtension;
            try
            {
                // create the directory the file wil be written to if doesnt already exist 
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                // Serialize the C# game data object to into a JSON
                string dataToStore = JsonUtility.ToJson(gameData, true);

                if (useEncryption)
                {
                    dataToStore = encryptDecrypt(dataToStore);
                }

                // write the serialized data to the file
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }

                // Verify if the file just saved isn't corrupted
                GameData verifiedGameData = load();
                // if the data can be verified, back it up
                if (verifiedGameData != null)
                {
                    File.Copy(fullPath, backupFilePath, true);
                }
                else
                {
                    throw new Exception("Save file could not be verified and the backup could not be created.");
                }
            }
            catch (Exception e)
            {
                Debug.Log("Error occurred when trying to save data to file" + "\n" + e);
            }
        }

        public string encryptDecrypt(string data)
        {
            string modifiedData = "";
            for (int i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
            }

            return modifiedData;
        }

        private bool attemptRollBack(string fullPath)
        {
            bool success = false;
            string backupFilePath = fullPath + backupExtension;
            try
            {
                // if the backup file exists, attempt to roll back to it overwriting the original file
                if (File.Exists(backupFilePath))
                {
                    File.Copy(backupFilePath, fullPath, true);
                    success = true;
                    Debug.LogWarning("Had to roll back to file at: " + backupFilePath);
                }
                else
                {
                    throw new Exception("Tried to rollback, but no backup file exists to roll back to.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to roll back to backup file at " + backupFilePath + "\n" +
                               e);
            }

            return success;
        }
    }
}