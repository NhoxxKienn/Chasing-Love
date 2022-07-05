using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    private float reloadTime = 2.5f;

    public GameObject bulletPrefab;
    private float bulletHeight = -2.65f;

    // timer and Indicator when about to shoot the bullet
    private float timer;
    public GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        timer = reloadTime - 1f;
        StartCoroutine(shootBullet());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator shootBullet()
    {
        while (true)
        {
            Instantiate(bulletPrefab, new Vector3(transform.position.x, bulletHeight, 0), bulletPrefab.transform.rotation);
            yield return new WaitForSecondsRealtime(reloadTime);
        }
    }
}
