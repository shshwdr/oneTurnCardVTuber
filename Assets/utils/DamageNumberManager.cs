using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum DamageNumberType{industry, nature, gold}
public class DamageNumbersManager : Singleton<DamageNumbersManager>
{
    public GameObject collectionPrefab;
    IObjectPool<GameObject> resourceCollectionPool;
    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 50;
    public Transform parent;

    public struct  DamageNumberObjData
    {
        public Transform trans;
        public int amount;
        public DamageNumberType damageNumberType;
    }
    List<DamageNumberObjData> resourceFlyObjDatas = new List<DamageNumberObjData>();
    
    public void ShowResourceCollection(Transform trans, int amount, DamageNumberType damageNumberType)
    {
        if (resourceCollectionPool == null)
        {
            return;
        }

        var resourceFlyObjData = new DamageNumberObjData()
        {
            trans = trans,
            amount = amount,
            damageNumberType = damageNumberType,
        };
        
        resourceFlyObjDatas.Add(resourceFlyObjData);
    }

    private float flyTime = 1f;
    private float cooldown = 0.5f;
    private float cooldownTimer = 0;
    private void Update()
    {
        cooldownTimer+= Time.deltaTime;
        if (cooldownTimer<cooldown)
        {
            return;
        }
        //flyTimer+= Time.deltaTime;
        if (resourceFlyObjDatas.Count > 0 )
        {
            cooldownTimer = 0;
           // flyTimer = 0;
            var resourceFlyObjData = resourceFlyObjDatas[0];
            
           // var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, resourceFlyObjData.trans.position);
           if (resourceFlyObjData.trans == null)
           {
           }
           else
           {
               var go = resourceCollectionPool.Get();
               if (go == null)
               {
                   return;
               }
               go.transform.position = resourceFlyObjData.trans.position;
               go.GetComponent<DamageNumber>().Init(resourceFlyObjData);
           }
            resourceFlyObjDatas.RemoveAt(0);
        }
            
            
    }

    // Start is called before the first frame update
    void Start()
    {
        resourceCollectionPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
    }

    GameObject CreatePooledItem()
    {
        GameObject go = Instantiate(collectionPrefab,parent);
        return go;
    }
   
    // Called when an item is taken from the pool using Get
    void OnTakeFromPool(GameObject go)
    {
        if (!go || !go.GetComponent<PoolObject>())
        {
            return;
        }
        go.gameObject.SetActive(true);
        // if (!go.GetComponent<PoolObject>())
        // {
        //     return;
        // }
        go.GetComponent<PoolObject>().Init(resourceCollectionPool);
    }
// Called when an item is returned to the pool using Release
    void OnReturnedToPool(GameObject system)
    {
        system.gameObject.SetActive(false);
    }
    // If the pool capacity is reached then any items returned will be destroyed.
    // We can control what the destroy behavior does, here we destroy the GameObject.
    void OnDestroyPoolObject(GameObject system)
    {
        Destroy(system.gameObject);
    }
}
