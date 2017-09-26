using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArrowController : MonoBehaviour {

    public float Lifetime = 4.0f;

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
                    //TODO score points for hit
                    //TODO don't immediately destroy arrow?
                    //TODO variable damage amount?
                    hit.gameObject.GetComponent<EnemyController>().Damage(1);
                    Destroy(this.gameObject);
                }
                break;
            case "Ground":
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                live = false;
                break;
            default:
                break;
        }
    }
}
