using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumber : MonoBehaviour
{
    public TMP_Text label;
    public float jumpHeight = 30;
    public void Init(DamageNumbersManager.DamageNumberObjData data)
    {
        label.text = data.amount.ToString();
        
        label.gameObject.SetActive(true);
        switch (data.damageNumberType)
        {
            case (DamageNumberType.industry):
            {
                label.color = Color.grey;
                break;
            }
            case (DamageNumberType.nature):
                
                label.color = Color.green;
                break;
           
        }
        //随机转动一些角度
        float time = 0.15f;
        //transform.DORotate(new Vector3(0, Random.Range(-10, 10), 0), time, RotateMode.LocalAxisAdd);
        var position = transform.position+new Vector3(0, jumpHeight, 0);
        transform.DOMove(position, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            transform.DOScale(0, time);
        });
        transform.localScale = Vector3.zero;
        
        var scaleTo = Vector3.one;
        transform.DOScale(scaleTo, time);
    }
}
