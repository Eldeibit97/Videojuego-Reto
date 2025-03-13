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

public class QuizDatabaseManager : MonoBehaviour {
    public string juego_apiUrl = "http://localhost/quiz_game/get_questions.php";
    public List<Question> questions;

    public TextMeshProUGUI questionText;  // text Object in Unity
    
    public TextMeshProUGUI Problemaindex;
    public int NumeroProblema = 10;
    private int ProblemaActual = 0;
    private int ScoreCounter = 0;
    public Button verdaderoButton;
    public Button falsoButton;

    public GameObject GameScene_PreguntasVF;
    public GameObject ResultScreen;
    public GameObject PaginaInicial;
    public GameObject CorrectSign;
    public GameObject IncorrectSign;


    void Start() {
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
        if (questions.Count > index) {
            questionText.text = questions[index].Pregunta; // put the problem
            Problemaindex.text = ("Número total de preguntas: " + NumeroProblema + ", Actual: " + (ProblemaActual+1));
        }
    }

    // Acciones que se realizan cuando se presiona un botón
    void OnAnswerButtonClicked(bool userAnswer) {
        // Check the user's answer and proceed to next
        bool correctAnswer = questions[ProblemaActual].Correcta;

        if (userAnswer == correctAnswer) {
            Debug.Log("Respuesta correcta");
            ScoreCounter++;
            CorrectSign.SetActive(true);
            Invoke("HideCorrectSign", 1f); // Wait for 1 sec

        } else {
            // Debug.Log("Respuesta incorrecta");
            IncorrectSign.SetActive(true);
            Invoke("HideIncorrectSign", 1f); // Wait for 1 sec
            return;
        }

        // next quesiton
        ProblemaActual++;

        
        if (ProblemaActual < questions.Count) {
            DisplayQuestion(ProblemaActual);  // show the next problem
        } 
        else 
        {// when all of problems done
            GameScene_PreguntasVF.SetActive(false);
            SceneManager.LoadScene("Resultado");
        }
    }

    void HideCorrectSign() {
        CorrectSign.SetActive(false);
    }
    void HideIncorrectSign(){
        IncorrectSign.SetActive(false);

    }
}
