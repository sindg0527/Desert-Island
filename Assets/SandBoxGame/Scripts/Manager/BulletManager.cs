using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCamera;
    Rigidbody2D rigid;
    float speed = 10.0f;

    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main;
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position; //�Ѿ��� ���콺�� �ٶ󺸰� ����
        Vector3 rotation = transform.position - mousePos; //�Ѿ��� ����
        rigid.velocity = new Vector2(direction.x, direction.y).normalized * speed; //�Ѿ� �̵�
        float rotationZ = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg; //���⿡ ���� ȸ��
        transform.rotation = Quaternion.Euler(0, 0, rotationZ + 90); //�Ѿ��� ȸ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ToolManager.instance.ReturnBulletToPool(gameObject);
        if (collision.tag == "Player")
        {
            PlayerManager.Instance.PlayerDamage(2);
            Debug.Log($"{collision.name}�� 2�������� �Ծ����ϴ�.");
        }
    }
}