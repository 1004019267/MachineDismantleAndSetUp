using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
/// <summary>
/// 每个XML的标识
/// </summary>
public enum EXML
{
    /// <summary>
    /// 拆的XML表
    /// </summary>
    StepXml = 0,
    StepToolsXml,
    StepAniXml,
    StepAniStaXml,
    PartAniListXml,
    PartEquListXml,
    PartTraListXml,
    PartsXml,
}


/// <summary>
/// XML管理类
/// </summary>
/// <typeparam name="I"></typeparam>
/// <typeparam name="T"></typeparam>
public class ConfigManager<I, T> : Singleton<ConfigManager<I, T>>
{
    XMLConfigParser xml = new XMLConfigParser();
    Dictionary<EXML, Dictionary<I, T>> xmlConfig = new Dictionary<EXML, Dictionary<I, T>>();
    Dictionary<int, Dictionary<I, T>> aniConfig = new Dictionary<int, Dictionary<I, T>>();
    int i = 0;
    public void AddXml(EXML type, string tablename,string nodePath, string identify = "")
    {
        Dictionary<I, T> dic = new Dictionary<I, T>();
        dic = xml.LoadConfig<I, T>(tablename, nodePath, identify);
        if (type == EXML.StepAniXml)
        {
            aniConfig.Add(i, dic);
            i++;
            return;
        }
        xmlConfig.Add(type, dic);
    }

    public void RemoveXml(EXML type)
    {
        if (type == EXML.StepAniXml)
        {
            aniConfig.Clear();
            i = 0;
            return;
        }
        xmlConfig.Remove(type);
    }
    /// <summary>
    /// 其他表
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Dictionary<I, T> GetXml(EXML type)
    {
        return xmlConfig[type];
    }
    /// <summary>
    /// 动画表
    /// </summary>
    /// <param name="type"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public Dictionary<I, T> GetAniXml(int index)
    {
        return aniConfig[index];
    }

    public Dictionary<int, Dictionary<I, T>> GetAllAniXml()
    {
        return aniConfig;
    }

    public int GetAniXmlCount()
    {
        return aniConfig.Count;
    }

    public bool isHave(EXML type)
    {
        return xmlConfig.ContainsKey(type);
    }
}
/// <summary>
/// 节点路径方法管理类
/// </summary>
public class NodePathManager : Singleton<NodePathManager>
{
    public string GetDismantleStepPath(string equName, string actionName)
    {
        //WorkShop[@Name='']就是WorkShop里面的Name=''的路径
        return "Content/Equ[@Name='" + equName + "']/action[@Name='" + actionName + "']/Step";
    }

    public string GetDismantleStepToolsPath(string equName, string toolsType)
    {
        return "Content/Equ[@Name='" + equName + "']/tools[@Name='" + toolsType + "']/Tool";
    }

    public string GetDismantleStepOneAnimationPath(string equName, string animationsName, string ID)
    {
        return "Content/Equ[@Name='" + equName + "']/animations[@Name='" + animationsName + "']/Step[@ID='" + ID + "']/ani";
    }

    public string GetDismantleStepAllAnimationStatePath(string equName, string animationsName)
    {
        return "Content/Equ[@Name='" + equName + "']/animations[@Name='" + animationsName + "']/Step";
    }

    public string GetPartXmlListPath(string workName, string proName, string EquName, string listName)
    {
        return "Content/WorkShop[@Name='" + workName + "']/ProductLine[@Name='" + proName + "']/Equipment[@Name='" + EquName + "']/" + listName;
    }

    public string GetZhuangPartXmlPath(string equName, string partsName)
    {
        return "Content/Equ[@Name='" + equName + "']/Parts[@Name='" + partsName + "']/Part";
    }
}

public class EquList
{
    public string ShowName;
    public string Name;
    public int ID;
    public int Type;
    public string Path;
    public string isHaveChild;
    public float Dis;
    public float Min;
    public float Max;
}

public class AniList
{
    public string ShowName;
    public string Name;
    public string Path;
}

public class TraList
{
    public string ShowName;
    public string Name;
    public string Path;
    public string NameC;
    public string NameZ;
}

/// <summary>
/// 数据
/// </summary>
public class DismantleStep
{
    public int ID;
    public string Info;
}

public enum EAnimation
{
    position = 0,
    rotation,
    scale,
}

public class DismantleAniState
{
    public int ID;
    public Tool tool;
    public bool isEndActiveTogether;
    public bool canForward;
}

public class DismantleAni
{
    public int ID;
    public EAnimation EAni;
    public float duration;
    public Vector3 endPosOri;
    public float distance;
    public int count;
    public bool startActive;
    public bool endActive;
    public bool isInsertLast;
    public float insertDuration;
}

public class Parts
{
    public string Step;
    public string Name;
}



#region tool
/// <summary>
/// 工具类
/// </summary>
public enum Tool
{
    /// <summary>
    /// 手
    /// </summary>
    hind = 0,
    /// <summary>
    /// 扳手
    /// </summary>
    banshou,
    /// <summary>
    /// 内六角扳手
    /// </summary>
    neiliujiao,
    /// <summary>
    /// 锤子
    /// </summary>
    chuizi,
    /// <summary>
    /// 钳子
    /// </summary>
    jianzuiqian,
    /// <summary>
    /// 卡簧钳
    /// </summary>
    kahuangqian,
    /// <summary>
    /// 定位杆
    /// </summary>
    dingweigan
}
/// <summary>
/// 拆装工具
/// </summary>
public class DismantleTools
{
    public Tool id;
    public string name;
}
#endregion

