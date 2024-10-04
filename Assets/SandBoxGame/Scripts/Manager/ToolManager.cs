using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    public static ToolManager instance;

    public Item getItem;
    private Vector2 direction;
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
            spriteRenderer.sprite = getItem.itemImage;

            if(getItem.name == "Rifle")
            {
                //weaponPos = getItem.itemPrefab.transform.GetChild(0).transform;
                //BulletPool();
            }
        }
        else
        {
            spriteRenderer.sprite = null;
        }
    }

    void HandRotation()
    {
        if( PlayerManager.Instance.direction == PlayerManager.PlayerDirection.right)
        {
            transform.position = new Vector3(playerPos.x + 0.3f, playerPos.y, 0);
            direction = transform.right;
            spriteRenderer.flipX = false;
        }
        else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.left)
        {
            transform.position = new Vector3(playerPos.x - 0.3f, playerPos.y, 0);
            direction = -transform.right;

            if (getItem != null)
            {
                spriteRenderer.flipX = true;
            }
        }
        else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.up)
        {
            transform.position = new Vector3(playerPos.x + 0.4f, playerPos.y, 0);
            direction = transform.up;
            spriteRenderer.flipX = false;
        }
        else if (PlayerManager.Instance.direction == PlayerManager.PlayerDirection.down)
        {
            transform.position = new Vector3(playerPos.x - 0.4f, playerPos.y, 0);
            direction = -transform.up;
            spriteRenderer.flipX = true;
        }
    }

    void OnFire()
    {
        Debug.Log("발사");
    }

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

    void BulletPool() //총알 발사 위치
    {
        GameObject bullet = GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = weaponPos.position;
            bullet.transform.rotation = weaponPos.rotation;
            bullet.SetActive(true);
        }
    }

    public void ReturnBulletToPool(GameObject bullet) //총알을 처음으로 되돌리기위한 함수
    {
        bullet.SetActive(false);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }
}
