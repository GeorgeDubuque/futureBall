using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Transform> positions;
    public Transform position;
    int index = 0;
    public bool isFinished = false;

    public Vector2 Next()
    {
        if(index < positions.Count)
        {
            position = positions[index];
        }
        else
        {
            isFinished = true; 
        }
        index += 1;
        return position.position;
    }

    public Vector3 ResetPath()
    {
        index = 0;
        isFinished = false;
        return positions[index].position;
    }
}
