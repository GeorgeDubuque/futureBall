using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public float rotateSpeed = 5f;
    Material material;
    public float sampleX = 0;
    public float noiseSpeed = 1f;
    public float pullForce = 5f;
    float currPullForce;
    public float pullRate = 1f;
    public bool doesPullPlayer = false;
    public bool isForLevelSelection = false;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        currPullForce = pullForce;
        anim = GetComponentInParent<Animator>();
    }

    private void OnMouseDown()
    {
        if (isForLevelSelection)
        {
            Debug.Log("mouseDown");
            anim.SetTrigger("Pulse"); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("_Offset", new Vector4(sampleX, 0, 0));
        GetComponent<MeshRenderer>().sortingOrder = 10;
        sampleX += noiseSpeed * Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 suckDirection = transform.position - other.transform.position;
            Rigidbody2D playerRb = other.GetComponentInParent<Rigidbody2D>();
            Player player = other.GetComponentInParent<Player>();
            player.controlsEnabled = false;

            if (!player.seeingFuture && doesPullPlayer)
            {
                if(Vector2.Distance(other.transform.position, transform.position) > 0.01f)
                {
                    playerRb.velocity = suckDirection * currPullForce;
                }
                else
                {
                    other.transform.position = transform.position;
                    playerRb.velocity = Vector3.zero;
                    player.DisablePlayer();
                    anim.SetTrigger("Disappear");
                }
            }
        }
    }

    public void LoadLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}
