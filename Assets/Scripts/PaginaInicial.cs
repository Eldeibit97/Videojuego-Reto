using UnityEngine;
using UnityEngine.SceneManagement;

public class PaginaInicial : MonoBehaviour
{
    public static PaginaInicial instance;
    public AudioSource buttonClicked; 

    public void JugarPressed()
    {
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        SceneManager.LoadScene(1); 
    }
}
