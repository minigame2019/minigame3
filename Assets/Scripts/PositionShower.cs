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
    Text t;

    private void Awake()
    {
        player = FindObjectOfType<PlayerCharacter>().gameObject;
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
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }

        playerPos = player.GetComponent<Transform>().position;
        targetPos = target.GetComponent<Transform>().position;

        t.text = "text";
        t.font = PositionShowerManager.Instance.font;

        
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        Vector3 direction = (targetPos - playerPos);
        direction = ToEdge(direction);
        rectTransform.sizeDelta = new Vector2(14, 16);
        
        rectTransform.localPosition = direction;
        
    }

    Vector3 ToEdge(Vector3 input)
    {
        input.y = 0;
        Vector3 output = input.normalized * 50;
        output = new Vector3(output.x, output.z, output.y);
        return output;

    }

    bool IsOutside()
    {
        return false;
    }
}
