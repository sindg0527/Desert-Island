using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
        Rotation(); //�÷��̾� ȸ��
    }

    void OnMovement(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); //(x, y)
        if (input != null)
        {
            animator.SetBool("isRun", true);
            animator.SetBool("isIdle", false);

            move = new Vector3(input.x, input.y, 0); //(1, 0, 0), (-1, 0, 0)
        }
        if (input.x == 0 && input.y == 0)
        {
            animator.SetBool("isIdle", true);
            animator.SetBool("isMove", false);
        }
    }

    void Rotation() //�÷��̾� ȸ�� �� �ִϸ��̼�
    {
        if (move.x > 0) //������
        {
            animator.SetBool("isRight", true);
            animator.SetBool("isLeft", false);
            animator.SetBool("isBack", false);
            animator.SetBool("isFront", false);
        }
        else if (move.x < 0) //����
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", true);
            animator.SetBool("isBack", false);
            animator.SetBool("isFront", false);
        }
        else if (move.y > 0) //��
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
            animator.SetBool("isBack", true);
            animator.SetBool("isFront", false);
        }
        else if (move.y < 0) //�Ʒ�
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

    private void OnTriggerStay2D(Collider2D collision)  // �����ۿ� ���� �� Ʈ���� �߻�
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (collision.CompareTag("Item"))  //�±װ� �������̸� pickupActivated True
            {

                if (collision.transform != null)
                {
                    Debug.Log(collision.transform.GetComponent<ItemPickUp>().item.itemName + " ȹ�� �߽��ϴ�.");
                    theInventory.AcquireItem(collision.transform.GetComponent<ItemPickUp>().item);  // �κ��丮 �ֱ�
                    Destroy(collision.transform.gameObject);
                }
            }
        }
    }
}
