using UnityEngine;
using System.Collections.Generic;
using Pool;
using Unity.VisualScripting;

public class RandomSpawner : MonoBehaviour
{
    public BoxCollider2D spawnArea;            // 限制生成区域的 BoxCollider2D
    public BoxCollider2D spawnArea2;            // 限制生成区域的 BoxCollider2D
    public float minDistance = 2f;             // 新生成物体与其他物体的最小距离

    public void UpdateAll()
    {
            foreach (var animator in spawnArea2.transform.GetComponentsInChildren<Animator>())
            {
                var isDisabled = DisasterManager.Instance.buffManager.hasBuff("disableNatureMan");
                
                animator.GetComponent<SpriteRenderer>().color = isDisabled?Color.gray:Color.white;
            }
            foreach (var animator in spawnArea.transform.GetComponentsInChildren<Animator>())
            {
                var isDisabled = DisasterManager.Instance.buffManager.hasBuff("disableIndustryMan");
                animator.GetComponent<SpriteRenderer>().color = isDisabled ? Color.gray : Color.white;
            }
    }
    void Start()
    {
        EventPool.OptIn("DisasterChanged", UpdateAll);
    }

    public void SpawnPrefab(string name, int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnPrefab(name);
        }
    }

    void SpawnPrefab(string name)
    {
        var pa = spawnArea2;
        if (name == "industryMan")
        {
            pa = spawnArea;
        }
        // 获取所有当前子物体的位置
        List<Vector2> occupiedPositions = new List<Vector2>();
        foreach (Transform child in transform)
        {
            occupiedPositions.Add(child.position);
        }

        // 寻找一个尽可能远的位置
        Vector2 spawnPosition = FindFarAwayPosition(occupiedPositions,pa);

        // 实例化 Prefab
        var prefab = Resources.Load<GameObject>("characters/"+name);
        var go =Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
        go.transform.parent = pa.transform;

        var name2 = name;
        name2 = name2.FirstCharacterToUpper();
        var isDisabled = DisasterManager.Instance.buffManager.hasBuff("disable"+name2);
        go.GetComponentInChildren<Animator>().GetComponent<SpriteRenderer>().color = isDisabled ? Color.gray : Color.white;
    }

    Vector2 FindFarAwayPosition(List<Vector2> occupiedPositions ,BoxCollider2D pa)
    {
        Vector2 bestPosition = Vector2.zero;
        float maxDistance = 0f;

        // 设置边界
        Vector2 min = pa.bounds.min;
        Vector2 max = pa.bounds.max;

        // 随机尝试多个位置
        for (int i = 0; i < 10; i++)
        {
            // 随机生成一个位置
            Vector2 randomPosition = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            // 检查这个位置与现有物体的距离
            float distance = GetMinimumDistance(randomPosition, occupiedPositions);

            // 记录距离最大的点
            if (distance > maxDistance)
            {
                maxDistance = distance;
                bestPosition = randomPosition;
            }
        }

        return bestPosition;
    }

    // 计算新位置与现有物体的最小距离
    float GetMinimumDistance(Vector2 position, List<Vector2> occupiedPositions)
    {
        float minDist = float.MaxValue;

        foreach (var occupiedPosition in occupiedPositions)
        {
            float dist = Vector2.Distance(position, occupiedPosition);
            if (dist < minDist)
            {
                minDist = dist;
            }
        }

        return minDist;
    }
}
