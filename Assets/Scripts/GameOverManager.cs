using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void ToResultPressed()
    {
        SceneManager.LoadScene(5);
    }
}
