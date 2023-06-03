using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //우측하단 아이템 사용 텍스트
    [SerializeField] private Text Effect_Text;
    //포션 사용 딜레이를 주기위한 bool값
    private bool PotionDelay = false;
    public List<ItemData> Items = new List<ItemData>();
    public void AddItem(ItemData item) //아이템 획득시
    {
        int itemIndex = Items.FindIndex(x => x.Quantity == item.Quantity);

        if (itemIndex < 0) //아이템이 없다면 아이템 생성
        {
            Items.Add(item);
        }
        else //아이템이 있다면 아이템 갯수 추가
        {
            Items[itemIndex].Quantity++;
        }
    }
    
    public void RemoveItem(ItemData item)
    {
        if(PotionDelay) //포션 딜레이 상태라면 사용할수 없도록
        {
            return;
        }
        ItemData removeItem = Items.Find(x => x.ItemName == item.ItemName);
        if (removeItem !=null)
        {
            
            if (removeItem.Quantity <= 1) //아이템이 1개 이하에서 사용하면 아이템 삭제
            {
                removeItem.Quantity--;
                StartCoroutine(Text_co(removeItem));
                
                Items.Remove(removeItem);
            }
            else //아이템이 2개 이상이라면 아이템 갯수 -1
            {
                removeItem.Quantity--;
                StartCoroutine(Text_co(removeItem));
            }
        }
    }
    private IEnumerator Text_co(ItemData item)
    {
        PotionDelay = true;
        Effect_Text.gameObject.SetActive(true);
        Effect_Text.text = string.Format("{0} 를 사용하였습니다. 남은 아이템의 갯수 {1} 개", item.ItemName, item.Quantity);
        yield return new WaitForSeconds(0.5f);
        Effect_Text.gameObject.SetActive(false);
        PotionDelay = false;
    }




}
