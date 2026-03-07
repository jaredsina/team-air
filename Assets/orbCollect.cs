using UnityEngine;

public class orbCollect : MonoBehaviour
{
    public int orbsCollected = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        orbsCollected += 1;
        Debug.Log(orbsCollected);
        Destroy(gameObject);
    }
}
