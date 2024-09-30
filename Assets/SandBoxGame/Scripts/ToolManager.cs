using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolManager : MonoBehaviour
{
    public static ToolManager instance;

    public GameObject getItem;
    private Vector2 direction;
    public LayerMask toolMask;
    private Vector2 playerPos;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Inventory theInventory;

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
    }

    void Update()
    {
        Debug.DrawLine(transform.position, direction, Color.red, 1.0f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.5f, toolMask.value);
        playerPos = PlayerManager.Instance.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        HandRotation();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E 버튼 클릭");

            if (hit.collider != null && hit.collider.tag != "Player")
            {
                Debug.Log(hit.collider.GetComponent<ItemPickUp>().item.itemName + " 획득 했습니다.");
                theInventory.AcquireItem(hit.collider.GetComponent<ItemPickUp>().item);  // 인벤토리 넣기
                Destroy(hit.collider.gameObject);
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
            spriteRenderer.sprite = getItem.GetComponent<SpriteRenderer>().sprite;
        }
    }

    void HandRotation()
    {
        if( PlayerManager.Instance.move.x > 0)
        {
            transform.position = new Vector3(playerPos.x + 0.3f, playerPos.y, 0);
            direction = transform.right;
            spriteRenderer.flipX = false;
        }
        else if (PlayerManager.Instance.move.x < 0)
        {
            transform.position = new Vector3(playerPos.x - 0.3f, playerPos.y, 0);
            direction = -transform.right;

            if (getItem != null)
            {
                spriteRenderer.flipX = true;
            }
        }
        else if (PlayerManager.Instance.move.y > 0)
        {
            transform.position = new Vector3(playerPos.x + 0.4f, playerPos.y, 0);
            direction = transform.up;
            spriteRenderer.flipX = false;
        }
        else if (PlayerManager.Instance.move.y < 0)
        {
            transform.position = new Vector3(playerPos.x - 0.4f, playerPos.y, 0);
            direction = -transform.up;
            spriteRenderer.flipX = true;
        }
    }
}
