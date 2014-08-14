using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;

public class ScoreCardScript : MonoBehaviour
{
    public float timeOnScreen = 2;
    public bool getTotalScore = false;
    private VignetteScript fade;
    private Timer lifetime;
	
	void Start()
    {
        fade = transform.GetComponentInChildren<VignetteScript>();
        fade.FadeIn();
        lifetime = new Timer(timeOnScreen + fade.fadeInTime, () => FadeOut());
        lifetime.Restart();
        if (getTotalScore)
        {
            int totalLosses = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>().GetTotal();
            GetComponent<GUIText>().text = totalLosses.ToString();
            Application.ExternalCall("TotalLosses", totalLosses);
        }
        else
        {
            ScoreScript score = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>();
            int losses = score.GetMostRecent();
            GetComponent<GUIText>().text = losses.ToString();
            Application.ExternalCall("LossesOnLevel", score.PreviousScene, losses);
        }
    }
	
	
	void Update()
    {
        lifetime.Update(Time.deltaTime);
	}

    private void FadeOut()
    {
        fade.FadeOut();
        lifetime.SetTargetTime(fade.fadeOutTime);
        lifetime.SetEventFunction(() => SwitchScene());
        lifetime.Restart();
    } 

    private void SwitchScene()
    {
        Application.LoadLevel(GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreScript>().FollowingScene);
    }
}
