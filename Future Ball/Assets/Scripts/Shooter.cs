using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public bool hasShot = false;
    public GameObject shotPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.up * 100, Color.red);

        if (!hasShot)
        {
            LayerMask playerLayerMask = LayerMask.GetMask("Player", "Wall");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity, playerLayerMask);
            if(hit && hit.transform.CompareTag("Player"))
            {
                    Shoot();
                    hasShot = true;
            }
        }
        
    }

    void Shoot()
    {
        GameObject bullet = GameObject.Instantiate(shotPrefab, null);
        bullet.transform.position = transform.position;
        bullet.GetComponent<Shot>().Shoot(transform.up);
    }
}
