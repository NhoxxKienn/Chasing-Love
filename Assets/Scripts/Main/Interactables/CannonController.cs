using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    // Cannonball (Projectile)
    public GameObject cannonballPrefab;
    private float cannonballHeight = -2.5f;

    // Charging Bar (UI)
    public GameObject chargingBarPrefab;
    private GameObject chargingBar;

    // Shooting SFX
    [SerializeField] private AudioSource shootingSFX;

    // Minimum travel distance of the cannonball
    private float minDistance = 0;

    private float chargeRate = 20;
    private float maxCharge = 6;
    private float charge;


    // To check if the cannon has shot the cannonball or not yet.
    private bool hasShot;

    // Check if the player is inside the collider
    [SerializeField]private bool isInside;


    // Start is called before the first frame update
    void Start()
    {
        charge = 0;
        hasShot = false;
        isInside = false;
    }

    // Update is called once per frame
    void Update()
    {
        // While holding down space, charge the bar
        if (Input.GetKey(KeyCode.Space) && !hasShot && isInside)
        {
            if (!chargingBar.activeSelf)
            {
                chargingBar.SetActive(true);
            }
            
            if (charge < maxCharge) // charging
            {
                charge += (chargeRate * Time.deltaTime);
            }
            else // After fully charge, shoot the cannonball
            {
                chargingBar.GetComponent<Animator>().SetBool("isFull", true);
                GameObject cannonball = Instantiate(cannonballPrefab,
                                  new Vector3(transform.position.x, cannonballHeight, 0),
                                  cannonballPrefab.transform.rotation);
                cannonball.GetComponent<CannonballController>().destination = transform.position.x + minDistance + charge;
                chargingBar.SetActive(false);
                charge = 0;
                hasShot = true;
                shootingSFX.Play();
            }
        }

        // If release the spacebar, shoot the cannonball
        if (Input.GetKeyUp(KeyCode.Space) && isInside && !hasShot)
        {
            GameObject cannonball = Instantiate(cannonballPrefab,
                    new Vector3(transform.position.x, cannonballHeight, 0),
                    cannonballPrefab.transform.rotation);
            cannonball.GetComponent<CannonballController>().destination = transform.position.x + minDistance + charge;
            chargingBar.SetActive(false);
            charge = 0;
            hasShot = true;
            shootingSFX.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            chargingBar = Instantiate(chargingBarPrefab, new Vector3(-5, -4, 3), chargingBarPrefab.transform.rotation);
            chargingBar.SetActive(false);
            isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInside = false;
            Destroy(chargingBar);
        }
    }
}