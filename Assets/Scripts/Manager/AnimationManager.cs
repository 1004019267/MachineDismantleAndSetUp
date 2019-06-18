using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System.Linq;


public class AnimationManager : Singleton<AnimationManager>
{
    public TweenCallback func;
    /// <summary>
    /// Dotween队列
    /// </summary>
    Sequence mySequence;

    AnimationGroup group;

    public void PlayAniGroup(Dictionary<int, DismantleAni> aniConfig, Transform stepParent, bool isEndActiveTogether, bool canForward = true, bool isForward = true)
    {
        if (!canForward)
        {
            isForward = true;
        }

        group = new AnimationGroup(stepParent, isForward);
        group.isEndActiveTogether = isEndActiveTogether;
        group.endActive = aniConfig[0].endActive;

        Ani ani;

        foreach (var item in isForward == false ? aniConfig.Reverse() : aniConfig)
        {
            ani = new Ani(item.Value.EAni, stepParent.GetChild(item.Value.ID), item.Value.endPosOri,
               item.Value.distance, item.Value.duration);
            ani.startActive = item.Value.startActive;
            ani.endActive = item.Value.endActive;
            ani.count = item.Value.count;
            ani.isForward = isForward;

            group.AddGroup(ani, item.Value.isInsertLast, item.Value.insertDuration);
        }

        if (func != null)
        {
            group.OnComplete(func);
        }

        group.Play();
    }
}

