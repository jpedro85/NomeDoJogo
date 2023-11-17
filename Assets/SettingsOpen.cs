using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsOpen : MonoBehaviour
{
    public void SettingsButton()
    {
        SceneManager.LoadScene(2);
    }
}
