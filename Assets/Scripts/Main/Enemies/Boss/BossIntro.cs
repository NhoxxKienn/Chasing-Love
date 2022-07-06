using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour
{
    private float jumpSpeed = 20f;
    private float topBorder = 7f;
    [SerializeField] private GameObject actualBoss;
    [SerializeField] private ParticleSystem waterSproutParticle;
    private float timer = 2.0f; 

    // Start is called before the first frame update
    void Start()
    {
        waterSproutParticle.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (transform.position.y < topBorder)
            {
                transform.Translate(Vector2.up * jumpSpeed * Time.deltaTime);
            }
            else
            {
                Instantiate(actualBoss, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
