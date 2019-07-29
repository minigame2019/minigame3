using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("AdvancedPoolingSystem/PoolingSystem")]
public sealed class PoolingSystem : MonoBehaviour
{
    public static PoolingSystem Instance;
    public PoolingItems[] poolingItems;
    public List<GameObject>[] pooledItems;
    public int defaultPoolAmount = 10;
    public bool poolExpand = true;

    private void Awake()
    {
        Instance = this;
    }

    public static void DestroyAPS(GameObject myObject)
    {
        myObject.transform.SetParent(null);
        myObject.SetActive(false);
    }

    public GameObject GetPooledItem(string itemType)
    {
        int index = 0;
        while (true)
        {
            if (index < this.poolingItems.Length)
            {
                if (this.poolingItems[index].prefab.name != itemType)
                {
                    index++;
                    continue;
                }
                int num2 = 0;
                while (true)
                {
                    if (num2 >= this.pooledItems[index].Count)
                    {
                        if (!this.poolExpand)
                        {
                            break;
                        }
                        GameObject item = Instantiate<GameObject>(this.poolingItems[index].prefab);
                        item.SetActive(false);
                        this.pooledItems[index].Add(item);
                        item.transform.SetParent(base.transform);
                        return item;
                    }
                    if (!this.pooledItems[index][num2].activeInHierarchy)
                    {
                        return this.pooledItems[index][num2];
                    }
                    num2++;
                }
            }
            return null;
        }
    }

    public void InitializePool()
    {
        this.pooledItems = new List<GameObject>[this.poolingItems.Length];
        int index = 0;
        while (index < this.poolingItems.Length)
        {
            this.pooledItems[index] = new List<GameObject>();
            int num2 = (this.poolingItems[index].amount <= 0) ? this.defaultPoolAmount : this.poolingItems[index].amount;
            int num3 = 0;
            while (true)
            {
                if (num3 >= num2)
                {
                    index++;
                    break;
                }
                GameObject item = Instantiate<GameObject>(this.poolingItems[index].prefab);
                item.SetActive(false);
                this.pooledItems[index].Add(item);
                num3++;
            }
        }
    }

    public GameObject InstantiateAPS(string itemType)
    {
        GameObject pooledItem = this.GetPooledItem(itemType);
        if (!pooledItem)
        {
            return null;
        }
        pooledItem.SetActive(true);
        return pooledItem;
    }

    public GameObject InstantiateAPS(string itemType, Vector3 itemPosition, Quaternion itemRotation)
    {
        GameObject pooledItem = this.GetPooledItem(itemType);
        pooledItem.transform.position = itemPosition;
        pooledItem.transform.rotation = itemRotation;
        pooledItem.SetActive(true);
        return pooledItem;
    }

    public GameObject InstantiateAPS(string itemType, Vector3 itemPosition, Quaternion itemRotation, GameObject myParent)
    {
        if (this.GetPooledItem(itemType) == null)
        {
            return null;
        }
        GameObject pooledItem = this.GetPooledItem(itemType);
        pooledItem.transform.position = itemPosition;
        pooledItem.transform.rotation = itemRotation;
        pooledItem.transform.SetParent(myParent.transform);
        pooledItem.SetActive(true);
        return pooledItem;
    }

    public static void PlayEffect(GameObject particleEffect, int particlesAmount)
    {
        if (particleEffect.GetComponent<ParticleSystem>())
        {
            particleEffect.GetComponent<ParticleSystem>().Emit(particlesAmount);
        }
    }

    public static void PlaySound(GameObject soundSource)
    {
        if (soundSource.GetComponent<AudioSource>())
        {
            soundSource.GetComponent<AudioSource>().PlayOneShot(soundSource.GetComponent<AudioSource>().GetComponent<AudioSource>().clip);
        }
    }

    [Serializable]
    public class PoolingItems: IMChangeable
    {
        public GameObject prefab;
        public int amount;

        public void SwitchMaterialToNight(bool isEnteringNight)
        {
            foreach (var part in prefab.GetComponentsInChildren<MeshRenderer>())
            {
                if (isEnteringNight)
                {
                    part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/White");
                }
                else
                {
                    part.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/DarkGrey");
                }
            }
        }
    }
}
