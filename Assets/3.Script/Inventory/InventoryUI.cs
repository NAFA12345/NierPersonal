using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Text[] Slot_Text;
    [SerializeField] Text[] SlotQuantity_Text;
    [SerializeField] Image[] Slot_Image;
    [SerializeField] GameObject Selected_Item;
    [SerializeField] GameObject Inventory_UI_ob;

    Vector3 MoveSelectedPoint = new Vector3(0, -85.7f, 0);
    Vector3 defalutItemPosition;
    Color Selected_BackColor = new Color(218 / 255f, 212 / 255f, 186 / 255f);
    Color InvenUI_BackColor = new Color(78 / 255f, 76 / 255f, 66 / 255f);

    private int MaxInvenLength = 3;
    private int InvenLength;
    public int ListNum = 0;


    public bool isActiveInven = false;

    private void Awake() //처음 위치 (1번자리) 값 할당
    {
        defalutItemPosition = Selected_Item.transform.position;

    }
    private void OnEnable() // 활성화가 될때마다 그 할당된 위치로 갈수있도록
    {
        UpdateUI();
    }

    public void UpdateUI()
    {

        InvenLength = PlayerData.Instance.inven.Items.Count; //인벤토리의 길이 캐싱
        if (InvenLength == 0) // 인벤토리가 없을시 인벤토리가 열리지 않게
        {
            Inventory_UI_ob.SetActive(false);
            return;
        }
        Selected_Item.transform.position = defalutItemPosition; // 다시 활성화시 위치 맨위로 올리기 위함

        ListNum = 0; // 변수값 초기화

        ClearSlot();

        for (int i = 0; i < InvenLength; i++)  //아이템 이름
        {
            Slot_Text[i].text = string.Format("{0}", PlayerData.Instance.inven.Items[i].ItemName);

        }
        for (int i = 0; i < InvenLength; i++) // 아이템 수량
        {
            SlotQuantity_Text[i].text = string.Format("{0}", PlayerData.Instance.inven.Items[i].Quantity);
        }

        ColorSet();

    }

    public void InvenActive() //인벤토리를 열고,닫는 메서드
    {
        if (!isActiveInven && InvenLength != 0
            )
        {
            isActiveInven = true;
            Inventory_UI_ob.SetActive(true);
            UpdateUI();

        }
        else
        {
            isActiveInven = false;
            Inventory_UI_ob.SetActive(false);
        }
    }

    public void DownSelected() 
    {
        if (ListNum >= 0 && ListNum < InvenLength - 1)
        {
            ListNum++;
            Selected_Item.transform.position += MoveSelectedPoint;
        }
        ColorSet();
    }

    public void UpSelected()
    {

        if (ListNum > 0 && ListNum < InvenLength)
        {
            ListNum--;
            Selected_Item.transform.position -= MoveSelectedPoint;
        }
        ColorSet();
    }

    public void ColorSet() // 커서 움직일시 색을 조절해줌.
    {
        for (int i = 0; i < InvenLength; i++)
        {
            Slot_Text[i].color = Color.white;
            SlotQuantity_Text[i].color = Color.white;
            Slot_Image[i].color = InvenUI_BackColor;
        }
        Slot_Text[ListNum].color = Color.black;
        SlotQuantity_Text[ListNum].color = Color.black;
        Slot_Image[ListNum].color = Selected_BackColor;
    }
    public void ClearSlot()  // 아이템이 리스트에서 제거됐을때 지우기 위함
    {
        for (int i = 0; i < MaxInvenLength; i++)
        {
            Slot_Text[i].text = "";
            SlotQuantity_Text[i].text = "";
            Slot_Image[i].color = InvenUI_BackColor;
        }
    }
}
