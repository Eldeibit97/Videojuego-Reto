using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Question {
    public int ID_pregunta;
    public string Pregunta;
    public bool Correcta;
}

[System.Serializable]
public class QuestionList {
    public List<Question> questions;
}

public class PreguntasVFManager : MonoBehaviour {
    private string juego_apiUrl = "http://hwkwokogcc40ww44sgw8ogsk.20.246.88.89.sslip.io/get_questions";
    public List<Question> questions;

    public TextMeshProUGUI questionText;  // text Object in Unity
    
    public TextMeshProUGUI Problemaindex;
    public int NumeroProblema = 10;
    private int ProblemaActual = 0;
    private int ScoreCounter = 0;
    public Button verdaderoButton;
    public Button falsoButton;

    public GameObject CorrectSignPrefab;
    public GameObject IncorrectSignPrefab;
    private GameObject CorrectSignInstance;
    private GameObject IncorrectSignInstance;

    public GameObject UIBasicoPrefab;

    public GameObject InstructionScreen;
    public GameObject GameScreen;
    public GameObject Timer;

    public AudioSource buttonClicked; 
    public AudioSource AudioCorrect;
    public AudioSource AudioIncorrect;

    void Awake() {
        GameScreen.SetActive(false);
        Timer.SetActive(false);
        InstructionScreen.SetActive(true);
    }
    
    void Start() {
        Instantiate(UIBasicoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
        PlayerPrefs.SetString("gameMode", "PreguntasVF");
        PlayerPrefs.SetInt("totalQuestions", NumeroProblema);
        StartCoroutine(GetQuestionsFromDatabase(juego_apiUrl));

        // Acciones que se realizan cuando se presiona un botón
        verdaderoButton.onClick.AddListener(() => OnAnswerButtonClicked(true));
        falsoButton.onClick.AddListener(() => OnAnswerButtonClicked(false));
    }

    IEnumerator GetQuestionsFromDatabase(string apiURL) {

        UnityWebRequest request = UnityWebRequest.Get(apiURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            string json = request.downloadHandler.text;
            // Debug.Log("Received JSON: " + json);

            QuestionList questionList = JsonUtility.FromJson<QuestionList>(json);
            if (questionList != null && questionList.questions != null) {
                questions = questionList.questions;
                // Debug.Log("Datos obtenidos exitosamente: " + questionList.questions.Count + " preguntas");
                
                DisplayQuestion(ProblemaActual);
            } else {
                Debug.LogError("Error: JSON no se pudo convertir correctamente.");
            }
        } else {
            Debug.LogError("Error al obtener datos: " + request.error);
        }
    }


    // Reflect the problem to the object in unity
    void DisplayQuestion(int index) {
        if (NumeroProblema > index) {
            questionText.text = questions[index].Pregunta; // put the problem
            Problemaindex.text = ("Número total de preguntas: " + NumeroProblema + ", Actual: " + (ProblemaActual+1));
            PlayerPrefs.SetInt("correctAnswers", ProblemaActual+1);
        }
    }

    // Acciones que se realizan cuando se presiona un botón
    void OnAnswerButtonClicked(bool userAnswer) {
        // Check the user's answer and proceed to next
        bool correctAnswer = questions[ProblemaActual].Correcta;

        if (userAnswer == correctAnswer) {
            DontDestroyOnLoad(AudioCorrect.gameObject);
            AudioCorrect.Play();
            // Debug.Log("Respuesta correcta");
            ScoreCounter++;
            CorrectSignInstance = Instantiate(CorrectSignPrefab);
            Destroy(CorrectSignInstance, 1f);

        } else {
            DontDestroyOnLoad(AudioIncorrect.gameObject);
            AudioIncorrect.Play();
            // Debug.Log("Respuesta incorrecta");
            IncorrectSignInstance = Instantiate(IncorrectSignPrefab);
            Destroy(IncorrectSignInstance, 1f);
            return;
        }

        // next quesiton
        ProblemaActual++;

        
        if (ProblemaActual < NumeroProblema) {
            DisplayQuestion(ProblemaActual);  // show the next problem
        } 
        else 
        {// when all of problems done
            PlayerPrefs.SetString("ClearChecker", "Game Clear!");
            SceneManager.LoadScene(5);
        }
    }

    public void StartPressed()
    {
        DontDestroyOnLoad(buttonClicked.gameObject);
        buttonClicked.Play();
        InstructionScreen.SetActive(false);
        GameScreen.SetActive(true);
        Timer.SetActive(true);
    }
}
