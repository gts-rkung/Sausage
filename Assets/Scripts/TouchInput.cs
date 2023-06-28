using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class TouchInput
{
    // This function should be called in Update()
    public static bool GetTap(bool avoidUi = true)
    {
        if (Input.touchSupported) // touch
        {
            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (avoidUi && !EventSystem.current.IsPointerOverGameObject(0)) // mobile must specify id
                    {
                        //print("tapped");
                        return true;
                    }
                }
            }
        }
        else // mouse
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!avoidUi)
                {
                    //print("<color=yellow>tapped</color> " + Time.time + " " + Input.mousePosition);
                    return true;
                }
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    //print("<color=white>ignore tapped on UI</color> " + Time.time + " " + Input.mousePosition);
                }
                else
                {
                    //print("<color=yellow>tapped</color> " + Time.time + " " + Input.mousePosition);
                    return true;
                }
            }
        }
        return false;
    }

    public static bool GetTouched(bool avoidUi = true)
    {
        if (Input.touchSupported) // touch
        {
            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Stationary ||
                    touch.phase == TouchPhase.Moved)
                {
                    if (avoidUi && !EventSystem.current.IsPointerOverGameObject(0)) // mobile must specify id
                    {
                        //print("touched");
                        return true;
                    }
                }
            }
        }
        else // mouse
        {
            if (Input.GetMouseButton(0))
            {
                if (!avoidUi)
                {
                    //print("<color=yellow>touched</color> " + Time.time + " " + Input.mousePosition);
                    return true;
                }
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    //print("<color=white>ignore touched on UI</color> " + Time.time + " " + Input.mousePosition);
                }
                else
                {
                    //print("<color=yellow>touched</color> " + Time.time + " " + Input.mousePosition);
                    return true;
                }
            }
        }
        return false;
    }
}
