using UnityEngine;
using System.Collections.Generic;

public class PolygonMetaTagger : MonoBehaviour
{
    public Dictionary<string, List<Vector2>> taggedPolygons = new Dictionary<string, List<Vector2>>();

    public void TagPolygon(string tag, List<Vector2> points)
    {
        if (!taggedPolygons.ContainsKey(tag))
        {
            taggedPolygons.Add(tag, new List<Vector2>(points));
        }
        else
        {
            taggedPolygons[tag] = new List<Vector2>(points);
        }

        Debug.Log("🏷️ Polygon tagué : " + tag);
    }

    public List<Vector2> GetPolygon(string tag)
    {
        if (taggedPolygons.ContainsKey(tag))
        {
            return taggedPolygons[tag];
        }
        return null;
    }

    public void ListTags()
    {
        foreach (KeyValuePair<string, List<Vector2>> entry in taggedPolygons)
        {
            Debug.Log("🔖 Tag présent : " + entry.Key);
        }
    }
}
