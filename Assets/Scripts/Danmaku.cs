using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Danmaku : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void RoundDanmaku(string itemType, Vector3 position, Vector3 direction, float angle)
    {
        float y = direction.y;
        for(float i = 0; Math.Abs(i) < 360; i += angle)
        {
            PoolingSystem.Instance.InstantiateAPS(itemType, position, Quaternion.Euler(0, y + i, 180));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
