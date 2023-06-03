using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{




    //  ----------------------  인풋 매니저 로 보낼예정 --------------------------------------

    [SerializeField] private InventoryUI Inventory_UI;
    public delegate void Item(int num);
    public static event Item UseItem;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // 왼쪽아래 작은 인벤툴팁 열기
        {
            Inventory_UI.InvenActive();
        }

        //인벤토리 List에서 n번째 아이템인지 확인해서 사용 , 키누르면 위아래로 I 인벤열기 , K 위 L 아래 J 사용

        if (Inventory_UI.isActiveInven)
        {

            if (Input.GetKeyDown(KeyCode.J))
            {


                UseItem?.Invoke(Inventory_UI.ListNum);
                Inventory_UI.UpdateUI();


            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                Inventory_UI.UpSelected();


            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Inventory_UI.DownSelected();
            }

            //}

        }

        // ---------------------------------------------------------------------------------------------
    }
}
