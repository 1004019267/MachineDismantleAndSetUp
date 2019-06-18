
using System;
/**
*Copyright(C) 2019 by #COMPANY#
*All rights reserved.
*FileName:     #SCRIPTFULLNAME#
*Author:       #AUTHOR#
*Version:      #VERSION#
*UnityVersion：#UNITYVERSION#
*Date:         #DATE#
*Description:   
*History:
*/
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class TextDic : MonoBehaviour
{
    Dictionary<string, int> dic = new Dictionary<string, int>() {
        {"b",1 },
        {"a",2 },
        {"c",3 },
    };
    // Use this for initialization
    void Start()
    {


        //var dicRe= dic.Reverse();
        //var dicRe = dic.OrderByDescending(o => o.Key);
        //var dicRe = dic.OrderBy(o => o.Key);
        //foreach (var item in dicRe)
        //{
        //    UnityEngine.Debug.Log(item.Key+":"+item.Value);
        //}
        List<int> a = new List<int>() { 3, 1, 2, 4, 5 };

        Stopwatch sw = new Stopwatch();
        sw.Start();
        Dic("as",1 , 2, 4, a);
        //ArrayList z = new ArrayList() {"a",1,a };
        //foreach (var item in z)
        //{
        //    if (item.GetType().ToString().Contains("List"))
        //    {
        //        foreach (var j in (List<int>)item)
        //        {
        //            UnityEngine.Debug.Log(j);

        //        }            
        //    }
        //}
        //foreach (var item in a)
        //{
        //    UnityEngine.Debug.Log(item);
        //}   

        //foreach (var item in )
        //{
        //    UnityEngine.Debug.Log(item);
        //}
        sw.Stop();
        UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
    }

    //public void Dic(params int[] obj)
    //{
    //    foreach (var item in obj)
    //    {
    //        UnityEngine.Debug.Log(item);
    //    }
    //}
   
    public void Value(float val)
    {

    }

    public void Dic(params object[] obj)
    {
        foreach (var item in obj)
        {
            UnityEngine.Debug.Log(item);
        }
    }

}
