using UnityEngine;
using System.Collections.Generic;

public class start : MonoBehaviour
{
    public Dictionary<string, (float x, float y)> dictionary = new Dictionary<string, (float x, float y)>();
    private SpriteRenderer[] renderers;

    void Start()
    {
        renderers = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
        LogAllSpritePositions();
        AssignTargets();
        RandomSpot();
    }

    void LogAllSpritePositions()
    {
        foreach (SpriteRenderer r in renderers)
        {
            if (r.name != "background")
            {
                Vector3 center = r.bounds.center;
                dictionary.Add(r.name, (center.x, center.y));
                r.enabled = false;
            }
        }
    }

    void AssignTargets()
    {
        foreach (SpriteRenderer r in renderers)
        {
            if (r.name != "background" && dictionary.ContainsKey(r.name))
            {
                puzzle_script ps = r.GetComponent<puzzle_script>();
                if (ps != null)
                {
                    var pos = dictionary[r.name];
                    ps.SetTargetPosition(new Vector3(pos.x, pos.y, 0));
                }
            }
        }
    }

    void RandomSpot()
    {
        foreach (SpriteRenderer r in renderers)
        {
            if (r.name != "background")
            {
                float randomX = Random.Range(-7f, 7f);
                float randomY = Random.Range(-5f, 5f);
                r.transform.position = new Vector3(randomX, randomY, 0);
                r.enabled = true;
            }
        }
    }
}
