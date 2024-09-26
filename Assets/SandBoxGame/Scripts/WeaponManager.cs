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

    public int poolSize = 100; //������Ʈ Ǯ�� ������
    private List<GameObject> bulletPool; //������Ʈ Ǯ ����Ʈ

    public bool IsFire = false; //���� ��� ����
    private float timer; //�ð����庯��
    private float delayTime = 0.2f; //���� ��� �ٽ� ��µ����� �ɸ��� �ð�
    //private int bulletCount = 0; //�Ѿ� ��� Ƚ��
    //private int maxBulletCount = 3; //��� ������ �Ѿ� ��

    public float rayDistance = 100f;
    public LayerMask targetLayer;
    Vector2 rayDirection; //������ ����

    [Header("Weapon Settings")]
    public bool IsGetPistol = false;
    public bool IsGetShotGun = false;

    [Header("Shotgun Settings")]
    public int pelletCount = 10; //���� źȯ ����
    public float spreaAngle = 30; //���� źȯ�� ������ ����
    public float knockbakForce = 10f; //�浹�� ������Ʈ�� �о�� ���� ũ��

    private int pistolBulletCount = 100;
    private int shotGunBulletCount = 100;
    private int maxBulletCount = 100;

    [Header("CameraShake Settings")]
    public float shakeDuration = 0.5f; //��鸲 ���ӽð�
    public float shakeMagnitude = 0.1f; //��鸲 ����
    private Vector3 originalPos; //ī�޶� ���� ��ġ

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
        for (int i = 0; i < poolSize; i++) //��Ȱ��ȭ�� �Ѿ� �̸� ����
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
            Debug.Log("���� ī�޶� ã�� �� �����ϴ�. MainCamera Tag�� Ȯ�����ּ���.");
        }
    }

    void Update()
    {
        //���콺 ��ġ ������Ʈ �� ���� ��ǥ�� ��ȯ
        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position; //���콺 ����

        //�Ѿ� �߻� ��ġ���� ���콺 ���������� ���� ���
        rayDirection = (mousePos - transform.position).normalized;

        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        //Atan() : �Լ��� ��ǥ��鿡�� ���������κ��� �� �������� ������ ���ϴ� �Լ�

        transform.rotation = Quaternion.Euler(0, 0, rotationZ); //�ѱ��� ���콺�� ���� �� �ְ� ���� ����
        //Debug.DrawRay(bulletPos.position, rayDirection * rayDistance, Color.green); //������ �ǽð����� ���� Ȯ��

        if (!IsFire) //���� ���� �ʾ��� ��
        {
            timer += Time.deltaTime;
            if (timer > delayTime) //timer�� ������Ÿ���� �Ѿ��� �� �� �߻� ����
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
                SoundManager.instance.PlaySFX("WeaponEquip"); //������ ���� �� ��ü �Ҹ�
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

    //Ǯ���� ��Ȱ��ȭ�� �Ѿ��� �������� �Լ�
    GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy) //�ڽŰ� �θ� Ȱ��ȭ�� �ƴҰ��
            {
                return bullet;
            }
        }
        //Ǯ�� ����� �� �ִ� �Ѿ��� ������ Null ��ȯ
        return null;
    }

    void OnFire() //�� �߻�
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
                Debug.Log("���� ����");
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

        // ���콺 ��ġ�� �����ͼ� ���� ��ǥ�� ��ȯ
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // ���� ������Ʈ ��ġ���� ���콺 ���������� ���� ���
        Vector2 rayDirection = (mousePosition - (Vector2)weaponPos.position).normalized;

        // RaycastHit2D�� ���̰� �浹�� ������Ʈ�� ������ ����ϴ�.
        RaycastHit2D hit = Physics2D.Raycast(weaponPos.position, rayDirection, rayDistance, targetLayer);

        // ���̰� ���𰡿� �浹�ߴٸ�
        if (hit.collider)
        {
            Debug.DrawRay(weaponPos.position, rayDirection * rayDistance, Color.blue);
            // �浹�� ������Ʈ�� �̸� ���
            Debug.Log("�浹�� ������Ʈ: " + hit.collider.name);
        }

        // ����׿� ���� �ð�ȭ (Scene â���� ���� ��θ� Ȯ��)
        Debug.DrawRay(weaponPos.position, rayDirection * rayDistance, Color.red);

        //�Ѿ� �߻�
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
            //������ ������ ���
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

    //ī�޶� ����ũ(ȭ�� ����)
    public IEnumerator Shake(float duration, float magnitude)
    {
        originalPos = Camera.main.transform.localPosition;
        CameraManager.instance.isShake = true;

        if (Camera.main == null)
        {
            Debug.Log("MainCamera�� �����ϴ�.");
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

    public void ReturnBulletToPool(GameObject bullet) //�Ѿ��� ó������ �ǵ��������� �Լ�
    {
        bullet.SetActive(false);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }
}