using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testa : MonoBehaviour
{
    SpriteRenderer spriteRender;
    private void Awake()
    {
        Sprite sprite = Resources.Load<Sprite>("Sprites/arrow");
        transform.localScale = new Vector2(100, 100);
        spriteRender = this.gameObject.AddComponent<SpriteRenderer>();
        spriteRender.sprite = sprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
