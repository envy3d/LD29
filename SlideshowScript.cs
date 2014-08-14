using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class SlideshowScript : MonoBehaviour {

    public GUITexture[] slides;
    public float slideFadeOutTime = 1;
    public GUITexture reminder;
    public float reminderTime = 3;
    public float reminderBlendTime = 0.5f;
    public bool useInputReminder = false;
    public string nextScene;
    public bool killMusic = false;
    public AnimationCurve musicFade;
    public float musicFadeTime = 1;

    private Timer reminderTimer;
    private float time;
    private float remBlendTime;
    private int slideIdx = -1;
    private float reminderAlpha;
    private bool readyForNextScene = false;
    private bool reminderActive = true;

	void Start()
    {
        time = slideFadeOutTime;
        reminderTimer = new Timer(reminderTime, () => ShowReminder());
        remBlendTime = reminderBlendTime;
        transform.GetComponentInChildren<VignetteScript>().FadeIn();
	}
	
	void Update()
    {
        time += Time.deltaTime;
        
        if (time >= slideFadeOutTime)
        {
            if (slideIdx >= 0)
                slides[slideIdx].color = new Color(slides[slideIdx].color.r, slides[slideIdx].color.b, slides[slideIdx].color.g, 0);

            if (readyForNextScene)
                NextScene();
            else if (Input.GetButtonDown("MenuSelect"))
            {
                NextSlide();
                RemoveReminder();
                if (slideIdx == slides.Length - 1)
                {
                    readyForNextScene = true;
                    if (killMusic)
                    {
                        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicSingletonScript>().FadeOut(musicFade, musicFadeTime);
                    }
                }
            }
        }
        else if (time < slideFadeOutTime)
        {
            reminderTimer.Restart();
            slides[slideIdx].color = new Color(slides[slideIdx].color.r,
                                               slides[slideIdx].color.b,
                                               slides[slideIdx].color.g, 
                                               Mathf.Lerp(1,0,time/slideFadeOutTime));
        }

        UpdateReminder();
	}

    private void NextSlide()
    {
        time = 0;
        slideIdx++;
    }

    private void NextScene()
    {
        Application.LoadLevel(nextScene);
    }

    private void ShowReminder()
    {
        remBlendTime = 0;
        reminderActive = false;
    }

    private void RemoveReminder()
    {
        remBlendTime = 0;
        reminderActive = true;
        if (useInputReminder)
            reminderAlpha = reminder.color.a;
    }

    private void UpdateReminder()
    {
        reminderTimer.Update(Time.deltaTime);

        if (useInputReminder)
        {
            remBlendTime += Time.deltaTime;
            if (reminderActive)
            {
                if (remBlendTime < reminderBlendTime)
                {
                    reminder.color = new Color(reminder.color.r,
                                               reminder.color.b,
                                               reminder.color.g,
                                               Mathf.Lerp(reminderAlpha, 0, remBlendTime / reminderBlendTime));
                }
                else
                {
                    reminder.color = new Color(reminder.color.r, reminder.color.b, reminder.color.g, 0);
                }
            }
            else
            {
                if (remBlendTime < reminderBlendTime)
                {
                    reminder.color = new Color(reminder.color.r,
                                               reminder.color.b,
                                               reminder.color.g,
                                               Mathf.Lerp(0, 1, remBlendTime / reminderBlendTime));
                }
                else
                {
                    reminder.color = new Color(reminder.color.r, reminder.color.b, reminder.color.g, 1);   
                }
            }
        }
    }
}
