using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Question {
    public int id;
    public string problema_text;
    public bool respuesta_correcta;
}

[System.Serializable]
public class QuestionList {
    public List<Question> questions;
}

public class QuizDatabaseManager : MonoBehaviour {
    public string juego_apiUrl = "http://localhost/quiz_game/get_questions.php?table=problemas_VF";
    public List<Question> questions;

    public TextMeshProUGUI questionText;  // text Object in Unity
    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI Problemaindex;
    public int NumeroPloblema = 10;
    private int ProblemaActual = 0;
    private int ScoreCounter = 0;
    public Button verdaderoButton;  // Verdaderoボタン
    public Button falsoButton;  // Falsoボタン

    public GameObject GameScene_PreguntasVF;
    public GameObject ResultScreen;
    public GameObject PaginaInicial;



    void Start() {
        Debug.Log(juego_apiUrl);
        StartCoroutine(GetQuestionsFromDatabase(juego_apiUrl));

        // ボタンにリスナーを追加（ボタンが押されたときの処理）
        verdaderoButton.onClick.AddListener(() => OnAnswerButtonClicked(true));
        falsoButton.onClick.AddListener(() => OnAnswerButtonClicked(false));
    }

    IEnumerator GetQuestionsFromDatabase(string apiURL) {
        Debug.Log("Sending request to: " + apiURL); // 送信するURLを確認

        UnityWebRequest request = UnityWebRequest.Get(apiURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) {
            string json = "{\"questions\":" + request.downloadHandler.text + "}";
            Debug.Log("Received JSON: " + json); // 受信データの確認

            QuestionList questionList = JsonUtility.FromJson<QuestionList>(json);
            if (questionList != null && questionList.questions != null) {
                questions = questionList.questions;
                Debug.Log("Datos obtenidos exitosamente: " + questionList.questions.Count + " preguntas");
                
                DisplayQuestion(ProblemaActual);
            } else {
                Debug.LogError("Error: JSON no se pudo convertir correctamente.");
            }
        } else {
            Debug.LogError("Error al obtener datos: " + request.error);
        }
    }


    // QuestionをTextオブジェクトに反映させる
    void DisplayQuestion(int index) {
        if (questions.Count > index) {
            questionText.text = questions[index].problema_text; // 問題文をセット
            Problemaindex.text = (ProblemaActual + " / " + NumeroPloblema);
        }
    }

    // ボタンがクリックされたときの処理
    void OnAnswerButtonClicked(bool userAnswer) {
        // ユーザーの回答を確認し、次の問題に進む
        bool correctAnswer = questions[ProblemaActual].respuesta_correcta;

        if (userAnswer == correctAnswer) {
            Debug.Log("Respuesta correcta");
            ScoreCounter++;
        } else {
            Debug.Log("Respuesta incorrecta");
        }

        // 次の問題に進む
        ProblemaActual++;

        // すべての問題が終わった場合
        if (ProblemaActual < questions.Count) {
            DisplayQuestion(ProblemaActual);  // 次の問題を表示
        } else {
            // Debug.Log("¡Has terminado todas las preguntas!");
            GameScene_PreguntasVF.SetActive(false);
            ResultScreen.SetActive(true);
            ResultText.text = (ScoreCounter + " / " +NumeroPloblema);
        }
    }
}
