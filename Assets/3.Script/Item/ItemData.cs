using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public enum EItemType
    {
        HealingPotionSmall,
        HealingPotionMedium,
        HealingPotionLarge

    }

    public string ItemName;
    [Header("설명")]
    public string Tooltip;
    [Header("힐량")]
    public int HealingValue;
    [Header("갯수")]
    public int Quantity;
    
    [Header("아이템 종류")] // 잠시 보류 ㄱㄷ;
    public int ItemNum;
    public EItemType ItemType;

    


    //[Space]
    //[Header("칩 카운트량")]
    //[SerializeField] private float ChipCount;
    //[Header("칩 스텟")]
    //[SerializeField] private float Attack;
    //[SerializeField] private float Defence;
    //[SerializeField] private bool isAutoHeal;
    //[SerializeField] private bool isAutoFire;
    //[SerializeField] private bool isLifeSteal;




}
