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
        rigid2D = GetComponent<Rigidbody2D>(); //오브젝트의 Rigidbody2D 컴포넌트를 가져옴
    }

    void Update()
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //부드러운 회전을 위해 선형보간을 사용

        PlayerMove();
    }

    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //rigid2D.AddForce(new Vector2(speed, 0), ForceMode2D.Force); //물리에 의한 이동
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0)); //좌표에 의한 이동
            //transform.rotation //get; set은 대입
            transform.localEulerAngles = new Vector3(0, 0, 0); //오브젝트의 회전(rotation)을 벡터값으로 변환
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
