using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Danmaku
{

    // 
    public void RoundDanmaku(string itemType, Vector3 position, Vector3 direction, float angle)
    {
        float y = direction.y;
        for(float i = 0; Math.Abs(i) < 360; i += angle)
        {
            PoolingSystem.Instance.InstantiateAPS(itemType, position, Quaternion.Euler(0, y + i, 180));
        }
    }

    public void GrapeshotDanmaku(string itemType, Vector3 position, Vector3 direction, float angle, int num)
    {
        float y = direction.y;
        PoolingSystem.Instance.InstantiateAPS(itemType, position, Quaternion.Euler(0, y, 180));
        for(int i = 1; i <= num; i++)
        {
            PoolingSystem.Instance.InstantiateAPS(itemType, position, Quaternion.Euler(0, y + i * angle, 180));
            PoolingSystem.Instance.InstantiateAPS(itemType, position, Quaternion.Euler(0, y - i * angle, 180));


        }
    }
}
