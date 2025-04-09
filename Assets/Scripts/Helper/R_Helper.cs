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

    public static List<T> shuffle<T> (List<T> list) // Shuffle List
    {
        int lastIndex = list.Count - 1;
       
        while (lastIndex > 0)
        {
            T temp = list[lastIndex];
            int randomIndex = Random.Range(0, lastIndex);
            list[lastIndex] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    } 
    public static Vector3 PositionBottomLeftCamera()
    {
        Vector3 bottomLeft = GetCamera.ScreenToWorldPoint(new Vector3(0, 0, GetCamera.nearClipPlane));
        return bottomLeft;
    }
    
   
}
