using System;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Texture2D Input;
    public GameObject RegularWall;
    public GameObject KillWall;
    public GameObject ObstacleWall;
    public GameObject PlayerObject;

    public void GenerateLevelFromInput()
    {
        GameObject obj2 = new GameObject("LevelContainer");
        int num = this.Input.width / 2;
        int num2 = this.Input.height / 2;
        int x = 0;
        while (x < this.Input.width)
        {
            int y = 0;
            while (true)
            {
                if (y >= this.Input.height)
                {
                    x++;
                    break;
                }
                Color pixel = this.Input.GetPixel(x, y);
                if (pixel == Color.white)
                {
                    Instantiate<GameObject>(this.RegularWall, new Vector3((float)(x - num), 0f, (float)(y - num2)), Quaternion.identity).transform.SetParent(obj2.transform);
                }
                else if (pixel == Color.red)
                {
                    Instantiate<GameObject>(this.KillWall, new Vector3((float)(x - num), 0f, (float)(y - num2)), Quaternion.identity).transform.SetParent(obj2.transform);
                }
                else if (pixel == Color.green)
                {
                    Instantiate<GameObject>(this.ObstacleWall, new Vector3((float)(x - num), 0f, (float)(y - num2)), Quaternion.identity).transform.SetParent(obj2.transform);
                }
                else if (pixel == Color.blue)
                {
                    Instantiate<GameObject>(this.PlayerObject, new Vector3((float)(x - num), 0f, (float)(y - num2)), Quaternion.identity).transform.SetParent(obj2.transform);
                }
                y++;
            }
        }
    }

    private void Start()
    {
        if (this.Input)
        {
            this.GenerateLevelFromInput();
        }
    }
}
