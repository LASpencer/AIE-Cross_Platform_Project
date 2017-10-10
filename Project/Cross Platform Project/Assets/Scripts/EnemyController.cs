using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
            //score points for kill
            GameManager.Score += KillPoints;
            Destroy(this.gameObject);
        }
        //TODO movement pattern (ie lurching zombies)
        Vector3 movement = new Vector3(-speed * Time.deltaTime, 0, 0);
        transform.position += movement;
	}

    public void Damage(float amount)
    {
        health -= amount;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "KillBox")
        {
            other.GetComponentInParent<PlayerController>().Damage(1);
            Destroy(this.gameObject);
        }
    }
}
