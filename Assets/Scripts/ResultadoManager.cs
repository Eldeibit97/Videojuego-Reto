using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultadoManager : MonoBehaviour
{   
    public GameObject PauseMenu;

     public void BackToMenuPressed(){
        SceneManager.LoadScene("SampleScene");
    }

    public void PausePressed(){
        PauseMenu.SetActive(true);
    }

    public void ResumePressed(){
        PauseMenu.SetActive(false);
    }

    public void QuitPressed(){
        SceneManager.LoadScene("SampleScene");
    }     
}
