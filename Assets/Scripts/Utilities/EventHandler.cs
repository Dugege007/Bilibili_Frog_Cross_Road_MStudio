using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{
    public static event Action<int> GetPointEvent;

    //∫ÙΩ–
    public static void CallGetPointEvent(int point)
    {
        //if (GetPointEvent != null)
        //{
        //    GetPointEvent.Invoke(point);    //∆Ù∂Ø
        //}

        GetPointEvent?.Invoke(point);   //ºÚ–¥
    }

    public static event Action GameOverEvent;
    public static void CallGameOverEvent()
    {
        GameOverEvent?.Invoke();
    }
}
