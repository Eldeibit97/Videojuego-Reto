using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardID;  // Unique ID for matching
    public Sprite frontImage;  // Card's front image
    public Sprite backImage;   // Card's back image
    private bool isFlipped = false;  // Check if flipped
    private Button button;
    private Image image;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(FlipCard);  // Add click event
        ResetCard();
    }

    public void FlipCard()
    {
        if (isFlipped) return; // Prevent flipping again

        isFlipped = true;
        image.sprite = frontImage; // Show front image

        // Call a function from GameManager to check for a match
        FindObjectOfType<MemoramaMode>().CheckMatch(this);
    }

    public void ResetCard()
    {
        isFlipped = false;
        image.sprite = backImage; // Show back image
    }
}
