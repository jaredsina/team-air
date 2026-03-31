using UnityEngine;

public class playerMove : MonoBehaviour
{
    public Transform player;
    public Transform cursor;

    public Vector2 target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
      target = player.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = cursor.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, target,1.0f * Time.deltaTime);
    }
}
