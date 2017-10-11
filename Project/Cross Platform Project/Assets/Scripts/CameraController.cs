using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour {

    // Position for player in screen space
    public Vector3 TargetPosition;
    public GameObject Player;

    Camera cam;

	// Use this for initialization
	void Start () {
        cam = gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        // Move camera to put player at set position on screen
        Vector3 playerPos = Player.transform.position;
        Ray targetRay = cam.ScreenPointToRay(TargetPosition);
        Plane playerPlane = new Plane(transform.forward, playerPos);
        float distance;
        playerPlane.Raycast(targetRay, out distance);
        Vector3 point = targetRay.GetPoint(distance);
        Vector3 offset = playerPos - point;
        transform.position += offset;
	}
}
