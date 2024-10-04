using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public enum MonsterState //������ ���¸� enum���� ����
    {
        Chase, //���� ����
        Attack, //���� ����
        Damage, //������ ����
        Idle, //��� ����
        Die //���� ����
    }

    public MonsterState currentState = MonsterState.Idle; //���� ���¸� ����
    public Transform target;
    public int monsterDamage = 3; //���� ���ݷ�
    public float attackRange = 1.0f; //���ݹ���
    public float targetRange = 3.0f; //�νĹ���
    public float attackCoolDown = 2.0f; //���� ������ �ð�
    private float nextAttackTime = 0f; //������ �ð� + ���� ������ �ð�(������ �������� �����̸� ���� ����)
    public Transform respawnPoints; //ó�� ������ ��ġ
    public float moveSpeed = 2.0f; //�̵��ӵ�

    private bool isAttack = false; //���ݻ���
    private float evadeRange = 5.0f; //ȸ�ǰŸ�
    private float monsterHP = 10.0f; //���� ü��
    float distanceToTarget; //���Ϳ� �÷��̾��� �Ÿ�

    Animator animator;
    private bool isWaiting = false; //���¿� ���� ������
    public float IdleTime = 2.0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position); //���Ϳ� �÷��̾��� �Ÿ�

        if (monsterHP <= 0 && currentState != MonsterState.Die) //���� ü���� 0���ϰ� �Ǹ� ����
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
        Debug.Log("�÷��̾� ����");
    }

    void Chase()
    {
        if (distanceToTarget > attackRange) //�÷��̾ ���ݹ����� ����� �߰�
        {
            Debug.Log("�÷��̾� �߰�");
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void Idle()
    {
        Debug.Log("���");
    }

    void Die()
    {
        Debug.Log("����");
    }

    public void isDamage(int damage)
    {
        monsterHP -= damage;
        Debug.Log($"{damage}�� ���ظ� �Ծ���.");
    }

    IEnumerator TransitionState(MonsterState newState)
    {
        isWaiting = true; //��� ���·� ��ȯ
        yield return new WaitForSeconds(IdleTime); //��� �ð�
        currentState = newState; //���ο� ���·� ��ȯ
        isWaiting = false; //��� ���� ����
    }
}
