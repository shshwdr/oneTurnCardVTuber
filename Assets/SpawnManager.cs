using System.Collections;
using System.Collections.Generic;
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

    public void SpawnMoneyRain()
    {
        var go =Instantiate(moneyRain, transform.position, Quaternion.identity,transform);
        go.transform.position = moneyRain.transform.position;
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
