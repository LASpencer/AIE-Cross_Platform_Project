﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO add required components
public class PlayerController : MonoBehaviour {

    public float MaxArrowCharge = 10.0f;
    public float FullChargeTime = 2.0f;
    public GameObject Projectile;
    public Slider ChargeSlider;

    public float ChargeTime = 0.0f;

    [HideInInspector]
    public Vector3 AimDirection;

	// Use this for initialization
	void Start () {
        AimDirection = new Vector3(1, 0, 0);
	}

    // Update is called once per frame
    void Update()
    {
        //TODO aim towards point (mouse position/touch position for web and phone, by vertical axis movement for web and ps4)
        UpdateAimDirection();
        transform.Find("Arm").transform.up = AimDirection;
        //TODO check if holding down fire, if so charge up
        if (FireHeld())
        {
            ChargeTime = Mathf.Min(ChargeTime + Time.deltaTime, FullChargeTime);
            //TODO some UI element/animations/sounds to show charging up
        }
        else
        {
            // On release, fire arrow at chosen power and direction
            if(ChargeTime > 0)
            {
                ShootArrow();
            }
            ChargeTime = 0;
        }

        //TODO nicer charge slider animations
        ChargeSlider.value = ChargeTime / FullChargeTime;
    }

    void UpdateAimDirection()
    {
        //TODO conditionally compile based on platform

        // Windows/Web version
        Vector3 direction = GetDirectionToPoint(Input.mousePosition);
        //TODO clamp within range [1,0] [0,1] counterclockwise
        AimDirection = direction;

        // TODO playstation version moves based on axis

    }

    // Calculates the vector from the player to the position selected on the screen
    Vector3 GetDirectionToPoint(Vector3 point)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(point);
        Plane playerPlane = new Plane(Vector3.back, transform.position);
        float distance = 0;
        playerPlane.Raycast(cameraRay, out distance);
        Vector3 aimPoint = cameraRay.GetPoint(distance);
        Vector3 direction = aimPoint - transform.position;
        return direction.normalized;
    }

    // Returns true if player is holding the fire input
    bool FireHeld()
    {
        //TODO conditional compilation

        // PC/Web/PS4 version
        if(Input.GetAxis("Fire1") > 0)
        {
            return true;
        } else
        {
            return false;
        }

        // TODO touchscreen version
    }

    void ShootArrow()
    {
        //TODO calculate force magnitude more appropriately, set direction
        //TODO arrow has min charge
        float arrowCharge = MaxArrowCharge * ChargeTime / FullChargeTime;
        Vector3 arrowForce = AimDirection * arrowCharge;

        GameObject arrow = (GameObject)Instantiate(Projectile);

        // TODO set possition appropriately
        arrow.transform.position = transform.position;


        arrow.GetComponent<Rigidbody>().AddForce(arrowForce, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                //TODO trigger killing player, game over
                break;
            default:
                break;
        }
    }
}
