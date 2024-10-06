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

        Vector3 direction = mousePos - transform.position; //총알이 마우스를 바라보게 설정
        Vector3 rotation = transform.position - mousePos; //총알의 각도
        rigid.velocity = new Vector2(direction.x, direction.y).normalized * speed; //총알 이동
        float rotationZ = Mathf.Atan2(rotation.x, rotation.y) * Mathf.Rad2Deg * -1; //방향에 따른 회전
        transform.rotation = Quaternion.Euler(0, 0, rotationZ - 90); //총알의 회전값
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            MonsterManager.instance.isDamage(ToolManager.instance.getItem.itemValue);
            Debug.Log($"{collision.name}이 2데미지를 입었습니다.");
        }
        ToolManager.instance.ReturnBulletToPool(gameObject);
    }
}