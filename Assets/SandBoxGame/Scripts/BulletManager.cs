using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    float BulletSpeed = 10.0f;
    Rigidbody2D rigid;
    PlayerController playerController;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerController = FindObjectOfType<PlayerController>();

        //if (playerController.dir == 0)
        //{
        //    rigid.AddForce(new Vector2(BulletSpeed, 0), ForceMode2D.Impulse);
        //}
        //else if (playerController.dir == 1)
        //{
        //    rigid.AddForce(new Vector2(-BulletSpeed, 0), ForceMode2D.Impulse);
        //}
        //else if (playerController.dir == 2)
        //{
        //    rigid.AddForce(new Vector2(0, BulletSpeed), ForceMode2D.Impulse);
        //    transform.localEulerAngles = new Vector3(0, 0, 90);
        //}
        //else if (playerController.dir == 3)
        //{
        //    rigid.AddForce(new Vector2(0, -BulletSpeed), ForceMode2D.Impulse);
        //    transform.localEulerAngles = new Vector3(0, 0, 90);
        //}

        //Invoke(DestroyObj(), 1000);
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
