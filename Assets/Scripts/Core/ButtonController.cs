using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonController : MonoBehaviour
{
   public void Restart()
   {
       MyEnemyGeneratorController.MaxCount = 3;
       PlayerManager.Score = 0;
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   public void Exit() => Application.Quit();
}
