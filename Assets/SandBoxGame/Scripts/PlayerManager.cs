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
        RotationAnimation();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E ��ư Ŭ��");

            //���� ĳ��Ʈ�� ��
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, ItemMask);
            transform.rotation = Quaternion.Euler(0, 0, 0);

            if (hit.collider != null && hit.collider.tag != "Player")
            {
                Debug.Log(hit.collider.GetComponent<ItemPickUp>().item.itemName + " ȹ�� �߽��ϴ�.");
                theInventory.AcquireItem(hit.collider.GetComponent<ItemPickUp>().item);  // �κ��丮 �ֱ�
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void OnMovement(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); //(x, y)

        if (input != null)
        {
            move = new Vector3(input.x, input.y, 0); //(1, 0, 0), (-1, 0, 0)
            //RotationAnimation(); //�÷��̾� ȸ��
            animator.SetBool("isMove", true);
            //animator.SetBool("isIdle", false);
        }
        if (move.x == 0 && move.y == 0)
        {
            //animator.SetBool("isIdle", true);
            //animator.SetBool("isMove", false);
            animator.SetBool("isMove", false);
        }
    }

    void RotationAnimation() //�÷��̾� ȸ�� �� �ִϸ��̼�
    {
        if (move.x > 0) //������
        {
            //animator.SetBool("isRight", true);
            //animator.SetBool("isLeft", false);
            //animator.SetBool("isBack", false);
            //animator.SetBool("isFront", false);
            Debug.DrawRay(transform.position, transform.right, Color.red, 0.5f);
            animator.SetFloat("xDir", move.x);
            animator.SetFloat("yDir", 0);
        }
        else if (move.x < 0) //����
        {
            //animator.SetBool("isRight", false);
            //animator.SetBool("isLeft", true);
            //animator.SetBool("isBack", false);
            //animator.SetBool("isFront", false);
            Debug.DrawRay(transform.position, -transform.right, Color.red, 0.5f);
            animator.SetFloat("xDir", move.x);
            animator.SetFloat("yDir", 0);
        }
        else if (move.y > 0) //��
        {
            //animator.SetBool("isRight", false);
            //animator.SetBool("isLeft", false);
            //animator.SetBool("isBack", true);
            //animator.SetBool("isFront", false);
            Debug.DrawRay(transform.position, transform.up, Color.red, 0.5f);
            animator.SetFloat("xDir", 0);
            animator.SetFloat("yDir", move.y);
        }
        else if (move.y < 0) //�Ʒ�
        {
            //animator.SetBool("isRight", false);
            //animator.SetBool("isLeft", false);
            //animator.SetBool("isBack", false);
            //animator.SetBool("isFront", true);
            Debug.DrawRay(transform.position, -transform.up, Color.red, 0.5f);
            animator.SetFloat("xDir", 0);
            animator.SetFloat("yDir", move.y);
        }
    }

    void OnFire()
    {
        //Instantiate(bulletObj, bulletPos.transform.position, Quaternion.identity);
    }

    IEnumerator DamageCount() //�����ð�
    {
        playerSpeed = 2.0f;
        yield return new WaitForSeconds(0.5f); //���� �ڵ带 �����ð��� ������ ����
        isDamage = false;
        playerSpeed = 3.0f;
        spriteRenderer.material.color = Color.white;
    }

    //�÷��̾� �ǰ�
    public void PlayerDamage(int damage)
    {
        if (!isDamage)
        {
            if (playerHP <= 0) //ü���� 0���ϰ� �� ��
            {
                isDie = true;
                playerHP = 0;
            }
            playerHP -= damage;
            SoundManager.instance.PlaySFX("hit");
            //WeaponManager.instance.shakeDuration = 0.1f;
            //WeaponManager.instance.shakeMagnitude = 0.2f;
            //StartCoroutine(WeaponManager.instance.Shake(WeaponManager.instance.shakeDuration, WeaponManager.instance.shakeMagnitude));

            spriteRenderer.material.color = Color.red;
            StartCoroutine(DamageCount()); //�ڷ�ƾ ����
        }
    }
}
