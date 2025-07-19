using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pool;
using TMPro;
using UnityEngine;

public class ItemsView : MonoBehaviour
{
    public GameObject prefab;

    public Transform parent;

    public void UpdateView()
    {
        var data = ItemManager.Instance.items;
        if (data.Count > parent.childCount)
        {
            for (int i = parent.childCount; i < data.Count; i++)
            {
                GameObject go = Instantiate(prefab, parent);
            }
        }
        for (int i = 0; i < data.Count; i++)
        {
            parent.GetChild(i).gameObject.SetActive(true);
            parent.GetChild(i).GetComponentInChildren<EffectIcon>().Init(data[i]);
            parent.GetChild(i).GetComponentInChildren<EffectIcon>().image.sprite =
                Resources.Load<Sprite>("item/" + data[i].image);
        }

        for (int i = data.Count; i < parent.childCount; i++)
        {
            parent.GetChild(i).gameObject.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("ItemChanged", UpdateView);
        EventPool.OptIn<string>("ItemTrigger", trigger);
    }

    public void trigger(string actionName)
    {
        var data = ItemManager.Instance.items;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].actions.Contains(actionName))
            {
                parent.GetChild(i).transform.localScale = Vector3.one;
                parent.GetChild(i).transform.DOKill();
                parent.GetChild(i).transform.DOPunchScale(Vector3.one, 0.5f);
            }
        }
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_joker_activation") ;
    }
}
