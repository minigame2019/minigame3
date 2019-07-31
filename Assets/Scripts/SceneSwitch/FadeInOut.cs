using UnityEngine;
using UnityEngine.UI;//注意添加RawImage命名空间

public class FadeInOut : MonoBehaviour
{
    public bool isBlack = false;
    public float fadeSpeed = 2f;
    public RawImage rawImage;
    public RectTransform rectTransform;

    void Start()
    {
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);//使背景满屏
        rawImage.color = Color.clear;
    }

    void Update()
    {
        if (isBlack == false)
        {
            rawImage.color = Color.Lerp(rawImage.color, Color.clear, Time.deltaTime * fadeSpeed);//渐亮
            if (rawImage.color.a < 0.05f)
            {
                rawImage.color = Color.clear;
            }
        }
        else if (isBlack)
        {
            rawImage.color = Color.Lerp(rawImage.color, Color.black, Time.deltaTime * fadeSpeed);//渐暗
            if (rawImage.color.a > 0.95f)
            {
                rawImage.color = Color.black;
                isBlack = false;
            }
        }
    }

    //切换状态
    public void BackGroundControl(bool b)
    {
        if (b == true)
            isBlack = true;
        else
        {
            rawImage.color = Color.black;
            isBlack = false;
        }
    }
}

