using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

    public GameObject zombie_prefab;
    public Vector3 Zombie_spawn_pos;
    public float SpawnRate = 5.0f;

    public float SpawnTimer;

	// Use this for initialization
	void Start () {
        SpawnTimer = SpawnRate;
	}

    // Update is called once per frame
    void Update()
    {
        SpawnTimer -= Time.deltaTime;
        if (SpawnTimer < 0.0f)
        {
            GameObject zombie = (GameObject)Instantiate(zombie_prefab);
            zombie.transform.position = Zombie_spawn_pos;
            SpawnTimer = SpawnRate;
        }
    }
}
