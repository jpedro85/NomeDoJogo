using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsOpen : MonoBehaviour
{
    public void SettingsButton()
    {
        SceneManager.LoadScene(2);
    }

    public void SettingsHomeButton()
    {
        SceneManager.LoadScene(5);
    }
    public void SettingsHomeBack()
    {
        SceneManager.LoadScene(0);
    }
}
