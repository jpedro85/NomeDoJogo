using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreeToLevel : MonoBehaviour
{
   public void PlayButton()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
   }
}
