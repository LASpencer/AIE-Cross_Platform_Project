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

public class PlayerController : MonoBehaviour {

    public float MinArrowCharge = 5.0f;         // Force on arrow at 0 charge
    public float MaxArrowCharge = 20.0f;        // Force on arrow at full charge
    public float FullChargeTime = 2.0f;         // Time to reach full charge
    public float ArmRotateRate = 100.0f;        // Rate of arm rotation when controlled by axis
    public float MaxHealth = 3.0f;
    public float Health;
    public GameObject Projectile;
    public Slider ChargeSlider;
    public Slider HealthSlider;
    public ControlType Control = ControlType.Mouse;
    public GameManagerController GameManager;

    public float ChargeTime = 0.0f;

    [HideInInspector]
    public Vector3 AimDirection;            // Direction aimed at

    [HideInInspector]
    public Transform MuzzleTransform;       // End of arm

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
        UpdateAimDirection();
        transform.Find("Arm").transform.up = AimDirection;
        // check if holding down fire, if so charge up
        if (FireHeld())
        {
            ChargeTime = Mathf.Min(ChargeTime + Time.deltaTime, FullChargeTime);
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
            gameObject.SetActive(false);
            GameManager.EndGame();
        }

        // Update slider values
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
#if UNITY_EDITOR
        switch (Control)
        {
            case ControlType.Mouse:
                // Windows/Web version
                AimDirection = GetDirectionToPoint(Input.mousePosition);
                break;
            case ControlType.Axis:
                AimDirection = Quaternion.AngleAxis(Input.GetAxis("Vertical") * ArmRotateRate * Time.deltaTime, transform.forward) * AimDirection;
                break;
            default:
                break;
        }
#endif
#if UNITY_STANDALONE_WIN
        AimDirection = GetDirectionToPoint(Input.mousePosition);
#endif
#if UNITY_WEBGL
        AimDirection = GetDirectionToPoint(Input.mousePosition);
#endif
#if UNITY_ANDROID
        if(Input.touches.Length > 0){
            Touch t = Input.touches[0];
            AimDirection = GetDirectionToPoint(t.position);
        }
#endif
#if UNITY_IOS
        if(Input.touches.Length > 0){
            Touch t = Input.touches[0];
            AimDirection = GetDirectionToPoint(t.position);
        }
#endif
#if UNITY_PS4
        AimDirection = Quaternion.AngleAxis(Input.GetAxis("Vertical") * ArmRotateRate * Time.deltaTime, transform.forward) * AimDirection;
#endif

        // Clamp rotation to 90 degree firing arc
        if (AimDirection.y < 0)
        {
            AimDirection = new Vector3(1, 0, 0);
        } else if (AimDirection.x < 0)
        {
            AimDirection = new Vector3(0, 1, 0);
        }
        

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
#if UNITY_EDITOR
        // PC/Web/PS4 version
        if(Input.GetAxis("Fire1") > 0)
        {
            return true;
        } else
        {
            return false;
        }
#endif
#if UNITY_STANDALONE_WIN
        if(Input.GetAxis("Fire1") > 0)
        {
            return true;
        } else
        {
            return false;
        }
#endif
#if UNITY_WEBGL
        if (Input.GetAxis("Fire1") > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
#endif
#if UNITY_ANDROID
        if(Input.touches.Length > 0){
            Touch t = Input.touches[0];
            if(t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary){
                return true;    
            } else {
                return false;
            }
        } else {
            return false;
        }
#endif
#if UNITY_IOS
        if(Input.touches.Length > 0){
            Touch t = Input.touches[0];
            if(t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary){
                return true;    
            } else {
                return false;
            }
        } else {
            return false;
        }
#endif
#if UNITY_PS4
        if (Input.GetAxis("Fire1") > 0)     // Square button
        {
            return true;
        }
        else
        {
            return false;
        }
#endif
    }

    void ShootArrow()
    {
        float arrowCharge = Mathf.Lerp(MinArrowCharge,MaxArrowCharge, ChargeTime / FullChargeTime);
        Vector3 arrowForce = AimDirection * arrowCharge;

        GameObject arrow = (GameObject)Instantiate(Projectile);

        // Arrow starts at muzzle point
        arrow.transform.position = MuzzleTransform.position;

        arrow.GetComponent<Rigidbody>().AddForce(arrowForce, ForceMode.Impulse);
    }

    public void Damage(float damage)
    {
        Health = Mathf.Max(Health - damage, 0);
    }
}
