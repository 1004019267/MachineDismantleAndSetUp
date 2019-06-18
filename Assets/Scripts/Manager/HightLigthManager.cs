using UnityEngine;
using System.Collections;
using HighlightingSystem;//引入高亮命名空间
using System.Collections.Generic;
using System.Linq;
public class HightLigthManager :Singleton<HightLigthManager>
{
    List<GameObject> gos=new List<GameObject>(); 
    /// <summary>
    /// 设置颜色 闪烁频率
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isLight"></param>
    public void SetLight(GameObject go, Color flashingStartColor, Color flashingEndColor, float flashingFrequency)
    {
        Highlighter h = go.GetComponent<Highlighter>();
        if (h == null)
        {
            h = go.AddComponent<Highlighter>();
        }
        h.FlashingOn(flashingStartColor, flashingEndColor, flashingFrequency);
    }
    /// <summary>
    /// 设置是否发光
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isLight"></param>
    public void SetLightActive(GameObject go, bool isLight)
    {
        Highlighter h = go.GetComponent<Highlighter>();
        if (isLight)
        {
            h.FlashingOn();
        }
        else
        {
            h.FlashingOff();
        }
    }
    /// <summary>
    /// 移除发光脚本
    /// </summary>
    /// <param name="go"></param>
    public void RemoveHight(GameObject go)
    {
        if (gos.Contains(go))
        {
          GameObject.Destroy(go.GetComponent<Highlighter>());
        }
    }
}
