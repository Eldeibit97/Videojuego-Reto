using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public AudioSource buttonClicked; 


    public void PausePressed(){
        PauseMenu.SetActive(true);
    }

    public void ResumePressed(){
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        PauseMenu.SetActive(false);
    }

    public void HomePressed(){
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        SceneManager.LoadScene(0);
    }
}
