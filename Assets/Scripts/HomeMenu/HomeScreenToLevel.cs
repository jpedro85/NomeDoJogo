using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenToLevel : MonoBehaviour
{
   public void PlayButton()
   {
      DataPersistence.DataPersistenceManager.instance.saveGame();
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
   }
}
