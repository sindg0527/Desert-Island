using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    float speed = 3.0f;
    Rigidbody2D rigid2D;

    public Transform target;
    public float rotationSpeed = 5.0f;

    private void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>(); //������Ʈ�� Rigidbody2D ������Ʈ�� ������
    }

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //�ε巯�� ȸ���� ���� ���������� ���

        PlayerMove();
    }

    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //rigid2D.AddForce(new Vector2(speed, 0), ForceMode2D.Force); //������ ���� �̵�
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0)); //��ǥ�� ���� �̵�
            //transform.rotation //get; set�� ����
            transform.localEulerAngles = new Vector3(0, 0, 0); //������Ʈ�� ȸ��(rotation)�� ���Ͱ����� ��ȯ
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //rigid2D.AddForce(new Vector2(-speed, 0), ForceMode2D.Force);
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
    }
}
