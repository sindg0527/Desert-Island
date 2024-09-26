using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    private Camera mainCamera;
    private Vector3 mousePos;


    [Header("Bullet System")]
    public GameObject bulletObj;

    [Header("Gun System")]
    //public GameObject[] Guns;
    public GameObject GetItem;
    public Transform weaponPos;
    public LayerMask weaponMake;

    public int poolSize = 100; //오브젝트 풀링 사이즈
    private List<GameObject> bulletPool; //오브젝트 풀 리스트

    public bool IsFire = false; //총을 쏘는 여부
    private float timer; //시간저장변수
    private float delayTime = 0.2f; //총을 쏘고 다시 쏘는데까지 걸리는 시간
    //private int bulletCount = 0; //총알 사용 횟수
    //private int maxBulletCount = 3; //사용 가능한 총알 수

    public float rayDistance = 100f;
    public LayerMask targetLayer;
    Vector2 rayDirection; //레이의 방향

    [Header("Weapon Settings")]
    public bool IsGetPistol = false;
    public bool IsGetShotGun = false;

    [Header("Shotgun Settings")]
    public int pelletCount = 10; //샷건 탄환 개수
    public float spreaAngle = 30; //샷건 탄환이 퍼지는 각도
    public float knockbakForce = 10f; //충돌한 오브젝트를 밀어내는 힘의 크기

    private int pistolBulletCount = 100;
    private int shotGunBulletCount = 100;
    private int maxBulletCount = 100;

    [Header("CameraShake Settings")]
    public float shakeDuration = 0.5f; //흔들림 지속시간
    public float shakeMagnitude = 0.1f; //흔들림 강도
    private Vector3 originalPos; //카메라 원래 위치

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        mainCamera = Camera.main;

        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++) //비활성화된 총알 미리 생성
        {
            GameObject bullet = Instantiate(bulletObj);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }

        if (Camera.main != null)
        {
            originalPos = Camera.main.transform.localPosition;
        }
        else
        {
            Debug.Log("메인 카메라를 찾을 수 없습니다. MainCamera Tag를 확인해주세요.");
        }
    }

    void Update()
    {
        //마우스 위치 업데이트 및 월드 좌표값 반환
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position; //마우스 방향

        //총알 발사 위치에서 마우스 방향으로의 벡터 계산
        rayDirection = (mousePos - transform.position).normalized;

        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        //Atan() : 함수는 좌표평면에서 수평축으로부터 한 점까지의 각도를 구하는 함수

        transform.rotation = Quaternion.Euler(0, 0, rotationZ); //총구가 마우스를 따라갈 수 있게 각도 변경
        //Debug.DrawRay(bulletPos.position, rayDirection * rayDistance, Color.green); //씬에서 실시간으로 레이 확인

        if (!IsFire) //총을 쏘지 않았을 때
        {
            timer += Time.deltaTime;
            if (timer > delayTime) //timer가 딜레이타임을 넘었을 때 총 발사 가능
            {
                IsFire = true;
                timer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E Key");
            Debug.DrawLine(transform.position, transform.right, Color.red, 1.0f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, 1, weaponMake);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            if (hit.collider != null && hit.collider.tag != "Player")
            {
                SoundManager.instance.PlaySFX("WeaponEquip"); //아이템 장착 및 교체 소리
                if (hit.collider.tag == "Pistol")
                {
                    shakeDuration = 0.1f;
                    shakeMagnitude = 0.05f;
                    if (IsGetShotGun)
                    {
                        GetItem.transform.SetParent(null);
                        IsGetShotGun = false;
                    }
                    hit.collider.transform.SetParent(transform);
                    hit.collider.transform.localPosition = Vector3.zero;
                    IsGetPistol = true;
                    GetItem = hit.collider.gameObject;
                }
                else if (hit.collider.tag == "ShotGun")
                {
                    shakeDuration = 0.1f;
                    shakeMagnitude = 0.2f;
                    if (IsGetPistol)
                    {
                        GetItem.transform.SetParent(null);
                        IsGetPistol = false;
                    }
                    hit.collider.transform.SetParent(transform);
                    hit.collider.transform.localPosition = Vector3.zero;
                    IsGetShotGun = true;
                    GetItem = hit.collider.gameObject;
                }
                else if (hit.collider.tag == "PistolBullet")
                {
                    pistolBulletCount += 5;
                    if (pistolBulletCount > maxBulletCount)
                    {
                        pistolBulletCount = maxBulletCount;
                    }
                    Debug.Log(pistolBulletCount);
                }
                else if (hit.collider.tag == "ShotGunBullet")
                {
                    shotGunBulletCount += 5;
                    if (shotGunBulletCount > maxBulletCount)
                    {
                        shotGunBulletCount = maxBulletCount;
                    }
                }
            }
        }
    }

    //풀에서 비활성화된 총알을 가져오는 함수
    GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy) //자신과 부모가 활성화가 아닐경우
            {
                return bullet;
            }
        }
        //풀에 사용할 수 있는 총알이 없으면 Null 반환
        return null;
    }

    void OnFire() //총 발사
    {
        if (IsFire && PlayerManager.Instance.isDie == false)
        {
            if (IsGetPistol && pistolBulletCount > 0)
            {
                pistolBulletCount--;
                delayTime = 0.2f;
                FirePistol();
                StartCoroutine(Shake(shakeDuration, shakeMagnitude));
                return;
            }
            else if (IsGetShotGun && shotGunBulletCount > 0)
            {
                shotGunBulletCount--;
                delayTime = 1.0f;
                FireShotGun();
                StartCoroutine(Shake(shakeDuration, shakeMagnitude));
                return;
            }
            else
            {
                Debug.Log("무기 없음");
                //SoundManager.instance.PlaySFX("NonWeapon");
            }
            IsFire = false;
        }
    }

    void FirePistol()
    {
        SoundManager.instance.PlaySFX("PistolShot");
        Debug.Log(GetItem.transform.GetChild(0).name);
        weaponPos.position = GetItem.transform.GetChild(0).position;

        // 마우스 위치를 가져와서 월드 좌표로 변환
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 현재 오브젝트 위치에서 마우스 방향으로의 벡터 계산
        Vector2 rayDirection = (mousePosition - (Vector2)weaponPos.position).normalized;

        // RaycastHit2D는 레이가 충돌한 오브젝트의 정보를 담습니다.
        RaycastHit2D hit = Physics2D.Raycast(weaponPos.position, rayDirection, rayDistance, targetLayer);

        // 레이가 무언가에 충돌했다면
        if (hit.collider)
        {
            Debug.DrawRay(weaponPos.position, rayDirection * rayDistance, Color.blue);
            // 충돌한 오브젝트의 이름 출력
            Debug.Log("충돌한 오브젝트: " + hit.collider.name);
        }

        // 디버그용 레이 시각화 (Scene 창에서 레이 경로를 확인)
        Debug.DrawRay(weaponPos.position, rayDirection * rayDistance, Color.red);

        //총알 발사
        //GameObject bullet = GetBulletFromPool();
        //if (bullet != null)
        //{
        //    bullet.transform.position = weaponPos.position;
        //    bullet.transform.rotation = weaponPos.rotation;
        //    bullet.SetActive(true);
        //}
    }

    void FireShotGun()
    {
        SoundManager.instance.PlaySFX("ShotGunShot");
        weaponPos.position = GetItem.transform.GetChild(0).transform.position;

        for (int i = 0; i < pelletCount; i++)
        {
            //퍼지는 각도를 계산
            float randomAngle = Random.Range(-spreaAngle / 2f, spreaAngle / 2f);
            Vector2 spreadDirection = Quaternion.Euler(0, 0, randomAngle) * rayDirection;

            RaycastHit2D hit = Physics2D.Raycast(weaponPos.position, spreadDirection, rayDistance, targetLayer);

            Debug.DrawRay(weaponPos.position, spreadDirection * rayDistance, Color.red, 0.5f);

            if (hit.collider != null)
            {
                Rigidbody2D rigid = hit.collider.GetComponent<Rigidbody2D>();

                if (rigid != null)
                {
                    Vector2 knockbackDirection = (hit.collider.transform.position - weaponPos.position).normalized;
                    rigid.AddForce(knockbackDirection * knockbakForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    //카메라 쉐이크(화면 흔들기)
    public IEnumerator Shake(float duration, float magnitude)
    {
        originalPos = Camera.main.transform.localPosition;
        CameraManager.instance.isShake = true;

        if (Camera.main == null)
        {
            Debug.Log("MainCamera가 없습니다.");
            yield break;
        }

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1f) * magnitude;
            float y = Random.Range(-1, 1f) * magnitude;

            mainCamera.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, -10);

            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.localPosition = originalPos;
        CameraManager.instance.isShake = false;
    }

    public void ReturnBulletToPool(GameObject bullet) //총알을 처음으로 되돌리기위한 함수
    {
        bullet.SetActive(false);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }
}