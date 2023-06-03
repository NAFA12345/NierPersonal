using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    // 물체를 감지해서 밖으로 나가면 삭제하거나 비활성화
    [Header("물체를 감지할 콜라이더")]
    [SerializeField] public Collider objectDestroyer;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BulletHard") || (other.CompareTag("BulletSoft")))
        {
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent(out FlagEmInformation flagEmInformation))
            {
                flagEmInformation.Disappear();
            }
        }
        else if (other.CompareTag("FlagBullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
