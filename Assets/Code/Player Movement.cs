using UnityEngine;
using UnityEngine.InputSystem;
/* --------------------------- NOTE SECTION -------------------------------------------          
https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/Actions.html
*/





public class PlayerMovement : MonoBehaviour
{
    // How fast the player can go

    public float speed = 5f;
    private float direction = 0f;
    public InputAction move;

    
    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        move.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
    }

    void Update()
    {
        movement = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
    rb.linearVelocity = new Vector2(movement.x * speed, movement.y * speed );
    }
}



