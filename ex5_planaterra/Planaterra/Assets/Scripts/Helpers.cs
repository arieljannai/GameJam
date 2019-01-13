using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    
}

namespace ExtensionsMethods
{
    public static class TransformExtensions
    {
        public static List<Transform> GetChildren(this Transform transform)
        {
            List<Transform> list = new List<Transform>(transform.childCount);

            for (int i = 0; i < transform.childCount; i++)
            {
                list.Add(transform.GetChild(i));
            }

            return list;
        }

        public static List<GameObject> GetChildrenGameObjects(this Transform transform)
        {
            List<GameObject> list = new List<GameObject>(transform.childCount);

            for (int i = 0; i < transform.childCount; i++)
            {
                list.Add(transform.GetChild(i).gameObject);
            }

            return list;
        }
    }

    public static class GameObjectExtensions
    {
        public static List<GameObject> GetChildrenGameObjects(this GameObject gameObject)
        {
            List<GameObject> list = new List<GameObject>(gameObject.transform.childCount);

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                list.Add(gameObject.transform.GetChild(i).gameObject);
            }

            return list;
        }

        public static void SetOpacity(this GameObject go, float opacity)
        {
            Color c = go.transform.GetComponent<SpriteRenderer>().color;
            go.transform.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, opacity);
        }

        public static float GetOpacity(this GameObject go)
        {
            return go.transform.GetComponent<SpriteRenderer>().color.a;
        }
    }

    public static class VectorExtensions
    {
        public static Vector3[] ToVector3Array(this Vector2[] v2s)
        {
            Vector3[] v3s = new Vector3[v2s.Length];

            for (int i = 0; i < v2s.Length; i++)
            {
                v3s[i] = v2s[i];
            }

            return v3s;
        }

        public static Vector3[] ToVector3Array(this List<Vector2> v2s)
        {
            Vector3[] v3s = new Vector3[v2s.Count];

            for (int i = 0; i < v2s.Count; i++)
            {
                v3s[i] = v2s[i];
            }

            return v3s;
        }

        public static List<Vector3> ToVector3List(this List<Vector2> v2s)
        {
            List<Vector3> v3s = new List<Vector3>(v2s.Count);

            v2s.ForEach(x => v3s.Add(x));

            return v3s;
        }
    }
}
