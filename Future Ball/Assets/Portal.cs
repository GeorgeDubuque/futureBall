using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public float rotateSpeed = 5f;
    Material material;
    public float sampleX = 0;
    public float noiseSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("_Offset", new Vector4(sampleX, 0, 0));
        GetComponent<MeshRenderer>().sortingOrder = 10;
        sampleX += noiseSpeed * Time.deltaTime;
    }
}
