using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
//using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("-----Player Settings-----")]
    public float playerSpeed = 5.0f;
    public int playerHP = 100;
    public bool isDamage = false;
    public bool isDie = false;
    public bool isAction = false;

    Vector3 move;
    SpriteRenderer spriteRenderer;
    public LayerMask ItemMask;

    Animator animator;

    [SerializeField]
    private Inventory theInventory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(new Vector3(move.x, move.y, 0) * playerSpeed * Time.deltaTime);
    }

    void OnMovement(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); //(x, y)

        if (input != null)
        {
            move = new Vector3(input.x, input.y, 0); //(1, 0, 0), (-1, 0, 0)
            RotationAnimation(); //플레이어 회전
            animator.SetBool("isMove", true);
            animator.SetBool("isIdle", false);
        }
        if (move.x == 0 && move.y == 0)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isMove", false);
        }
    }

    void RotationAnimation() //플레이어 회전 및 애니메이션
    {
        if (move.x > 0) //오른쪽
        {
            animator.SetBool("isRight", true);
            animator.SetBool("isLeft", false);
            animator.SetBool("isBack", false);
            animator.SetBool("isFront", false);
        }
        else if (move.x < 0) //왼쪽
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", true);
            animator.SetBool("isBack", false);
            animator.SetBool("isFront", false);
        }
        else if (move.y > 0) //위
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
            animator.SetBool("isBack", true);
            animator.SetBool("isFront", false);
        }
        else if (move.y < 0) //아래
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
            animator.SetBool("isBack", false);
            animator.SetBool("isFront", true);
        }
    }

    void OnFire()
    {
        //Instantiate(bulletObj, bulletPos.transform.position, Quaternion.identity);
    }

    IEnumerator DamageCount() //무적시간
    {
        playerSpeed = 2.0f;
        yield return new WaitForSeconds(0.5f); //이후 코드를 일정시간이 지나고 실행
        isDamage = false;
        playerSpeed = 3.0f;
        spriteRenderer.material.color = Color.white;
    }

    //플레이어 피격
    public void PlayerDamage(int damage)
    {
        if (!isDamage)
        {
            if (playerHP <= 0) //체력이 0이하가 될 때
            {
                isDie = true;
                playerHP = 0;
            }
            playerHP -= damage;
            SoundManager.instance.PlaySFX("hit");
            WeaponManager.instance.shakeDuration = 0.1f;
            WeaponManager.instance.shakeMagnitude = 0.2f;
            StartCoroutine(WeaponManager.instance.Shake(WeaponManager.instance.shakeDuration, WeaponManager.instance.shakeMagnitude));

            spriteRenderer.material.color = Color.red;
            StartCoroutine(DamageCount()); //코루틴 실행
        }
    }

    private void OnTriggerStay2D(Collider2D collision)  // 아이템에 접촉 시 트리거 발생
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E 버튼 클릭");

            //레이 캐스트로 비교
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, ItemMask);
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            //if (hit.collider != null && hit.collider.tag != "Player")

            if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                if (collision.transform != null)
                {
                    Debug.Log(collision.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                    theInventory.AcquireItem(collision.transform.GetComponent<ItemPickUp>().item);  // 인벤토리 넣기
                    Destroy(collision.transform.gameObject);
                }
            }
        }
    }
}
