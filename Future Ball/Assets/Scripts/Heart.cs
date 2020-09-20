using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rbLeft;
    public Rigidbody2D rbRight;
    public Transform leftForcePos;
    public Transform rightForcePos;
    public float breakForce = 1f;

    public void Break()
    {
        rbLeft.constraints = RigidbodyConstraints2D.None;
        rbRight.constraints = RigidbodyConstraints2D.None;
        rbLeft.AddForce(Vector3.left * breakForce);
        rbRight.AddForce(Vector3.right * breakForce);
        Destroy(gameObject, 2);
    }
}
