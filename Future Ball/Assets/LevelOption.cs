using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOption : MonoBehaviour
{
    public float rotateSpeed = 5f;
    public float noiseSpeed = 1f;
    public float sampleX = 0;
    Animator anim;
    public string sceneName;
    public GameManager gameManager;
    public string functionName;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
        GetComponent<MeshRenderer>().sharedMaterial.SetVector("_Offset", new Vector4(sampleX, 0, 0));
        GetComponent<MeshRenderer>().sortingOrder = 10;
        sampleX += noiseSpeed * Time.deltaTime;
    }
    private void OnMouseDown()
    {
        anim.SetTrigger("Pulse");
        StartCoroutine(functionName, .15f);
    }

    public void WaitForSeconds()
    {
        StartCoroutine("WaitForSeconds", 3);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator StartLevel(float t)
    {
        yield return new WaitForSeconds(t);
        gameManager.StartLevel();
    }

    IEnumerator StartFutureLevel(float t)
    {
        yield return new WaitForSeconds(t);
        gameManager.StartFutureLevel();
    }

    public void AnimationCallback(string functionName)
    {
        StartCoroutine(functionName);
    }
}
