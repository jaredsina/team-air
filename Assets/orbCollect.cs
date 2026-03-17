using UnityEngine;

public class orbCollect : MonoBehaviour

{
    public int orbsCollected = 0;
    
    public GameObject redOrb;
    public GameObject orangeOrb;
    public GameObject yellowOrb;
    public GameObject greenOrb;
    public GameObject brownOrb;
    public GameObject blueOrb;
    public GameObject purpleOrb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frames
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "red")
        {
            Debug.Log("red picked up");
            orbsCollected += 1;
            redOrb.transform.position = new Vector2(7.68f, 4.45f);
        }
        
        Debug.Log(orbsCollected);
       
        

    }
}
