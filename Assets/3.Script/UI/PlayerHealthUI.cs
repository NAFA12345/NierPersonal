using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    //[SerializeField] Text Level_Txt;
    

    [SerializeField] private Slider Hp_Slider;

    private void Update()
    {
        
        Hp_Slider.value = PlayerData.Instance._hp / PlayerData.Instance.maxHp;
        //Level_Txt.text = string.Format("Lv : {0}",PlayerData.Instance.Level); << 레벨이 없어서 보류 1로 고정시켜놓음. 사용할때 Level_txt도 주석풀것
    }


    //Invoke 지연함수 쓸때 사용하면댐 
    public void AcitveFalse()
    {
        gameObject.SetActive(false);
    }
    public void AcitveTrue()
    {
        gameObject.SetActive(true);
    }
}
