using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballController : MonoBehaviour
{
    private float speed;
    public float destination;

    // Use after destroy the obstacles
    public ParticleSystem disolveParticle;

    [SerializeField] private AudioSource exlosionSFX;
    
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        // Move forward till reach the destination
        if (transform.position.x < destination)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
            Destroy(gameObject);
    }


    // When it hits an obstacle, destroys it
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Instantiate(disolveParticle, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            exlosionSFX.Play();
        }

        if (other.CompareTag("Boss"))
        {
            Instantiate(disolveParticle, other.transform.position, Quaternion.identity);
            exlosionSFX.Play();
        }
    }
}
