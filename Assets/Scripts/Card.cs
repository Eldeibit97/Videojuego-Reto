using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public int cardID;                 // Unique ID for matching
    public Sprite frontImage;         // Card's front image
    public Sprite backImage;          // Card's back image
    public GameObject CardSelectedPrefab;

    private bool isFlipped = false;
    private bool isAnimating = false;
    private Button button;
    private Image image;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(FlipCard);
        ResetCard();
    }

    public void FlipCard()
    {
        if (isFlipped || isAnimating) return;

        GameObject sound = Instantiate(CardSelectedPrefab);
        AudioSource audio = sound.GetComponent<AudioSource>();
        audio.Play();
        Destroy(sound, audio.clip.length);

        isFlipped = true;
        StartCoroutine(FlipCardAnimation());

        MemoramaMode gameManager = FindFirstObjectByType<MemoramaMode>();
        if (gameManager != null)
        {
            gameManager.CheckMatch(this);
        }
    }

    private IEnumerator FlipCardAnimation()
    {
        isAnimating = true;

        for (float t = 0; t < 1; t += Time.deltaTime * 5f)
        {
            float scale = Mathf.Lerp(1f, 0f, t);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        transform.localScale = new Vector3(0f, 1f, 1f);
        image.sprite = frontImage;

        for (float t = 0; t < 1; t += Time.deltaTime * 5f)
        {
            float scale = Mathf.Lerp(0f, -1f, t);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        transform.localScale = new Vector3(-1f, 1f, 1f);
        isAnimating = false;
    }

    public void ResetCard()
    {
        isFlipped = false;
        image.sprite = backImage;
        transform.localScale = Vector3.one;
    }

    public void HideCard()
    {
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        if (canvas == null)
            canvas = gameObject.AddComponent<CanvasGroup>();

        canvas.alpha = 0;
        canvas.interactable = false;
        canvas.blocksRaycasts = false;
    }

    public IEnumerator ShakeCard()
    {
        Vector3 originalPos = transform.localPosition;
        float duration = 0.3f;
        float strength = 10f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * strength;
            float offsetY = Random.Range(-1f, 1f) * strength;
            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public IEnumerator PlayEntryAnimation(float delay = 0f)
    {
        isAnimating = true;
        transform.localScale = Vector3.zero;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        float duration = 0.4f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float scale = Mathf.SmoothStep(0f, 1f, t);
            transform.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }

        transform.localScale = Vector3.one;
        isAnimating = false;
    }
}
