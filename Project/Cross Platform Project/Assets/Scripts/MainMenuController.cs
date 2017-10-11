using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_ANDROID
        Screen.orientation = ScreenOrientation.LandscapeLeft;
#endif
#if UNITY_IOS
        Screen.orientation = ScreenOrientation.LandscapeLeft;
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Play()
    {
        SceneManager.LoadSceneAsync("ShootingScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
