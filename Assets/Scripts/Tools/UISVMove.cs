using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScrowView 点按钮移动显示
/// </summary>
public class UISVMove
{

    int times = 0;
    float maxTimes;
    int moveLong;
    Transform content;
    float btnSkipH;

    public UISVMove(Transform content, int moveLong)
    {
        this.content = content;
        this.moveLong = moveLong;
        //向上取整
        maxTimes = Mathf.Ceil((float)content.childCount / moveLong) - 1;
        btnSkipH = content.GetChild(0).GetComponent<RectTransform>().rect.height;
    }

    public void Up()
    {
        if (times <= 0)
            return;
        content.GetComponent<RectTransform>().position -= Vector3.up * btnSkipH * moveLong;
        times--;
    }

    public void Down()
    {
        if (times >= maxTimes)
            return;
        content.GetComponent<RectTransform>().position += Vector3.up * btnSkipH * moveLong;
        times++;
    }

    public void ClearTimes()
    {
        times = 0;
    }
}

