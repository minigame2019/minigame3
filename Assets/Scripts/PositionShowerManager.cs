using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PositionShowerManager : MonoSingleton<PositionShowerManager>
{
    public Font font;
    List<PositionShower> positionShowers = new List<PositionShower>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PositionShower CreatePositionShower(Sprite sprite)
    {
        GameObject obj = Instantiate(new GameObject(),this.transform);
        
        Image image = obj.AddComponent<Image>();
        image.sprite = sprite;

        PositionShower positionShower = obj.AddComponent<PositionShower>();
        this.positionShowers.Add(positionShower);
        positionShower.image = image; 
        
        return positionShower;
    }

    public void RemovePositionShower(PositionShower pos)
    {
        this.positionShowers.Remove(pos);
        pos.transform.parent = null;
        Destroy(pos.gameObject);
    }
}
