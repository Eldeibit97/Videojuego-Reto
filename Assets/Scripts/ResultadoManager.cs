using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultadoManager : MonoBehaviour
{   
    public GameObject UIBasicoPrefab;
    public TextMeshProUGUI GameClearText;
    public TextMeshProUGUI ResultText;

    public AudioSource buttonClicked; 

    void Start(){
        Instantiate(UIBasicoPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        int correctAnswers = PlayerPrefs.GetInt("correctAnswers", 0);
        int totalQuestions = PlayerPrefs.GetInt("totalQuestions", 0);
        string gameMode = PlayerPrefs.GetString("gameMode", "GameMode");
        string ClearChecker = PlayerPrefs.GetString("ClearChecker", "Game Clear!");

        int totalCards = PlayerPrefs.GetInt("totalCards", 0);
        int remainingCards = PlayerPrefs.GetInt("remainingCards", 0);
        int pairFounded = (totalCards - remainingCards)/2;

        GameClearText.text = ClearChecker;

        if (gameMode == "JuegoMemorama")
        {
            if (remainingCards == 0)
            {
                ResultText.text =   $"GameMode: " + gameMode+ "\n" +
                                    "Encontraste todos los pares de cartas" + "\n" +
                                    "Buen Trabajo!";
            } 
            else
            {
                ResultText.text =   $"GameMode: " + gameMode+ "\n" +
                                    "Encontraste " + pairFounded + " pares de cartas" + "\n" +
                                    "Buen Trabajo!";
            }

        } 
        else
        {
            ResultText.text =   $"GameMode: " + gameMode+ "\n" +
                                correctAnswers + " / " + totalQuestions + "\n" +
                                "Buen Trabajo!";
        }
        
    }   

     public void BackToMenuPressed(){
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        SceneManager.LoadScene(0);
    }
}
