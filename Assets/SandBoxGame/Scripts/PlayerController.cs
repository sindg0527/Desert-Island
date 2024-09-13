using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float speed = 5.0f;
    Vector3 move;
    public GameObject bulletObj;
    public GameObject weaponObj;
    public Transform bulletPos;
    public int dir;
    SpriteRenderer playerImage;
    public Sprite[] rotationImage;

    private void Start()
    {
        dir = 0;
        playerImage = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(new Vector3(move.x, move.y, 0) * speed * Time.deltaTime);
        Rotation();
    }

    void OnMovement(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    void Rotation()
    {
        if (move.x > 0)
        {
            dir = 0;
            PlayerImage(dir);
            weaponObj.transform.localEulerAngles = new Vector3(0, 0, 0);
            //transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (move.x < 0)
        {
            dir = 1;
            PlayerImage(dir);
            weaponObj.transform.localEulerAngles = new Vector3(0, 180, 0);
            //weaponObj.transform.position = new Vector3(-0.187f, -0.137f, 0);
            //transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (move.y > 0)
        {
            dir = 2;
            PlayerImage(dir);
            weaponObj.transform.localEulerAngles = new Vector3(0, 0, 90);
            //transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (move.y < 0)
        {
            dir = 3;
            PlayerImage(dir);
            weaponObj.transform.localEulerAngles = new Vector3(0, 0, 270);
            //transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void OnFire()
    {
        Instantiate(bulletObj, bulletPos.transform.position, Quaternion.identity);
    }

    void PlayerImage(int count)
    {
        if (count == 0)
        {
            playerImage.sprite = rotationImage[0];
        }
        if (count == 1)
        {
            playerImage.sprite = rotationImage[1];
        }
        if (count == 2)
        {
            playerImage.sprite = rotationImage[2];
        }
        if (count == 3)
        {
            playerImage.sprite = rotationImage[3];
        }
    }
}
