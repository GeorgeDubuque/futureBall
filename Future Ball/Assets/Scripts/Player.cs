using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float movementSpeed = 5f;
    public float currSpeed = 0;
    Path path;
    Vector2 prevDestination;
    public Vector2 currDestination;
    Vector2 currDirection;
    Vector2 playerPos;
    bool atDestination = false;
    public Transform shield;
    public bool seeingFuture = false;
    public GameManager gameManager;
    CircleCollider2D collider;
    Rigidbody2D rb;
    public bool controlsEnabled = true;
    public GameObject playerBroken;
    public Hearts hearts;
    
    void Start()
    {
        path = GameObject.FindGameObjectWithTag("Path").GetComponent<Path>();
        currDestination = path.Next();
        currDirection = GetCurrDirection(currDestination);
        transform.position = currDestination;
        collider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = (int)Input.GetAxis("Horizontal");
        float verticalInput = (int)Input.GetAxis("Vertical");
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Vector3 direction = new Vector3(verticalInput, -horizontalInput, 0);
        Vector3 direction = mousePosition - transform.position;
        direction = new Vector3(direction.y, -direction.x, 0);

        RotateShield(direction);
        CheckDestination();

        if (seeingFuture)
        {
            currDirection = GetCurrDirection(currDestination);
        }
        else
        {
            if (controlsEnabled)
            {
                currDirection = new Vector2(horizontalInput, verticalInput).normalized;
            }
        }

        playerPos = new Vector2(transform.position.x, transform.position.y);

        if (!path.isFinished)
        {
            MoveRb();
        }
        else
        {
            if (seeingFuture)
            {
                gameManager.ResetLevel();
            }
        }
    }

    private void CheckDestination()
    {
        float distanceFromDest = Vector2.Distance(playerPos,currDestination);
        atDestination = distanceFromDest < 0.05f;

        if (atDestination)
        {
            transform.position = currDestination;
            currDestination = path.Next();
        }
    }

    private void RotateShield(Vector3 direction)
    {
        if(direction.magnitude > 0)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            shield.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
    private void MoveRb()
    {
        rb.velocity = (currDirection * currSpeed);
    }

    private void Move()
    {
        transform.Translate(currDirection * currSpeed * Time.deltaTime);
    }

    Vector2 GetCurrDirection(Vector2 destination)
    {
        return (destination - playerPos).normalized;
    }

    public void StartFutureSelf()
    {
        collider.isTrigger = true;
        shield.gameObject.SetActive(false);
        seeingFuture = true;
        Color shieldColor = shield.GetComponent<SpriteRenderer>().color;
        shieldColor.a = .5f;
        shield.GetComponent<SpriteRenderer>().color = shieldColor;

        Color playerColor = transform.GetComponent<MeshRenderer>().material.color;
        playerColor.a = .5f;
        transform.GetComponent<MeshRenderer>().material.color = playerColor;

        currSpeed = movementSpeed;
    }

    public void StartPresentSelf()
    {
        controlsEnabled = true;
        collider.isTrigger = false;
        shield.gameObject.SetActive(true);
        Color shieldColor = shield.GetComponent<SpriteRenderer>().color;
        shieldColor.a = 1f;
        shield.GetComponent<SpriteRenderer>().color = shieldColor;

        Color playerColor = transform.GetComponent<MeshRenderer>().sharedMaterial.color;
        playerColor.a = 1f;
        transform.GetComponent<MeshRenderer>().sharedMaterial.color = playerColor;

        currSpeed = movementSpeed;
    }

    public void DisablePlayer()
    {
        shield.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

     
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") && collision.otherCollider.CompareTag("Player"))
        {
            Shot shot = collision.collider.GetComponent<Shot>();
            if (!shot.hasHit)
            {
                shot.hasHit = true;
                if (hearts.BreakHeart())
                {
                    GameObject brokenClone = Instantiate(playerBroken, null);
                    brokenClone.transform.position = transform.position;
                    Destroy(brokenClone, 3f);
                    DisablePlayer();
                    gameManager.ResetLevelAfterSeconds();
                }
            }
        }
    }
}
