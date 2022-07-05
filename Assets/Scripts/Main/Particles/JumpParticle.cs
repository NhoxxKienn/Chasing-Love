using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpParticle : MonoBehaviour
{
    private float timeToLive = 0.15f;
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < timeToLive) time += Time.deltaTime;
        else { time = 0; gameObject.SetActive(false); }
    }
}
