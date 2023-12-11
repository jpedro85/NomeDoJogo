using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeToPlay : MonoBehaviour
{
    public void GoToPlay()
    {
        DataPersistence.DataPersistenceManager.instance.saveGame();
        SceneManager.LoadScene(3);
    }
}
