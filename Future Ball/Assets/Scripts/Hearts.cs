using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearts : MonoBehaviour
{
    public int numHearts = 3;
    public GameObject heartPrefab;
    public List<Heart> hearts;
    public List<Vector3> heartPositions;
    int currHeart = 0;

    public void Start()
    {
        foreach(var heart in hearts)
        {
            heartPositions.Add(heart.transform.position);
        }
    }

    public bool BreakHeart()
    {
        hearts[currHeart].Break();
        currHeart += 1;
        return currHeart >= hearts.Count;
    }

    public void ResetHearts()
    {
        for(int i = 0; i < numHearts; i++)
        {
            GameObject currHeart = Instantiate(heartPrefab, transform);
            currHeart.transform.position = heartPositions[i];
            hearts.Insert(0, currHeart.GetComponent<Heart>());
        }
    }
}
