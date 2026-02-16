
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Lights : MonoBehaviour
{
    public int lightIndex;

    private Collider2D objectCollider;
    SpriteRenderer sr;
    Color originalColor;

    void Awake()
    {
        objectCollider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public IEnumerator Flash(float duration)
    {
        sr.color = Color.yellow;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
    }

    public IEnumerator FlashRed(float duration)
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
    }

    void Update()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane)
        );

        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);
        Collider2D hit = Physics2D.OverlapPoint(worldPos2D);

        if (hit != null && hit == objectCollider)
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.RegisterPlayerInput(lightIndex);
        }
    }
}
