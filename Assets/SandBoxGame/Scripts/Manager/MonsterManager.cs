using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public enum MonsterState //몬스터의 상태를 enum으로 정의
    {
        Chase, //추적 상태
        Attack, //공격 상태
        Damage, //데미지 상태
        Idle, //대기 상태
        Die //죽음 상태
    }

    public MonsterState currentState = MonsterState.Idle; //현재 상태를 저장
    public Transform target;
    public int monsterDamage = 3; //몬스터 공격력
    public float attackRange = 1.0f; //공격범위
    public float targetRange = 3.0f; //인식범위
    public float attackCoolDown = 2.0f; //공격 딜레이 시간
    private float nextAttackTime = 0f; //공격한 시간 + 공격 딜레이 시간(공격한 시점부터 딜레이를 제기 위함)
    public Transform respawnPoints; //처음 생성된 위치
    public float moveSpeed = 2.0f; //이동속도

    private bool isAttack = false; //공격상태
    private float evadeRange = 5.0f; //회피거리
    private float monsterHP = 10.0f; //몬스터 체력
    float distanceToTarget; //몬스터와 플레이어의 거리

    Animator animator;
    private bool isWaiting = false; //상태에 대한 딜레이
    public float IdleTime = 2.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position); //몬스터와 플레이어의 거리

        if (monsterHP <= 0 && currentState != MonsterState.Die) //몬스터 체력이 0이하가 되면 죽음
        {
            currentState = MonsterState.Die;
        }

        switch (currentState)
        {
            case MonsterState.Chase:
                Chase();
                break;
            case MonsterState.Attack:
                Attack();
                break;
            case MonsterState.Die:
                Die();
                break;
            case MonsterState.Idle:
                if (!isWaiting)
                {
                    Idle();
                }
                break;
            case MonsterState.Damage:
                isDamage(monsterDamage);
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.DrawWireSphere(transform.position, targetRange);
    }

    void Attack()
    {
        animator.SetTrigger("isAttack");
        PlayerManager.Instance.PlayerDamage(2);
        Debug.Log("플레이어 공격");
    }

    void Chase()
    {
        if (distanceToTarget > attackRange) //플레이어가 공격범위를 벗어나면 추격
        {
            Debug.Log("플레이어 추격");
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void Idle()
    {
        Debug.Log("대기");
    }

    void Die()
    {
        Debug.Log("죽음");
    }

    public void isDamage(int damage)
    {
        monsterHP -= damage;
        Debug.Log($"{damage}의 피해를 입었다.");
    }

    IEnumerator TransitionState(MonsterState newState)
    {
        isWaiting = true; //대기 상태로 전환
        yield return new WaitForSeconds(IdleTime); //대기 시간
        currentState = newState; //새로운 상태로 전환
        isWaiting = false; //대기 상태 해제
    }
}
