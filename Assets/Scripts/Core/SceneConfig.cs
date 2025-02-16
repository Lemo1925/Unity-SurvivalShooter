using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneConfig : MonoBehaviour
{
    public static void LoadGameScene() => SceneManager.LoadScene("GameScene");

    public static void LoadMenuScene() => SceneManager.LoadScene("MenuScene");
}
