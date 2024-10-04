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
    public enum PlayerDirection
    {
        right,
        left,
        up,
        down
    }

    [Header("-----Player Settings-----")]
    public float playerSpeed = 5.0f;
    public int playerHP = 100;

    public Vector3 move; //플레이어 이동
    public PlayerDirection direction; //플레이어의 방향
    public bool playerPause = false;
    private bool isDamage = false;
    private bool isDie = false;

    SpriteRenderer spriteRenderer;
    Animator animator;

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
    }

    void OnMovement(InputValue value)
    {
        Vector2 input = value.Get<Vector2>(); //(x, y)

        if (input != null && !playerPause)
        {
            move = new Vector3(input.x, input.y, 0); //(1, 0, 0), (-1, 0, 0)
            animator.SetBool("isMove", true);
        }
        if (move.x == 0 && move.y == 0)
        {
            animator.SetBool("isMove", false);
        }
    }

    void RotationAnimation() //플레이어 회전 및 애니메이션
    {
        if (move.x > 0) //오른쪽
        {
            direction = PlayerDirection.right;
            animator.SetFloat("xDir", move.x);
            animator.SetFloat("yDir", 0);
        }
        else if (move.x < 0) //왼쪽
        {
            direction = PlayerDirection.left;
            animator.SetFloat("xDir", move.x);
            animator.SetFloat("yDir", 0);
        }
        else if (move.y > 0) //위
        {
            direction = PlayerDirection.up;
            animator.SetFloat("xDir", 0);
            animator.SetFloat("yDir", move.y);
        }
        else if (move.y < 0) //아래
        {
            direction = PlayerDirection.down;
            animator.SetFloat("xDir", 0);
            animator.SetFloat("yDir", move.y);
        }
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
            //WeaponManager.instance.shakeDuration = 0.1f;
            //WeaponManager.instance.shakeMagnitude = 0.2f;
            //StartCoroutine(WeaponManager.instance.Shake(WeaponManager.instance.shakeDuration, WeaponManager.instance.shakeMagnitude));

            spriteRenderer.material.color = Color.red;
            StartCoroutine(DamageCount()); //코루틴 실행
        }
    }
}
