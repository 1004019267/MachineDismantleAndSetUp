using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
/// <summary>
/// 加载index
/// </summary>
public enum ELoad
{
    Tools = 0,

}
/// <summary>
/// 加载管理类
/// </summary>
/// <typeparam name="T"></typeparam>
public class ResourcesManager<T> :Singleton<ResourcesManager<T>> where T : UnityEngine.Object
{
    Dictionary<ELoad, T[]> resData = new Dictionary<ELoad, T[]>();

    public void ResLoadAll(ELoad type, string path)
    {
        T[] tex = Resources.LoadAll<T>(path);
        if (tex.Length==0)
        {
            throw new Exception("加载资源不存在");
        }
        //for (int i = 0; i < tex.Length; i++)
        //{
        //    //转换类型
        //     (T)Convert.ChangeType(tex[i], typeof(T))
        //    texL.Add(tex[i]);
        //}
        resData.Add(type, tex);
    }

    public T[] ResGetAll(ELoad type)
    {
        return resData[type];
    }
}
