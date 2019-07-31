using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IDamageable<Transform>
{
    public int Health = 5;
    private Material[] displayMaterials = new Material[0];
    private Color[] displayColors = new Color[0];
    private WaitForSeconds recoilTime = new WaitForSeconds(0.04f);

    private void Start()
    {
        Renderer[] componentsInChildren = base.GetComponentsInChildren<Renderer>();
        this.displayMaterials = new Material[componentsInChildren.Length];
        this.displayColors = new Color[componentsInChildren.Length];
        for (int i = 0; i < this.displayMaterials.Length; i++)
        {
            this.displayMaterials[i] = componentsInChildren[i].material;
            this.displayColors[i] = this.displayMaterials[i].color;
        }
    }

    public void TakeDamage(Transform hitObject)
    {
        this.Health--;
        if (this.Health > 0)
        {
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Hit, -1f);
            //base.StartCoroutine(this.HitColorChange());
        }
        else
        {
            CameraController.Shake(0.1f);
            GameManager.Instance.PlayAudio(base.transform.position, GameManager.Instance.GameSounds.Explode, -1f);
            GameManager.Instance.Switch();
            this.Health = 5;
            Debug.Log("Triggered");
        }
    }

    void OnBecameVisible()
    {
        Debug.Log(GameManager.Instance.IsSwitchShowed);
        if (GameManager.Instance.IsSwitchShowed == false)
        {
            StartCoroutine("ShowMessage");
            GameManager.Instance.IsSwitchShowed = true;
        }
    }

    private IEnumerator ShowMessage()
    {

        GameManager.Instance.GameMenus.MessagePanel.SetActive(true);
        GameManager.Instance.GameMenus.MessagePanel.transform.Find("SwitchHelMessage").gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
    }
}