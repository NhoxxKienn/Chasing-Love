using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public ParticleSystem explosionParticle;
    [SerializeField] private AudioSource explosionSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
            explosionSFX.Play();   
            Destroy(gameObject);
        }
    }
}
