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

    private void Start()
    {
        //dir = 0;
        //playerImage = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(new Vector3(Mathf.Abs(move.x), move.y, 0) * speed * Time.deltaTime);
        Rotation(); //�÷��̾� ȸ��
    }

    void OnMovement(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    void Rotation() //�÷��̾� ȸ�� �� �ִϸ��̼�
    {
        if (move.x > 0) //������
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            //animator.SetFloat("isHorizontal", move.x);
            Debug.Log("������" + move.x);
        }
        else if (move.x < 0) //����
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            //animator.SetFloat("isHorizontal", move.x);
            Debug.Log("����" + move.x);
        }
        else if (move.y > 0) //��
        {
            //animator.SetFloat("isUp", move.y);
            Debug.Log("����" + move.y);
        }
        else if (move.y < 0) //�Ʒ�
        {
            //animator.SetFloat("isDown", Mathf.Abs(move.y));
            Debug.Log("�Ʒ���" + Mathf.Abs(move.y));
        }
        animator.SetFloat("isHorizontal", move.x);
        animator.SetFloat("isUp", move.y);
        animator.SetFloat("isDown", Mathf.Abs(move.y));
    }

    void OnFire()
    {
        //Instantiate(bulletObj, bulletPos.transform.position, Quaternion.identity);
    }
}
