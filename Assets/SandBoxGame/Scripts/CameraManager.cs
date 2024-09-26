using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    //플레이어의 Transform 저장 변수
    public Transform playerTr;

    //배경 오브젝트의 Transform과 Renderer
    public Transform backGround;
    Renderer backGroundRenderer;

    //카메라의 절대 이동범위를 결정하는 변수
    private Vector2 minPosition;
    private Vector2 maxPosition;

    //카메라 시야를 설정하는 변수
    public float cameraFOV = 60f;
    public bool isShake = false;

    //카메라의 반경
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        backGroundRenderer = backGround.GetComponent<Renderer>();

        //카메라의 시야에 따른 절반 크기를 계산
        Camera camera = Camera.main;
        cameraHalfHeight = camera.orthographicSize; //카메라의 세로 반경
        cameraHalfWidth = cameraHalfHeight * camera.aspect; //카메라의 종횡비를 의미, 화면의 너비를 높이로 나눈 값
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
        //플레이어의 위치로 카메라 이동
        Vector3 newPosition = transform.position;

        newPosition.x = playerTr.position.x;
        newPosition.y = playerTr.position.y;

        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);

        transform.position = newPosition;

        Camera.main.fieldOfView = cameraFOV;
    }
}