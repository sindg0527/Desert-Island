using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false; //마우스 커서 상태(유니티 게임상에서 커서 on/off)
        Cursor.lockState = CursorLockMode.Locked; //마우스 커서 고정
    }
}
