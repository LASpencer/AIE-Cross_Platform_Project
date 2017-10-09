using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HACK enumeration for testing different control schemes
public enum ControlType
{
    Mouse,
    Axis
}

// TODO add required components
public class PlayerController : MonoBehaviour {

    public float MinArrowCharge = 5.0f;
    public float MaxArrowCharge = 20.0f;
    public float FullChargeTime = 2.0f;
    public float ArmRotateRate = 100.0f;
    public float MaxHealth = 3.0f;
    public float Health;
    public GameObject Projectile;
    public Slider ChargeSlider;
    public Slider HealthSlider;
    public ControlType Control = ControlType.Mouse;
    public GameManagerController GameManager;

    public float ChargeTime = 0.0f;

    [HideInInspector]
    public Vector3 AimDirection;

    [HideInInspector]
    public Transform MuzzleTransform;

	// Use this for initialization
	void Start () {
        GameManager = FindObjectOfType<GameManagerController>();
        Health = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = Health;
        AimDirection = new Vector3(1, 0, 0);
        MuzzleTransform = transform.Find("Arm").Find("Muzzle");
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

        if(Health == 0.0f)
        {
            GameManager.EndGame();
        }

        //TODO nicer charge slider animations
        ChargeSlider.value = ChargeTime / FullChargeTime;

        HealthSlider.value = Health;
    }

    public void RestartGame()
    {
        Health = MaxHealth;
        ChargeTime = 0;
    }

    void UpdateAimDirection()
    {
        //TODO conditionally compile based on platform

        //HACK using switch to test out different controls

        switch (Control) {
            case ControlType.Mouse:
            // Windows/Web version
            Vector3 direction = GetDirectionToPoint(Input.mousePosition);
            //TODO clamp within range [1,0] [0,1] counterclockwise
            AimDirection = direction;
                break;
            case ControlType.Axis:
                //TODO clamp within range [1,0] [0,1] counterclockwise
                AimDirection = Quaternion.AngleAxis(Input.GetAxis("Vertical") * ArmRotateRate * Time.deltaTime, transform.forward) * AimDirection;
                break;
            default:
                break;
    }
        // Clamp rotation to 90 degree firing arc
        //HACK could rewrite to allow different arcs
        if(AimDirection.y < 0)
        {
            AimDirection = new Vector3(1, 0, 0);
        } else if (AimDirection.x < 0)
        {
            AimDirection = new Vector3(0, 1, 0);
        }

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
        float arrowCharge = Mathf.Lerp(MinArrowCharge,MaxArrowCharge, ChargeTime / FullChargeTime);
        Vector3 arrowForce = AimDirection * arrowCharge;

        GameObject arrow = (GameObject)Instantiate(Projectile);

        // TODO set position appropriately
        arrow.transform.position = MuzzleTransform.position;


        arrow.GetComponent<Rigidbody>().AddForce(arrowForce, ForceMode.Impulse);
    }

    public void Damage(float damage)
    {
        Health = Mathf.Max(Health - damage, 0);
    }
}
