using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class ToolManager : MonoBehaviour
{
    public static ToolManager instance;

    [Header("Gun Settings")]
    public bool isFire = false; //총을 쏘는 여부
    private Camera mainCamera;
    private Vector3 mousePos;
    Vector2 rayDirection; //마우스 레이의 방향
    private float timer; //시간저장변수
    private float delayTime = 0.2f;

    public Item getItem;
    private Vector2 direction; //캐릭터 기준 레이의 방향
    public LayerMask toolMask;
    private Vector2 playerPos;
    private SpriteRenderer spriteRenderer;

    public int poolSize = 10; //오브젝트 풀링 사이즈
    private List<GameObject> bulletPool; //오브젝트 풀 리스트
    public GameObject bulletObj;
    public Transform weaponPos = null;

    [SerializeField]
    public Inventory theInventory;

    void Awake()
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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        bulletPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletObj);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    void Update()
    {
        Debug.DrawRay(PlayerManager.Instance.transform.position, direction, Color.red, 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(PlayerManager.Instance.transform.position, direction, 0.5f, toolMask.value);
        playerPos = PlayerManager.Instance.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        HandRotation();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E 버튼 클릭");

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Item")
                {
                    Debug.Log(hit.collider.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                    theInventory.AcquireItem(hit.collider.GetComponent<ItemPickUp>().item);  // 인벤토리 넣기
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.tag == "Tree") //해당 오브젝트가 나무일 때
                {
                    if (getItem != null) //장비를 장착하고 있는지 확인
                    {
                        if (getItem.name == "Axe") //도끼를 장착하고 있다면 실행
                        {
                            hit.collider.GetComponent<TreeManager>().CutTree();
                        }
                    }
                }

                if (hit.collider.tag == "Furniture") //해당 오브젝트가 가구일 때
                {
                    if (getItem == null)
                    {                        
                        GameManager.Instance.CraftingTableOn(); //제작대 열기
                    }
                    else if (getItem.name == "Axe") //도끼를 장착하고 있다면 실행
                    {
                        Debug.Log(hit.collider.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                        theInventory.AcquireItem(hit.collider.GetComponent<ItemPickUp>().item);  // 인벤토리 넣기
                        Destroy(hit.collider.gameObject);
                    }

                }

                if (hit.collider.tag == "Box")
                {

                }

                if (hit.collider.tag == "Coin")
                {
                    GameManager.Instance.playerCoin += 100;
                    Destroy(hit.collider.gameObject);
                }
            }
        }

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mousePos - transform.position; //마우스 방향

        //총알 발사 위치에서 마우스 방향으로의 벡터 계산
        rayDirection = (mousePos - transform.position).normalized;

        float rotationZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        //Atan() : 함수는 좌표평면에서 수평축으로부터 한 점까지의 각도를 구하는 함수

        //transform.rotation = Quaternion.Euler(0, 0, rotationZ); //총구가 마우스를 따라갈 수 있게 각도 변경

        if (!isFire)
        {
            timer += Time.deltaTime;
            if (timer > delayTime)
            {
                isFire = true;
                timer = 0;
            }
        }
        //hit.collider.transform.SetParent(transform);
        //hit.collider.transform.localPosition = Vector3.zero;
        //GetItem = hit.collider.gameObject;
    }

    public void ToolSprite()
    {
        if (getItem != null)
        {
            if (getItem.weaponType == "Gun")
            {
                //weaponPos = getItem.itemPrefab.transform.GetChild(0).transform;
            }
            spriteRenderer.sprite = getItem.itemImage;
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }

    void HandRotation()
    {
        if (getItem == null || getItem.weaponType != "Gun")
        {
            if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.right)
            {
                transform.position = new Vector3(playerPos.x + 0.3f, playerPos.y, 0);
                direction = transform.right;
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.left)
            {
                transform.position = new Vector3(playerPos.x - 0.3f, playerPos.y, 0);
                direction = -transform.right;
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
            }
            else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.up)
            {
                transform.position = new Vector3(playerPos.x + 0.4f, playerPos.y, 0);
                direction = transform.up;
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.down)
            {
                transform.position = new Vector3(playerPos.x - 0.4f, playerPos.y, 0);
                direction = -transform.up;
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
            }
        }
        else
        {
            if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.right)
            {
                transform.position = new Vector3(playerPos.x + 0.3f, playerPos.y, 0);
                weaponPos.transform.position = new Vector3(playerPos.x + 0.65f, playerPos.y + 0.05f, 0);
                direction = transform.right;
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = false;
            }
            else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.left)
            {
                transform.position = new Vector3(playerPos.x - 0.3f, playerPos.y, 0);
                weaponPos.transform.position = new Vector3(playerPos.x + -0.65f, playerPos.y + 0.05f, 0);
                direction = -transform.right;
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
            }
            else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.up)
            {
                transform.position = new Vector3(playerPos.x + 0.3f, playerPos.y, 0);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                weaponPos.transform.position = new Vector3(playerPos.x + 0.36f, playerPos.y + 0.35f, 0);
                direction = transform.up;
                spriteRenderer.flipX = false;
                spriteRenderer.flipY = true;
            }
            else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.down)
            {
                transform.position = new Vector3(playerPos.x - 0.25f, playerPos.y - 0.1f, 0);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                weaponPos.transform.position = new Vector3(playerPos.x - 0.31f, playerPos.y - 0.45f, 0);
                direction = -transform.up;
                spriteRenderer.flipX = true;
                spriteRenderer.flipY = false;
            }
        }
    }

    void OnFire()
    {
        if (getItem != null)
        {
            if (isFire && getItem.weaponType == "Gun")
            {
                GameObject bullet = GetBulletFromPool();
                if (bullet != null)
                {
                    bullet.transform.position = weaponPos.position;
                    //bullet.transform.rotation = weaponPos.rotation;
                    bullet.SetActive(true);
                }
                isFire = false;
                SoundManager.instance.PlaySFX("Shot");
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

    public void ReturnBulletToPool(GameObject bullet) //총알을 처음으로 되돌리기위한 함수
    {
        bullet.SetActive(false);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }
}