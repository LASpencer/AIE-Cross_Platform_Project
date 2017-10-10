using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArrowController : MonoBehaviour {

    // Time until arrow is destroyed
    public float Lifetime = 4.0f;

    // Live arrows can kill enemies
    public bool live = true;

	// Use this for initialization
	void Start () {
        live = true;
	}
	
	// Update is called once per frame
	void Update () {
        Lifetime -= Time.deltaTime;
        if(Lifetime < 0)
        {
            Destroy(this.gameObject);
        }
        // Dead arrows can't move
        if (!live)
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
	}

    void OnCollisionEnter(Collision hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Enemy":
                if (live)
                {
                    hit.gameObject.GetComponent<EnemyController>().Damage(1);
                    Destroy(this.gameObject);
                }
                break;
            case "Ground":
                // Stop arrow and kill it on touching ground
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                live = false;
                break;
            default:
                break;
        }
    }
}
