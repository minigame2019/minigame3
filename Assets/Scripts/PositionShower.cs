using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PositionShower : MonoBehaviour
{
    public Vector3 playerPos;
    public Vector3 targetPos;
    public GameObject player;
    public GameObject target;
    Sprite sprite;
    // Start is called before the first frame update
    GameObject shower;
    RectTransform rectTransform;
    Text t;
    private void Awake()
    {

        player = FindObjectOfType<PlayerCharacter>().gameObject;
        //shower = Instantiate(Resources.Load("Prefabs/Non-import/Triangle") as GameObject,this.transform);
       // rectTransform = this.gameObject.AddComponent<RectTransform>();
        t = this.gameObject.AddComponent<Text>();

    }

    void Start()
    {
        
    }
    bool flag = false;
    // Update is called once per frame
    void Update()
    {
        if (this.target == null || this.player == null)
        {
            Destroy(this.shower);
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }
        if (!IsOutside())
        {
            t.enabled = false;
            return;
        }
        else
        {
            t.enabled = true;
        }
        rectTransform = this.transform.GetComponent<RectTransform>();
        playerPos = player.GetComponent<Transform>().position;
        targetPos = target.GetComponent<Transform>().position;
        t.text = "t";
        t.font = PositionShowerManager.Instance.font;

        Vector3 direction = (targetPos - playerPos);
        direction = ToEdge(direction);

        rectTransform.localPosition = direction;
        rectTransform.sizeDelta = new Vector2(20, 20);
        
    }
    
    Vector3 ToEdge(Vector3 input)
    {
        float width = (float)Screen.width;
        float height = (float)Screen.height;
        Debug.Log(height / width);
        input.y = 0;
        Vector3 output = new Vector3(input.x, input.z,0);
        output.Normalize();
        Debug.Log(output.x);

        Debug.Log(output.y);


        if (Mathf.Abs(output.x) < 0.01f)
        {
            output = output * height / 2 / Mathf.Abs(output.y);
        }
        else
        {
            if (Mathf.Abs(output.y) < 0.01f)
            {
                output = output * width / 2 / Mathf.Abs(output.x);
            }
            else
            {
                if (Mathf.Abs(output.y / output.x) > height / width)
                {
                    output = output * height / 2 / Mathf.Abs(output.y);
                }
                else
                {
                    output = output * width / 2 / Mathf.Abs(output.x);
                }
            }
        }
        Debug.Log(output);
        float l = 20;
        output = (output.magnitude - l) * output.normalized;
        return output;
        
    }

    bool IsOutside()
    {
        var position = Camera.main.WorldToViewportPoint(target.transform.position);
        if (position.x > 0 && position.x < 1 && position.y > 0 && position.y < 1)
            return false;
        else
            return true;
    }
}
