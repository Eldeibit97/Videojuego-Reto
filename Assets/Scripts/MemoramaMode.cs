using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

    // ⬇ NUEVO: reproducir animación de entrada tipo cascada
    for (int i = 0; i < cards.Count; i++)
    {
        StartCoroutine(cards[i].PlayEntryAnimation(i * 0.05f));
    }
}


    public void CheckMatch(Card selectedCard)
{
    if (!canClick || firstCard != null && secondCard != null) return; // ✨ NO DEJAR MÁS DE 2 CARTAS

    if (firstCard == null)
    {
        firstCard = selectedCard;
    }
    else if (secondCard == null)
    {
        secondCard = selectedCard;
        canClick = false; // ✨ BLOQUEAR INMEDIATAMENTE
        StartCoroutine(CheckPair());
    }
}

IEnumerator CheckPair()
{
    canClick = false; // 🔵 Bloquear selección

    // 🔵 Desactivar TODOS los botones mientras comparas
    foreach (var card in cards)
    {
        card.GetComponent<Button>().interactable = false;
    }

    yield return new WaitForSeconds(1f); // Mostrar ambas cartas

    if (firstCard.cardID == secondCard.cardID)
    {
        DontDestroyOnLoad(CardMatched.gameObject);
        CardMatched.Play();

        firstCard.HideCard();
        secondCard.HideCard();
        remainingCards -= 2;
        PlayerPrefs.SetInt("remainingCards", remainingCards);
    }
    else
    {
        StartCoroutine(firstCard.ShakeCard());
        StartCoroutine(secondCard.ShakeCard());

        yield return new WaitForSeconds(0.35f); // Esperar después del shake

        firstCard.ResetCard();
        secondCard.ResetCard();
    }

    // 🔵 Reset para el siguiente turno
    firstCard = null;
    secondCard = null;

    // 🔵 Volver a habilitar los botones solo cuando todo terminó
    foreach (var card in cards)
    {
        if (card.isActiveAndEnabled) // Solo si sigue activa
            card.GetComponent<Button>().interactable = true;
    }

    canClick = true; // 🔵 Permitir clicks otra vez

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
        SceneManager.LoadScene("Resultado");
    }
}