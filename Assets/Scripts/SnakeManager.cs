using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{

    public int bodyLength = 5;
    public List<GameObject> prefabBodyParts;

    [HideInInspector] public List<Transform> bodyParts = new List<Transform>();
    private Vector3 direction;
    private bool directionChosen;
    [HideInInspector] public bool updatingLength;
    [HideInInspector] public bool isUpdating;

    void Start()
    {
        Init();
    }

    public void Init()
    {
      Spawn();
      isUpdating = false;
      direction = Vector3.up;
      directionChosen = false;
      updatingLength = false;
      InvokeRepeating("Move", 0.55f, 0.55f);
    }


    void Update()
    {
        if ((Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical")) && !directionChosen)
        {
            float dx = (float)Input.GetAxisRaw("Horizontal");
            float dy = (float)Input.GetAxisRaw("Vertical");

            if (dx != 0 && dy != 0)
                dy = 0;

            Vector3 new_direction = new Vector3(dx, dy, 0);
            if (direction != -new_direction && direction != new_direction)
            {
                direction = new_direction;
                directionChosen = true;
            }
        }
    }

    void Spawn()
    {
        bodyParts.Add(Instantiate(prefabBodyParts[0]).transform);
        for (int i = 0; i < bodyLength-2; i++)
        {
            bodyParts.Add(Instantiate(prefabBodyParts[1]).transform);
        }
        bodyParts.Add(Instantiate(prefabBodyParts[2]).transform);

        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].SetParent(transform);
        }
    }

    void updateLength()
    {

        bodyParts.Insert(bodyParts.Count-1, Instantiate(prefabBodyParts[1], bodyParts[bodyParts.Count-1].transform.position, Quaternion.identity).transform);


        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].SetParent(transform);
        }
      //  updatingLength = false;
    }

    void Move()
    {

        if (updatingLength)
        {
            updateLength();
            updatingLength = false;
        }

        bodyParts[0].rotation = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, -90);
        Vector3 new_position = bodyParts[0].position + direction;
        if (!CheckIfOutOfBounds(new_position) && !CheckIfAutoHit(new_position))
        {
            for (int i = bodyParts.Count-1; i > 0; i--)
            {
                bodyParts[i].position = bodyParts[i-1].position;
            }
            bodyParts[0].position = new_position;

            bodyParts[bodyParts.Count-1].rotation = Quaternion.LookRotation(Vector3.forward,
              bodyParts[bodyParts.Count-2].position - bodyParts[bodyParts.Count-1].position) * Quaternion.Euler(0, 0, -90);
        }
        directionChosen = false;
    }

    bool CheckIfAutoHit(Vector3 head)
    {
        for (int i = 0; i < bodyParts.Count-1; i++) //-1 to avoid considering tail hit since it hasn't been updated yet by now
        {
            if (head == bodyParts[i].position)
            {
                StartCoroutine(ResetSnake());
                return true;
            }
        }
        return false;
    }

    bool CheckIfOutOfBounds(Vector3 head)
    {
      if (head.x < -5 || head.x > 5 || head.y < -5 || head.y > 5)
      {
          StartCoroutine(ResetSnake());
          return true;
      }
      return false;
    }



    IEnumerator ResetSnake()
    {
        isUpdating = true;
        CancelInvoke("Move");
        for (int i = 0; i < bodyParts.Count; i++)
        {
            yield return new WaitForSeconds(0.15f);
            Destroy(bodyParts[i].gameObject);
        }

        bodyParts.Clear();
        GameManagerScript.ResetGame();
        isUpdating = false;
    }
}
