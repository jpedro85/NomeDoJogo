using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeToPlay : MonoBehaviour
{
    public void GoToPlay()
    {
        SceneManager.LoadScene(3);
    }
}
