using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public GameObject boxPrefab; // 상자 프리팹
    public List<GameObject> itemPrefabs; // 내용물 프리팹 리스트
    public int numberOfBoxes = 5; // 생성할 상자 수
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


        // 상자 생성
        GameObject newBox = Instantiate(boxPrefab, new Vector3(boxSpawnPos[randomSpawnPos].transform.position.x,
                boxSpawnPos[randomSpawnPos].transform.position.y, 0), Quaternion.identity);

        // 상자 안에 랜덤 내용물 생성
        SpawnItemInBox(newBox);
    }

    void SpawnItemInBox(GameObject box)
    {
        // 내용물 프리팹 리스트가 비어있지 않은지 확인
        if (itemPrefabs.Count > 0)
        {
            // 랜덤 내용물 선택
            int randomIndex = Random.Range(0, itemPrefabs.Count);
            GameObject item = Instantiate(itemPrefabs[randomIndex]);

            // 상자 안에 내용물 배치
            item.transform.SetParent(box.transform);
            item.transform.localPosition = Vector3.zero; // 상자 안 중앙에 배치
        }
        else
        {
            Debug.LogWarning("아이템 프리팹 리스트가 비어있습니다.");
        }
    }

    void DropItem(GameObject item)
    {
        //Instantiate(dropItem, transform.position - new Vector3(0, 0.8f, 0), Quaternion.identity);
    }
}