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
using UnityEngine;
using DG.Tweening;
public class Ani
{

    public bool startActive = true;
    public bool endActive = false;
    //是否正向播放
    public bool isForward = true;
    public int count = 1;

    Tweener tween;
    EAnimation type;
    Transform go;
    Vector3 endPosOri;
    float distance;
    float duration;

    public Ani(EAnimation type, Transform go, Vector3 endPosOri, float distance, float duration)
    {
        this.type = type;
        this.go = go;
        this.endPosOri = endPosOri;
        this.distance = distance;
        this.duration = duration;
    }

    /// <summary>
    /// 播放单个动画
    /// </summary>
    /// <param name="endPosOri">向量</param>
    /// <param name="distance">差值</param>
    /// <param name="count">pingpong来回一次是2</param>
    /// <param name="isInsertLast">是否插入 isTogether=false才队列有效</param>
    /// <param name="insertDuration">插入时间</param>
    public void Play()
    {
        tween.OnStart(() => { go.gameObject.SetActive(startActive); });

        switch (type)
        {
            case EAnimation.position:
                tween = DOTween.To(() => go.position, x => go.position = x, go.position + endPosOri * distance, duration)
                .SetLoops(count, LoopType.Yoyo);
                break;
            case EAnimation.rotation:
                tween = DOTween.To(() => go.eulerAngles, x => go.eulerAngles = x, go.eulerAngles + endPosOri * distance, duration)
            .SetLoops(count, LoopType.Yoyo);
                break;
            case EAnimation.scale:
                tween = DOTween.To(() => go.localScale, x => go.localScale = x, endPosOri * distance, duration)
            .SetLoops(count, LoopType.Yoyo);
                break;
        }

        if (!isForward)
        {
            tween.From();
            endActive = !endActive;
        }
    }

    /// <summary>
    /// 获取当前Tween 先Play才获取的到
    /// </summary>
    /// <returns></returns>
    public Tweener GetTween()
    {
        return tween;
    }

    /// <summary>
    /// 事件回调
    /// </summary>
    /// <param name="func"></param>
    /// <param name="isTogether"></param>
    public void OnComplete()
    {
        tween.onComplete+=(() =>
        {
            go.gameObject.SetActive(endActive);
        });
    }
}
