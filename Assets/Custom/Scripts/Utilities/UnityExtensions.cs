using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;

public static class UnityExtensions
{

    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    public static List<T> GetInterfaces<T>(this GameObject objectToSearch) where T : class
    {
        List<T> resultList = new List<T>();
        resultList = GetInterfacesInChildren<T>(objectToSearch);
        resultList.AddRange(GetInterfacesInParent<T>(objectToSearch));
        return resultList;
    }

    public static List<T> GetInterfacesInParent<T>(this GameObject objectToSearch) where T : class
    {
        return GetInterfacesInObjects<T>(objectToSearch.GetComponentsInParent<MonoBehaviour>());
    }

    public static List<T> GetInterfacesInChildren<T>(this GameObject objectToSearch) where T : class
    {
        return GetInterfacesInObjects<T>(objectToSearch.GetComponentsInChildren<MonoBehaviour>());
    }

    private static List<T> GetInterfacesInObjects<T>(MonoBehaviour[] list) where T : class
    {
        List<T> resultList = new List<T>();
        foreach (MonoBehaviour mb in list)
        {
            if (mb is T)
            {
                //found one
                resultList.Add((T)((System.Object)mb));
            }
        }
        return resultList;
    }


    public static bool ContainsOfType<T>(this List<T> list, object objectTypeToSearch) where T : class
    {
        Type typeToSearch = objectTypeToSearch.GetType();
        return list.Where(x => x.GetType() == typeToSearch).Select(z => z).Any();
    }

    public static T GetOfType<T>(this List<T> list, object objectTypeToSearch) where T : class
    {
        Type typeToSearch = objectTypeToSearch.GetType();
        return list.Where(x => x.GetType() == typeToSearch).Select(z => z).FirstOrDefault();
    }
}