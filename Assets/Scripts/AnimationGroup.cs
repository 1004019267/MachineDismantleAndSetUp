
using DG.Tweening;
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

public class AnimationGroup
{

    public bool isEndActiveTogether = false;
    public bool endActive = false;

    /// <summary>
    /// Dotween队列
    /// </summary>
    Sequence sequence;

    Transform stepParent;
    bool isForward = true;

    public AnimationGroup(Transform stepParent, bool isForward, bool setAutoKill = false)
    {
        //不杀死动画队列
        sequence = DOTween.Sequence().SetAutoKill(setAutoKill);
        this.stepParent = stepParent;
        this.isForward = isForward;
    }

    public void AddGroup(Ani ani, bool isInsertLast, float insertDuration)
    {

        ani.Play();

        if (!isEndActiveTogether)
        {
            ani.OnComplete();
        }

        if (isInsertLast)
        {
            sequence.Insert(insertDuration, ani.GetTween());
        }
        else
        {
            sequence.Append(ani.GetTween());
        }
    }
    /// <summary>
    /// 播放一组动画根据表
    /// </summary>
    /// <param name="aniConfig"></param>
    /// <param name="StepParent"></param>
    /// <param name="isTogether"></param>
    public void Play()
    {

        if (isEndActiveTogether)
        {
            sequence.onComplete += (
                () =>stepParent.gameObject.SetActive(endActive));
        }

        //回归初始位置
        if (!isForward)
        {
            sequence.onComplete += (() => { sequence.PlayForward(); });
        }
        else
        {
            sequence.onComplete += (() => { sequence.Rewind(); });
        }
    }

    public Sequence GetSequence()
    {
        return sequence;
    }

    /// <summary>
    /// 事件回调
    /// </summary>
    /// <param name="func"></param>
    /// <param name="isTogether"></param>
    public void OnComplete(TweenCallback func)
    {
        //分批播放完毕回调
        sequence.onComplete += func;
    }
}
