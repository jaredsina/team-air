using UnityEngine;
using System.Collections.Generic;
public class start
{
    public Dictionary<string, (float x, float y)> dictionary = new Dictionary<string, (float x, float y)>();

    private GameObject background;
    private GameObject mainbackground;
    public void init()
    {
        background = GameObject.Find("background");
        mainbackground = GameObject.Find("Canvas");
        LogAllSpritePositions();
        random_spot();
        //Debug.Log(dictionary);
    }

    void LogAllSpritePositions()
    {
        SpriteRenderer[]  renderers = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);
        foreach (SpriteRenderer r in renderers)
        {
            if (r.name != "background")
            {
                //GameObject go = r.gameObject;
                //Vector3 worldpos = go.transform.position;
                //Vector3 screenpos = Camera.main.WorldToScreenPoint(worldpos);
                Vector3 centerposworld = r.bounds.center;
                Vector3 centerposscreen = Camera.main.ScreenToWorldPoint(centerposworld);
                dictionary.Add(r.name, (centerposscreen.x, centerposscreen.y));
                r.enabled = false;
            }
           
            
        }
    }

    void random_spot()
    {
        SpriteRenderer bgbox = background.GetComponent<SpriteRenderer>();
        SpriteRenderer backgroundrenderer = mainbackground.GetComponent<SpriteRenderer>();
        BoxCollider2D boxcollider = bgbox.GetComponent<BoxCollider2D>();
        GameObject[] go = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject g in go)
        {
            if (g.name != "background" && g.name != "mainbackground" && g.name != "Main Camera" && g.name != "Canvas" && g.name != "Image" && g.name != "EventSystem" && g.name != "puzzle_script" && g.name != "[Debug Updater]")
            {
                BoxCollider2D bbox =g.GetComponent<BoxCollider2D>();
                float randomx = Random.Range(-2* backgroundrenderer.bounds.min.x, 2* backgroundrenderer.bounds.max.x);
                float randomy = Random.Range(-2* backgroundrenderer.bounds.min.y, 2*backgroundrenderer.bounds.max.y);
                g.transform.position = new Vector3(randomx, randomy, 0);
                
                /*
                while (boxcollider.bounds.Intersects(bbox.bounds))
                {
                    randomx = Random.Range(backgroundrenderer.bounds.min.x, backgroundrenderer.bounds.max.x);
                    randomy = Random.Range(backgroundrenderer.bounds.min.y, backgroundrenderer.bounds.max.y);
                    g.transform.position = new Vector3(randomx, randomy, 0);
                }*/
                SpriteRenderer renderer = g.GetComponent<SpriteRenderer>();
                renderer.enabled = true;
            }

        }
    }
    
}