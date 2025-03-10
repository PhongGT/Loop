using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class R_Helper 
{
    private static Camera mainCamera;

    public static bool CheckRandom(float chance) // Check can element can hit or not
    {
        float randomValue = Random.Range(0, 100);
        if (randomValue < chance)
        {
            return true;
        }
        return false;
    }

    public static Camera GetCamera // Get Main Camera
    {
        get {
            if(mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            return mainCamera;
        }
    }

    private static PointerEventData pointerData;
    private static List<RaycastResult> results;
    public static bool IsPointerOverUI() // Check Pointer/ Mouse is over UI or not
    {
        pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results.Count > 0;
    }
}
