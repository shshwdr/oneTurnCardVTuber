using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public GameObject moneyRain;
    public GameObject starFall;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SpawnMoneyRain(int value)
    {
        var go =Instantiate(moneyRain, transform.position, Quaternion.identity,transform);
        go.transform.position = moneyRain.transform.position;

        var newBurstCount = math.min( value,1000);
        
        var ps = go.GetComponent<ParticleSystem>();

        // 获取 Emission 模块
        var emission = ps.emission;

        // 获取现有的 burst 设置
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];
        emission.GetBursts(bursts);

        if (bursts.Length > 0)
        {
            // 修改第一个 burst 的 count
            bursts[0].count = newBurstCount;

            // 重新设置 bursts
            emission.SetBursts(bursts);
        }
        
        
        go.SetActive(true);
    }
    public void SpawnStarFall()
    {
        var go =Instantiate(starFall, transform.position, Quaternion.identity,transform);
        go.transform.position = starFall.transform.position;
        go.SetActive(true);
    }
    public void SpawnExplosion()
    {
        var go =Instantiate(explosion, transform.position, Quaternion.identity,transform);
        go.transform.position = explosion.transform.position;
        go.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
