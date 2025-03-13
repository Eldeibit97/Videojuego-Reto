using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoramaMode : MonoBehaviour
{
    public List<Card> cards; // List of all cards in the game
    private Card firstCard, secondCard;

    void Start()
    {
        // Obtain every card existing in the scene
        cards = new List<Card>(FindObjectsOfType<Card>());
    }
    
    public void CheckMatch(Card selectedCard)
    {
        if (firstCard == null)
        {
            firstCard = selectedCard;
        }
        else
        {
            secondCard = selectedCard;
            StartCoroutine(CheckPair());
        }
    }

    IEnumerator CheckPair()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 sec to show both cards

        if (firstCard.cardID == secondCard.cardID)
        {
            // If they match, disable them
            firstCard.gameObject.SetActive(false);
            secondCard.gameObject.SetActive(false);
        }
        else
        {
            // If they don’t match, flip them back
            firstCard.ResetCard();
            secondCard.ResetCard();
        }

        // Reset for the next selection
        firstCard = null;
        secondCard = null;

        CheckGameClear();
    }
    
    void CheckGameClear()
    {
        // if every card is not active, gameclear
        foreach (Card card in cards)
        {
            if (card.gameObject.activeSelf)
            {
                return; // if the cards is stil remain, don't do anything
            }
        }

        GameClear();
    }
    
    void GameClear(){
        SceneManager.LoadScene("Resultado");
    }
}

