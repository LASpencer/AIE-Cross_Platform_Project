using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        int score = GameController.Instance.LastScore;
        int highScore = GameController.Instance.HighScore;
        gameObject.GetComponent<Text>().text = "Score: " + score.ToString() + "\nHigh Score: " + highScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
