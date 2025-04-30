using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Option
{
    public string respuesta;
    public bool correcta;
}

[System.Serializable]
public class Problema
{
    public int ID_Pregunta;
    public string Pregunta;
    public List<Option> options;
}

[System.Serializable]
public class ProblemaList
{
    public List<Problema> questions;
}

public class QuestionarioGeneralManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;

    private List<Problema> allQuestions;
    private int currentQuestionIndex = 0;

    public TextMeshProUGUI Problemaindex;
    public int NumeroProblema = 10;

    public GameObject UIBasicoPrefab;

    public GameObject CorrectSignPrefab;
    public GameObject IncorrectSignPrefab;
    private GameObject CorrectSignInstance;
    private GameObject IncorrectSignInstance;

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

    void Start()
    {
        PlayerPrefs.SetString("gameMode", "QuestionarioGeneral");
        PlayerPrefs.SetInt("totalQuestions", NumeroProblema);

        Instantiate(UIBasicoPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        StartCoroutine(LoadQuestions());
    }

    IEnumerator LoadQuestions()
    {
        string url = "http://hwkwokogcc40ww44sgw8ogsk.20.246.88.89.sslip.io/get_choicequestions";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al cargar preguntas: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                allQuestions = JsonUtility.FromJson<ProblemaList>(json).questions;
                DisplayQuestion();
            }
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex >= NumeroProblema)
        {
            PlayerPrefs.SetString("ClearChecker", "Game Clear!");
            SceneManager.LoadScene(5);
            return;
        }

        Problema q = allQuestions[currentQuestionIndex];
        questionText.text = q.Pregunta;
        Problemaindex.text = ("Número total de preguntas: " + NumeroProblema + ", Actual: " + (currentQuestionIndex+1));

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI btnText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            btnText.text = q.options[i].respuesta;

            int index = i; // Para evitar problema de cierre en lambda
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => CheckAnswer(q.options[index].correcta));
        }
    }

    void CheckAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            DontDestroyOnLoad(AudioCorrect.gameObject);
            AudioCorrect.Play();
            // Debug.Log("¡Respuesta correcta!");
            CorrectSignInstance = Instantiate(CorrectSignPrefab);
            Destroy(CorrectSignInstance, 1f);
            currentQuestionIndex++;
            PlayerPrefs.SetInt("correctAnswers", currentQuestionIndex);
            DisplayQuestion();
        }
        else
        {
            DontDestroyOnLoad(AudioIncorrect.gameObject);
            AudioIncorrect.Play();
            // Debug.Log("Respuesta incorrecta. Intenta otra vez.");
            IncorrectSignInstance = Instantiate(IncorrectSignPrefab);
            Destroy(IncorrectSignInstance, 1f);
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
