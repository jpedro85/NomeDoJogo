using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveButton : MonoBehaviour
{
public void LeaveGame()
    {
        SceneManager.LoadScene(1);
    }
}

