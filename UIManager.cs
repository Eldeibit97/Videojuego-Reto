using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // public GameObject[] AllCanvas;
    public GameObject PaginaInicial;
    public GameObject SeleccionarModulos;
    public GameObject SeleccionarJuegos;
    public GameObject GameScene_PreguntasVF;
    public GameObject GameScene_DescubreProblema;
    public GameObject GameScene_ConectaPieza;
    public GameObject GameScene_QuestionarioGeneral;
    public GameObject ResultScreen;
    public GameObject PauseMenu;
    

    public string Modulo;

    public void JugarPressed(){
        PaginaInicial.SetActive(false);
        SeleccionarModulos.SetActive(true);
    }

    public void PausePressed(){
        PauseMenu.SetActive(true);
    }

    public void ResumePressed(){
        PauseMenu.SetActive(false);
    }

    public void QuitPressed(){
        // AllCanvas.SetActive(false);
        PauseMenu.SetActive(false);
        PaginaInicial.SetActive(true);
    }     

    public void RefrigeradorPressed(){
        Modulo = "Refrigerador";

        SeleccionarModulos.SetActive(false);
        SeleccionarJuegos.SetActive(true);
    }

    public void MicroondasPressed(){
        Modulo = "Microondas";

        SeleccionarModulos.SetActive(false);
        SeleccionarJuegos.SetActive(true);
    }

    public void HornoPressed(){
        Modulo = "Horno";

        SeleccionarModulos.SetActive(false);
        SeleccionarJuegos.SetActive(true);
    }

    public void JuegoVFPressed(){
        SeleccionarJuegos.SetActive(false);
        GameScene_PreguntasVF.SetActive(true);
    }

    public void JuegoDescubrePressed(){
        SeleccionarJuegos.SetActive(false);
        GameScene_DescubreProblema.SetActive(true);
    }

    public void JuegoConectaPressed(){
        SeleccionarJuegos.SetActive(false);
        GameScene_ConectaPieza.SetActive(true);
    }

    public void JuegoQuestionarioPressed(){
        SeleccionarJuegos.SetActive(false);
        GameScene_QuestionarioGeneral.SetActive(true);
    }

    public void BackToMenuPressed(){
        ResultScreen.SetActive(false);
        PaginaInicial.SetActive(true);
    }
}
