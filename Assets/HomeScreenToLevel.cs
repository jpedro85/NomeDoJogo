using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenToLevel : MonoBehaviour
{
   public void PlayButton()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
   }
}
