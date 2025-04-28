using UnityEngine;
using UnityEngine.SceneManagement;

public class SeleccionarJuego : MonoBehaviour
{
    public GameObject UIBasicoPrefab;
    public AudioSource buttonClicked; 

    void Start(){
        Instantiate(UIBasicoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void PreguntasVFPressed()
    {
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        SceneManager.LoadScene(2);
    }

    public void JuegoDescubrePressed()
    {
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        SceneManager.LoadScene(3);
    }

    public void JuegoMemorama()
    {
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        SceneManager.LoadScene(3);
    }

    public void QuestionarioGeneralPressed()
    {
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        SceneManager.LoadScene(4);
    }
}
