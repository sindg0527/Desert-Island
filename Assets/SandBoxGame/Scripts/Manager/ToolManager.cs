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

    public int poolSize = 10; //������Ʈ Ǯ�� ������
    private List<GameObject> bulletPool; //������Ʈ Ǯ ����Ʈ
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
            Debug.Log("E ��ư Ŭ��");

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Item")
                {
                    Debug.Log(hit.collider.GetComponent<ItemPickUp>().item.itemName + " ȹ�� �߽��ϴ�.");
                    theInventory.AcquireItem(hit.collider.GetComponent<ItemPickUp>().item);  // �κ��丮 �ֱ�
                    Destroy(hit.collider.gameObject);
                }

                if (hit.collider.tag == "Tree") //�ش� ������Ʈ�� ������ ��
                {
                    if (getItem != null) //��� �����ϰ� �ִ��� Ȯ��
                    {
                        if (getItem.name == "Axe") //������ �����ϰ� �ִٸ� ����
                        {
                            hit.collider.GetComponent<TreeManager>().CutTree();
                        }
                    }
                }

                if (hit.collider.tag == "Furniture") //�ش� ������Ʈ�� ������ ��
                {
                    if (getItem == null)
                    {                        
                        GameManager.Instance.CraftingTableOn(); //���۴� ����
                    }
                    else if (getItem.name == "Axe") //������ �����ϰ� �ִٸ� ����
                    {
                        Debug.Log(hit.collider.GetComponent<ItemPickUp>().item.itemName + " ȹ�� �߽��ϴ�.");
                        theInventory.AcquireItem(hit.collider.GetComponent<ItemPickUp>().item);  // �κ��丮 �ֱ�
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
        Debug.Log("�߻�");
    }

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

    void BulletPool() //�Ѿ� �߻� ��ġ
    {
        GameObject bullet = GetBulletFromPool();
        if (bullet != null)
        {
            bullet.transform.position = weaponPos.position;
            bullet.transform.rotation = weaponPos.rotation;
            bullet.SetActive(true);
        }
    }

    public void ReturnBulletToPool(GameObject bullet) //�Ѿ��� ó������ �ǵ��������� �Լ�
    {
        bullet.SetActive(false);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }
}
