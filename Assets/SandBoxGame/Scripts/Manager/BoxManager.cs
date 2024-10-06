using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public GameObject boxPrefab; // ���� ������
    public List<GameObject> itemPrefabs; // ���빰 ������ ����Ʈ
    public int numberOfBoxes = 5; // ������ ���� ��
    public GameObject background; // ��� ������Ʈ

    private float spawnAreaWidth; // ������ ������ ��
    private float spawnAreaHeight; // ������ ������ ����

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
        // ����� ũ�⸦ �����ͼ� ���� ������ ����
        if (background != null)
        {
            spawnAreaWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
            spawnAreaHeight = background.GetComponent<SpriteRenderer>().bounds.size.y;
        }
        else
        {
            Debug.LogWarning("��� ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
            return; // ����� ������ �� �̻� �������� ����
        }

        for (int i = 0; i < numberOfBoxes; i++)
        {
            SpawnBox();
        }
    }

    void SpawnBox()
    {
        // ���� ��ġ ����
        Vector2 randomPosition = new Vector2(
            Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
            Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2)
        );

        // ���� ����
        GameObject newBox = Instantiate(boxPrefab, randomPosition, Quaternion.identity);

        // ���� �ȿ� ���� ���빰 ����
        SpawnItemInBox(newBox);
    }

    void SpawnItemInBox(GameObject box)
    {
        // ���빰 ������ ����Ʈ�� ������� ������ Ȯ��
        if (itemPrefabs.Count > 0)
        {
            // ���� ���빰 ����
            int randomIndex = Random.Range(0, itemPrefabs.Count);
            GameObject item = Instantiate(itemPrefabs[randomIndex]);

            // ���� �ȿ� ���빰 ��ġ
            item.transform.SetParent(box.transform);
            item.transform.localPosition = Vector3.zero; // ���� �� �߾ӿ� ��ġ
        }
        else
        {
            Debug.LogWarning("������ ������ ����Ʈ�� ����ֽ��ϴ�.");
        }
    }

    void DropItem(GameObject item)
    {
        //Instantiate(dropItem, transform.position - new Vector3(0, 0.8f, 0), Quaternion.identity);
    }
}