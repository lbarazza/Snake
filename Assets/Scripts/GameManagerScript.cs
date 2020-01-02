using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public GameObject snakePrefab;
    public GameObject boardPrefab;

    public Text scoreText;

    private static Text scoreTextStatic;
    private static int score;
    private static GameObject snake;
    private static SnakeManager snakeScript;
    private static GameObject board;
    private static GameBoardScript boardScript;
    void Start()
    {
        scoreTextStatic = scoreText;
        score = 0;
        snake = Instantiate(snakePrefab);
        snakeScript = snake.GetComponent<SnakeManager>();
        board = Instantiate(boardPrefab);
        boardScript = board.GetComponent<GameBoardScript>();
    }

    void LateUpdate()
    {
        /*if (head.position.x < -5 || head.position.x > 5)
        {
            Destroy(head.gameObject);
        }*/
        //Debug.Log(boardScript.foodPosition);
        //Debug.Log(snakeScript.bodyParts[0].transform.position);
        if (!snakeScript.isUpdating)
        {
            if (boardScript.foodPosition == snakeScript.bodyParts[0].transform.position)
            {
                //Debug.Log("Hit!");
                boardScript.EatFood();
                score++;
                //, snakeScript.bodyParts[0].transform.position);
                scoreText.text = "Score: " + score;
            }
        }
    }

    public static void ResetGame()
    {
        score = 0;
        snakeScript.Init();
        scoreTextStatic.text = "Score: " + score;
    }

}
