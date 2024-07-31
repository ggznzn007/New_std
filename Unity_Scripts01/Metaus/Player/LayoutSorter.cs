using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class LayoutSorter : MonoBehaviour
{
    private SpriteRenderer parentsRenderer;
    private List<Obstacle> obstacles = new List<Obstacle>();

    public void Start()
    {
        parentsRenderer = transform.parent.GetComponent<SpriteRenderer>();        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Debug.Log("triggerEnter");
            Obstacle ob = collision.GetComponent<Obstacle>();
            //SpriteRenderer obSpriteRenderer = ob.mySpriteRenderer;
            if (obstacles.Count == 0 || ob.mySpriteRenderer.sortingOrder - 1 < parentsRenderer.sortingOrder)
            {
                parentsRenderer.sortingOrder = ob.mySpriteRenderer.sortingOrder - 1;
            }
            obstacles.Add(ob);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Debug.Log("triggerExit");
            Obstacle ob = collision.GetComponent<Obstacle>();
            obstacles.Remove(ob);
            if (obstacles.Count == 0)
            {
                parentsRenderer.sortingOrder = 200;
            }
            else
            {
                obstacles.Sort();
                parentsRenderer.sortingOrder = obstacles[0].mySpriteRenderer.sortingOrder - 1;
            }
        }
    }
}
