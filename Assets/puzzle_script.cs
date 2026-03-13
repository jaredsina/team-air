using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//detect if mouse cursor is touching shape
//detect if mouse is clicked
//change shape position to mouse position while clicked 
//detect when mouse cursor is lifted
public class puzzle_script : MonoBehaviour
{
    private bool isDraggable;

    private bool isDragging;

    private Collider2D objectCollider;
    
    static puzzle_script currentlyDragging;

    private start sx;
    void Start()
    {
        objectCollider = GetComponent<Collider2D>();
        isDraggable = false;
        isDragging = false;
        sx = new start();
        sx.init();
        
    }

    // Update is called once per frame 
    

    void dragAndDrop()
    {
        Vector3 mousePosition;
        mousePosition = Mouse.current.position.ReadValue();
        Vector3 worldPos3 = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        worldPos3.z = transform.position.z;
        Vector2 worldPos2 = new Vector2(worldPos3.x, worldPos3.y);
        Collider2D hit = Physics2D.OverlapPoint(worldPos2);
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentlyDragging != null)
            {
                return;
            }
            if (hit != null && objectCollider == hit)
            {
                isDraggable = true;
                isDragging = true;
                currentlyDragging = this;
            }
        }
        
        if (isDragging && currentlyDragging == this)
        {
            objectCollider.transform.position = worldPos3;
            
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (currentlyDragging == this)
            {
                currentlyDragging = null;
            }
            isDraggable = false;
            isDragging = false;
        }
}
    void Update()
    {
        dragAndDrop();
    }
}
