using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsPlayOpen : MonoBehaviour
{
    public void SettingsBtn()
    {
        DataPersistence.DataPersistenceManager.instance.saveGame();
        SceneManager.LoadScene(4);
    }
}
