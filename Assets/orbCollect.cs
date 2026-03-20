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
    public GameObject lock1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frames
    void Update()
    {
        if (orbsCollected > 7)
        {
            Destroy(lock1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SpriteRenderer sr = collision.gameObject.GetComponent<SpriteRenderer>();
        if (collision.gameObject.name == "red")
        {
            Debug.Log("red picked up");
            orbsCollected += 1;
            redOrb.transform.position = new Vector2(7.68f, 4.45f);
            sr.sortingLayerName = "orbdisplay";
        }
        
        if (collision.gameObject.name == "orange")
        {
            Debug.Log("orange picked up");
            orbsCollected += 1;
            orangeOrb.transform.position = new Vector2(7.68f, 2.95f);
            sr.sortingLayerName = "orbdisplay";
        }

        if (collision.gameObject.name == "yellow")
        {
            Debug.Log("yellow picked up");
            orbsCollected += 1;
            yellowOrb.transform.position = new Vector2(7.68f, 1.45f);
            sr.sortingLayerName = "orbdisplay";
        }
        
        if (collision.gameObject.name == "green")
        {
            Debug.Log("green picked up");
            orbsCollected += 1;
            greenOrb.transform.position = new Vector2(7.68f, -0.05f);
            sr.sortingLayerName = "orbdisplay";
        }
        
        if (collision.gameObject.name == "brown")
        {
            Debug.Log("brown picked up");
            orbsCollected += 1;
            brownOrb.transform.position = new Vector2(7.68f, -1.5f);
            sr.sortingLayerName = "orbdisplay";
        }

        if (collision.gameObject.name == "blue")
        {
            Debug.Log("blue picked up");
            orbsCollected += 1;
            blueOrb.transform.position = new Vector2(7.68f, -3.05f);
            sr.sortingLayerName = "orbdisplay";
        }
        
        if (collision.gameObject.name == "purple")
        {
            Debug.Log("purple picked up");
            orbsCollected += 1;
            purpleOrb.transform.position = new Vector2(7.68f, -4.55f);
            sr.sortingLayerName = "orbdisplay";
        }

        
        Debug.Log(orbsCollected);
       
        

    }
}
