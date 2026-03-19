using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Lights : MonoBehaviour
{
    public int lightIndex;

    private CircleCollider2D circleCollider;
    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public IEnumerator Flash(float duration)
    {
        Color flashColor = new Color(0.56f, 1f, 0.54f);
        float t = 0f;

        while (t < duration)
        {
            sr.color = Color.Lerp(originalColor, flashColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;
        while (t < duration)
        {
            sr.color = Color.Lerp(flashColor, originalColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

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
            new Vector3(
                mouseScreenPos.x,
                mouseScreenPos.y,
                Mathf.Abs(Camera.main.transform.position.z)
            )
        );

        Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

        if (circleCollider != null && circleCollider.OverlapPoint(worldPos2D))
        {
            GameManager gm = FindFirstObjectByType<GameManager>();
            if (gm != null)
                gm.RegisterPlayerInput(lightIndex);
        }
    }
}