using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Player : MonoBehaviour
{
    private static Main_Player instance;
    public static Main_Player Instance { get { return instance; } }

    [Header("컴포넌트")]
    StateManager sm;
    public Animator anim_player;
    public Animator anim_sword;
    public Animator anim_bigsword;
    public CapsuleCollider collider_body;
    public BoxCollider collider_sword;
    public BoxCollider collider_bigsword;
    [Header("검 거치대")]
    public GameObject idleSword;
    public GameObject idleBigSword;
    public GameObject bigSword;
    public GameObject sword;
    [Header("손 위치")]
    public Transform leftHand;
    public Transform rightHand;
    [Header("손에 맞출 포지션")]
    public Transform swordBone;
    public Transform bigSwordBone;


    [Header("컨트롤용 불변수")]
    public bool isGrounded;
    public bool isDash;
    public bool isAtk;
    public bool isDodge;
    public bool isHitted;
    public bool isSwordhand;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        sm = GetComponent<StateManager>();
        isGrounded = true;
        isDash = false;
        isAtk = false;
        isDodge = false;
        isHitted = false;
        collider_sword.enabled = false;
        collider_bigsword.enabled = false;
    }

    public void ResetBool()
    {
        isDash = false;
        isAtk = false;
        isDodge = false;
        isHitted = false;
    }
    //public void Rotation2(Vector3 inputVec)
    //{
    //    StartCoroutine(Rotatewhile(inputVec));
    //}
    //public IEnumerator Rotatewhile(Vector3 inputVec)
    //{
    //    Quaternion targetRotation = Quaternion.LookRotation(inputVec, Vector3.up);
    //    while (transform.rotation != targetRotation)
    //    {
    //        transform.rotation = 
    //            Quaternion.RotateTowards(transform.rotation, targetRotation, 1f * Time.deltaTime);
    //        yield return null;
    //    }
    //}
    public void Rotate()
    {
        Vector3 rotation = new Vector3(0f, Input.GetAxis("Horizontal"), 0f) * 1f;
        transform.Rotate(rotation);
    }
    public bool isCanHit()
    {
        if (isDodge || isHitted) return false;
        else return true;
    }
    public bool isCanAttack()
    {
        if (isDodge || isHitted) return false;
        else return true;
    }
    public void OnDamage(float damage)
    {
        if (isCanHit())
        {
            sm.ChangeState(sm.hitted);
            PlayerData.Instance.OnDamage(damage);
        }
        else Debug.Log("무적 상태입니다");
        //todo 닷지상태일 때, 닷지 효과 재생.
    }
    public void SwordToHand()
    {
        swordBone.position = leftHand.position;
    }
    public void BigSwordToHand()
    {
        bigSwordBone.position = rightHand.position;
    }

    [Header("메쉬베이크용")]
    [SerializeField] MeshBake meshBake;
    public void HittedWhileDodge()
    {
        
    }

    private IEnumerator DestroyCopiesAfterDelay(GameObject[] copies, float delay)
    {
        yield return new WaitForSeconds(delay);

        for (int i = 0; i < copies.Length; i++)
        {
            Destroy(copies[i]);
        }
    }
}
