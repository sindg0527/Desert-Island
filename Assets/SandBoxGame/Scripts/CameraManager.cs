using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //�÷��̾��� Transform ���� ����
    public Transform playerTr;

    //��� ������Ʈ�� Transform�� Renderer
    public Transform backGround;
    Renderer backGroundRenderer;

    //ī�޶��� ���� �̵������� �����ϴ� ����
    private Vector2 minPosition;
    private Vector2 maxPosition;

    //ī�޶� �þ߸� �����ϴ� ����
    public float cameraFOV = 60f;

    //ī�޶��� �ݰ�
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void Start()
    {
        backGroundRenderer = backGround.GetComponent<Renderer>();

        //ī�޶��� �þ߿� ���� ���� ũ�⸦ ���
        Camera camera = Camera.main;
        cameraHalfHeight = camera.orthographicSize; //ī�޶��� ���� �ݰ�
        cameraHalfWidth = cameraHalfHeight * camera.aspect; //ī�޶��� ��Ⱦ�� �ǹ�, ȭ���� �ʺ� ���̷� ���� ��
        CalculateCameraBounds();
    }

    void CalculateCameraBounds()
    {
        Bounds backGroundBounds = backGroundRenderer.bounds;

        minPosition = new Vector2(backGroundBounds.min.x + cameraHalfWidth,
            backGroundBounds.min.y + cameraHalfHeight);

        maxPosition = new Vector2(backGroundBounds.max.x - cameraHalfWidth,
            backGroundBounds.max.y - cameraHalfHeight);
    }

    void LateUpdate()
    {
        //�÷��̾��� ��ġ�� ī�޶� �̵�
        Vector3 newPosition = transform.position;

        newPosition.x = playerTr.position.x;
        newPosition.y = playerTr.position.y;

        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

        transform.position = newPosition;

        Camera.main.fieldOfView = cameraFOV;
    }
}