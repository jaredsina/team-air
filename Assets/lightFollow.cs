using UnityEngine;

public class lightFollow : MonoBehaviour
{
    public Transform lightFollowing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = lightFollowing.position;
    }
}
