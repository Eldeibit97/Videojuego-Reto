using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class TimeManager : MonoBehaviour {
	public float time = 60;
	public TextMeshProUGUI TimeText;
	public AudioSource countdownSE;
    private AudioSource audioSource;
    private bool countdownSEPlayed = false; 
	
	void Start () 
    {
		TimeText.text = ((int)time).ToString();
	}
	
	void Update ()
    {
		time -= Time.deltaTime;

		if (time <= 9f && !countdownSEPlayed)
        {
            countdownSE.Play();
            countdownSEPlayed = true;
        }
		
		if (time < 0) // When the time is over
		{
			time = 0;
			PlayerPrefs.SetString("ClearChecker", "Game Over");
			SceneManager.LoadScene(6);
		}
        TimeText.text = ((int)time).ToString();	
    }
}