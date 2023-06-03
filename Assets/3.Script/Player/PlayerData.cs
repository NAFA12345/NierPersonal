using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static PlayerData instance = null;
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerData>();
                //Init();
                return instance;
            }
            else return instance;
        }
    }
    static void Init()
    {
        GameObject obj = new GameObject("PlayerManager");
        obj.AddComponent<PlayerData>();
        instance = obj.GetComponent<PlayerData>();
    }

    public float atk;
    public float def;

    public float _hp;
    public float maxHp = 100;
    public Inventory inven;
    public Weapon weapon;

    public float hp
    {
        get => _hp;
        set
        {
            if (value < 0)
            {
                _hp = 0;
                Die();
            }
            else if (value > maxHp)
            {
                _hp = maxHp;
            }
            else
            {
                _hp = value;
            }
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void OnDamage(float damage)
    {
        hp -= damage;
    }

    public void Die()
    {
        // todo 이벤트 발생 => 메인필드랑 flag 각각 다른 행동 해야될 것 같으니까 이벤트만 발생시켜야 될듯?
    }
}
