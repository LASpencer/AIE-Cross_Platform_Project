using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO add requirements
//TODO create an enemy spawner
public class EnemyController : MonoBehaviour {

    public float health = 1.0f;
    public float speed = 3.0f;
    public int KillPoints;
    public GameManagerController GameManager;
	// Use this for initialization
	void Start () {
        GameManager = FindObjectOfType<GameManagerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0)
        {
            //TODO do some dying animation, etc
            //TODO score points for kill
            GameManager.Score += KillPoints;
            Destroy(this.gameObject);
        }
        //TODO movement pattern (ie lurching zombies)
        Vector3 movement = new Vector3(-speed * Time.deltaTime, 0, 0);
        transform.position += movement;
        //TODO oncollision/trigger enter with player, game over
	}

    public void Damage(float amount)
    {
        // TODO do hurt animation
        // TODO min of 0, can only reduce not increase health
        health -= amount;
    }
}
