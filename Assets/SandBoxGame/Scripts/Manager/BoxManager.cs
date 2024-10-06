using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public GameObject boxPrefab; // 상자 프리팹
    public List<GameObject> itemPrefabs; // 내용물 프리팹 리스트
    public int numberOfBoxes = 5; // 생성할 상자 수
    public GameObject background; // 배경 오브젝트

    private float spawnAreaWidth; // 생성할 영역의 폭
    private float spawnAreaHeight; // 생성할 영역의 높이

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
        // 배경의 크기를 가져와서 생성 영역을 설정
        if (background != null)
        {
            spawnAreaWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
            spawnAreaHeight = background.GetComponent<SpriteRenderer>().bounds.size.y;
        }
        else
        {
            Debug.LogWarning("배경 오브젝트가 할당되지 않았습니다.");
            return; // 배경이 없으면 더 이상 진행하지 않음
        }

        for (int i = 0; i < numberOfBoxes; i++)
        {
            SpawnBox();
        }
    }

    void SpawnBox()
    {
        // 랜덤 위치 생성
        Vector2 randomPosition = new Vector2(
            Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2),
            Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2)
        );

        // 상자 생성
        GameObject newBox = Instantiate(boxPrefab, randomPosition, Quaternion.identity);

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