using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPlayOpen : MonoBehaviour
{
    public void SettingsButton()
    {
        DataPersistence.DataPersistenceManager.instance.saveGame();
        SceneManager.LoadScene(4);
    }
}
