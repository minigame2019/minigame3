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
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(Input.mousePosition);
        }
        
    }

    public PositionShower CreatePositionShower()
    {
        GameObject obj = Instantiate(new GameObject());
        obj.transform.parent = this.transform;
        PositionShower positionShower = obj.AddComponent<PositionShower>();
        this.positionShowers.Add(positionShower);
        return positionShower;
    }

    public void RemovePositionShower(PositionShower pos)
    {
        this.positionShowers.Remove(pos);
        pos.transform.parent = null;
        Destroy(pos.gameObject);
    }
}
