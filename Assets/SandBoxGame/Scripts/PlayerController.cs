using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
//using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    float speed = 5.0f;
    Vector3 move;
    //public GameObject bulletObj;
    //public GameObject weaponObj;
    //public Transform bulletPos;
    //public int dir;
    //SpriteRenderer playerImage;
    //public Sprite[] rotationImage;

    Animator animator;

    [SerializeField]
    private Inventory theInventory;

    private void Start()
    {
        //dir = 0;
        //playerImage = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(new Vector3(move.x, move.y, 0) * speed * Time.deltaTime);
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

    private void OnTriggerStay2D(Collider2D collision)  // 아이템에 접촉 시 트리거 발생
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E 버튼 클릭");
            if (collision.gameObject.tag == "Item")  //태그가 아이템이면 pickupActivated True
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
