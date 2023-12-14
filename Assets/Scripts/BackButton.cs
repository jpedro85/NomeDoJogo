using UnityEngine;
using UnityEngine.SceneManagement;
public class BackButton : MonoBehaviour
{
public void Backbtn()
   {
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
   }
}


