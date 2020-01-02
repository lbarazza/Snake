using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoardScript : MonoBehaviour
{

    public int height = 10;
    public int width = 10;

    public GameObject tile;
    public GameObject foodPrefab;

    private GameObject food;
    [HideInInspector] public Vector3 foodPosition;
    private GameObject[] snakes;

    void Start()
    {
        CreateBoard();
        snakes = GameObject.FindGameObjectsWithTag("Snake");

        CreateFood();

        //Debug.Log(snakes);

    }

    void Update()
    {

    }

    void CreateFood()
    {
        bool samePosition = true;
        while (samePosition)
        {
          samePosition = false;
          foodPosition = new Vector3(Random.Range(-5,6), Random.Range(-5,6), 0);
          foreach(GameObject snake in snakes)
          {
              SnakeManager snakeScript = snake.GetComponent<SnakeManager>();
              for (int i = 0; i < snakeScript.bodyParts.Count; i++)
              {
                  if (foodPosition == snakeScript.bodyParts[i].transform.position)
                  {
                      samePosition = true;
                  }
              }
          }

        }
        food = Instantiate(foodPrefab, foodPosition, Quaternion.identity);
    }

    public void EatFood()
    {
        Destroy(food);
        GameObject snake = snakes[0];
        SnakeManager snakeScript = snake.GetComponent<SnakeManager>();
        snakeScript.updatingLength = true;

        CreateFood();

    }

    void CreateBoard()
    {

        transform.position = new Vector3(-width/2.0f+0.5f , -height/2.0f+0.5f, 0);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject instance = Instantiate(tile, new Vector3(x, y, 0), Quaternion.Euler(0, 0, 0));
                instance.transform.SetParent(transform, false);
            }
        }
    }

    /*void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }*/

}
