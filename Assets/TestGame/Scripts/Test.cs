using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false; //���콺 Ŀ�� ����(����Ƽ ���ӻ󿡼� Ŀ�� on/off)
        Cursor.lockState = CursorLockMode.Locked; //���콺 Ŀ�� ����
    }
}
