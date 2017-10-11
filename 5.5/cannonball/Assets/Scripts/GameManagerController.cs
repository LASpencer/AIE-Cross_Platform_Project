using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour {

    public int Score = 0;
    public Text ScoreText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ScoreText.text = "Score: " + Score.ToString();
	}

    public void EndGame()
    {
        GameController.Instance.LastScore = Score;
        GameController.Instance.HighScore = Mathf.Max(Score, GameController.Instance.HighScore);
        SceneManager.LoadSceneAsync("GameOverScene");
    }

    public void RestartGame()
    {
        Score = 0;
        foreach (var enemy in FindObjectsOfType<EnemyController>())
        {
            Destroy(enemy.gameObject);
        }
        foreach (var spawner in FindObjectsOfType<SpawnerController>())
        {
            spawner.SpawnTimer = spawner.SpawnRate;
        }
        foreach (var arrow in FindObjectsOfType<ArrowController>())
        {
            Destroy(arrow.gameObject);
        }

        FindObjectOfType<PlayerController>().RestartGame();
    }
}
