using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gridSize = 100;
    public GameObject gridLine;
    public Player player;
    public GameObject menuButtons;
    public Path path;
    // Start is called before the first frame update
    void Start()
    {
        for(int x = 0; x < gridSize; x++)
        {
            var gridLineClone = GameObject.Instantiate(gridLine, null);
            gridLine.transform.position = new Vector3(x - gridSize/2, 0, 1);
        }
        for(int y = 0; y < gridSize; y++)
        {
            var gridLineClone = GameObject.Instantiate(gridLine, null);
            gridLineClone.transform.Rotate(new Vector3(0, 0, 90));
            gridLineClone.transform.position = new Vector3(0, y - gridSize/2, 1);
        }
        
    }

    public void StartLevel()
    {
        player.StartPresentSelf();
        menuButtons.SetActive(false);
    }

    public void StartFutureLevel()
    {
        player.StartFutureSelf();
        menuButtons.SetActive(false);
    }

    public void ResetLevel()
    {
        Vector3 startPosition = path.ResetPath();
        player.currDestination = startPosition;
        player.transform.position = startPosition;
        ToggleMenu();
        player.seeingFuture = false;
        player.currSpeed = 0;
        List<Shooter> shooters = GameObject.FindObjectsOfType<Shooter>().ToList();
        foreach(Shooter shooter in shooters)
        {
            shooter.hasShot = false;
        }
    }

    public void ToggleMenu()
    {
        bool isMenuActive = menuButtons.activeSelf;
        menuButtons.SetActive(!isMenuActive);
    }
}
