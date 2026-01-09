using UnityEngine;
using UnityEngine.InputSystem;
/* --------------------------- NOTE SECTION -------------------------------------------          
https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/Actions.html
*/





public class PlayerMovement : MonoBehaviour
{
    // How fast the player can go

    public float speed = 5f;

    public InputAction move;

    
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

/*    void Update()
    {
        // Get Direction from KEYBOARD
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        
        movement = movement.normalized;
    }
*/
    void FixedUpdate()
    {
        // Move The Player
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}

