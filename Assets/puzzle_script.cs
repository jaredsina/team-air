using UnityEngine;

public class puzzle_script : MonoBehaviour
{
    private bool isDragging;
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 targetPosition;
    private bool isSnapped = false;

    [SerializeField] private float snapDistance = 0.5f;

    public void SetTargetPosition(Vector3 target)
    {
        targetPosition = target;
    }

    void Start()
    {
        mainCamera = Camera.main;
        isDragging = false;
    }

    void OnMouseDown()
    {
        if (isSnapped) return; // Don't allow dragging once snapped

        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        float distance = Vector3.Distance(transform.position, targetPosition);
        if (distance <= snapDistance)
        {
            transform.position = targetPosition;
            isSnapped = true;
            Debug.Log($"{gameObject.name} snapped into place!");
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
