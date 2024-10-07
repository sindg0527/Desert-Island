using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public GameObject boxPrefab; // ���� ������
    public List<GameObject> itemPrefabs; // ���빰 ������ ����Ʈ
    public int numberOfBoxes = 5; // ������ ���� ��
    public Transform[] boxSpawnPos;
    Dictionary<Transform, bool> boxPos = new Dictionary<Transform, bool>();

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
        for (int i = 0; i < numberOfBoxes; i++)
        {
            SpawnBox();
        }
    }

    void SpawnBox()
    {
        int randomSpawnPos = Random.Range(0, 5);


        // ���� ����
        GameObject newBox = Instantiate(boxPrefab, new Vector3(boxSpawnPos[randomSpawnPos].transform.position.x,
                boxSpawnPos[randomSpawnPos].transform.position.y, 0), Quaternion.identity);

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