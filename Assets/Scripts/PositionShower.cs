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
    public Image image;
    // Start is called before the first frame update
    RectTransform rectTransform;
    public void SetColor(Color color)
    {
        image.color = color;
    }
    private void Awake()
    {       
        player = FindObjectOfType<PlayerCharacter>().gameObject;
        rectTransform = this.transform.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector2((float)0.5, (float)0.5);
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (this.target == null || this.player == null)
        {
            Destroy(this.image);
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }
        if (!IsOutside())
        {
            image.enabled = false;
            return;
        }
        else
        {
            image.enabled = true;
        }
        
        playerPos = player.GetComponent<Transform>().position;
        targetPos = target.GetComponent<Transform>().position;

        Vector3 direction = (targetPos - playerPos);
        direction = ToEdge(direction);
        rectTransform.localPosition = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        rectTransform.localRotation = Quaternion.AngleAxis(angle,Vector3.forward);
    }
    
    Vector3 ToEdge(Vector3 input)
    {
        float width = (float)Screen.width;
        float height = (float)Screen.height;
        input.y = 0;
        Vector3 output = new Vector3(input.x, input.z,0);
        output.Normalize();

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
