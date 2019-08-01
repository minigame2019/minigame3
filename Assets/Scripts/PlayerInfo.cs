using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public GameObject player;
    public void Renew()
    {
        player = GameObject.Find("/LevelContainer/Hero(Clone)");
        if (player)
        {
            this.transform.Find("Health").gameObject.GetComponent<Text>().text = "Health:" + player.GetComponent<PlayerCharacter>().Stats.Health;
        }
        else
        {
            this.transform.Find("Health").gameObject.GetComponent<Text>().text = "not found";
        }
    }
}
