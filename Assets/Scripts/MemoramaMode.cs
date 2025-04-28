using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoramaMode : MonoBehaviour
{
    public List<Card> cards; // List of all cards in the game
    private Card firstCard, secondCard;

    public int totalCards;
    private int remainingCards;

    public GameObject UIBasicoPrefab;

    public GameObject InstructionScreen;
    public GameObject GameScreen;
    public GameObject Timer;

    public AudioSource buttonClicked; 
    public AudioSource CardMatched;

    private bool canClick = true; // Evita múltiples selecciones simultáneas

    void Awake()
    {
        GameScreen.SetActive(false);
        Timer.SetActive(false);
        InstructionScreen.SetActive(true);
    }

    public void StartPressed()
{
    DontDestroyOnLoad(buttonClicked.gameObject);
    buttonClicked.Play();
    InstructionScreen.SetActive(false);
    GameScreen.SetActive(true);
    Timer.SetActive(true);

    cards = new List<Card>(FindObjectsOfType<Card>());
    Instantiate(UIBasicoPrefab, Vector3.zero, Quaternion.identity);

    totalCards = cards.Count;
    remainingCards = totalCards;

    PlayerPrefs.SetString("gameMode", "JuegoMemorama");
    PlayerPrefs.SetInt("totalCards", totalCards);
    PlayerPrefs.SetInt("remainingCards", remainingCards);

    // ⬇️ NUEVO: reproducir animación de entrada tipo cascada
    for (int i = 0; i < cards.Count; i++)
    {
        StartCoroutine(cards[i].PlayEntryAnimation(i * 0.05f));
    }
}


    public void CheckMatch(Card selectedCard)
    {
        if (!canClick) return;

        if (firstCard == null)
        {
            firstCard = selectedCard;
        }
        else if (secondCard == null)
        {
            secondCard = selectedCard;
            StartCoroutine(CheckPair());
        }
    }

    IEnumerator CheckPair()
    {
        canClick = false; // bloquear clics mientras se compara

        yield return new WaitForSeconds(1f); // Mostrar ambas cartas

        if (firstCard.cardID == secondCard.cardID)
        {
            DontDestroyOnLoad(CardMatched.gameObject);
            CardMatched.Play();

            // If they match, disable them
            firstCard.HideCard();
            secondCard.HideCard();
            remainingCards -= 2;
            PlayerPrefs.SetInt("remainingCards", remainingCards);
        }
        else
        {
            // Shake antes de hacer reset
            StartCoroutine(firstCard.ShakeCard());
            StartCoroutine(secondCard.ShakeCard());

            yield return new WaitForSeconds(0.35f); // esperar a que termine el shake

            firstCard.ResetCard();
            secondCard.ResetCard();
        }

        // Reset para el siguiente turno
        firstCard = null;
        secondCard = null;

        canClick = true; // permitir clics otra vez

        CheckGameClear();
    }

    void CheckGameClear()
    {
        foreach (Card card in cards)
        {
            CanvasGroup canvasGroup = card.GetComponent<CanvasGroup>();
            if (canvasGroup == null || canvasGroup.alpha > 0)
            {
                return; // aún hay cartas visibles
            }
        }

        GameClear();
    }

    void GameClear()
    {
        PlayerPrefs.SetString("ClearChecker", "Game Clear!");
        SceneManager.LoadScene(5);
    }
}