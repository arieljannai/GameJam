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
            return gameObject.GetChildrenGameObjects(gameObject.transform.childCount);
        }

        public static List<GameObject> GetChildrenGameObjects(this GameObject gameObject, int objectsLimit)
        {
            List<GameObject> list = new List<GameObject>(gameObject.transform.childCount);

            for (int i = 0; i < objectsLimit; i++)
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

        public static Animator Animator(this GameObject go)
        {
            return go.GetComponent<Animator>();
        }

        public static Object Script(this GameObject go, System.Type className)
        {
            return go.GetComponent(className);
        }
    }

    public static class AnimatorExtensions
    {
        public static void SetState(this Animator animator, int value)
        {
            animator.SetInteger("State", value);
        }

        public static int GetState(this Animator animator)
        {
            return animator.GetInteger("State");
        }

        public static bool IsPlaying(this Animator animator)
        {
            
            return animator.GetCurrentAnimatorStateInfo(0).length > animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public static bool IsPlaying(this Animator animator, string stateName)
        {
            return animator.IsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        public static bool Done(this Animator animator)
        {
            return !animator.IsPlaying();
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
                v3s[i] = new Vector3(v2s[i].x, v2s[i].y, 0);
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

    public static class ArrayExtensions
    {
        private static readonly string DEFAULT_PREFIX = "";
        private static readonly string DEFAULT_SUFFIX = "";
        private static readonly string DEFAULT_SEPARATOR = ", ";

        public static string ToPrintableString<T>(this T[] objs)
        {
            return objs.ToPrintableString(DEFAULT_PREFIX, DEFAULT_SUFFIX, DEFAULT_SEPARATOR);
        }

        public static string ToPrintableString<T>(this T[] objs, string separator)
        {
            return objs.ToPrintableString(DEFAULT_PREFIX, DEFAULT_SUFFIX, separator);
        }

        public static string ToPrintableString<T>(this T[] objs, string prefix, string suffix)
        {
            return objs.ToPrintableString(prefix, suffix, DEFAULT_SEPARATOR);
        }

        public static string ToPrintableString<T>(this T[] objs, string prefix, string suffix, string separator)
        {
            string str = prefix;
            for (int i = 0; i < objs.Length - 1; i++)
            {
                str += objs[i].ToString() + separator;
            }

            str += objs[objs.Length - 1].ToString() + suffix;
            return str;
        }
    }
}
