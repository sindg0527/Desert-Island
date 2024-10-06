using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialsManager : MonoBehaviour
{
    public GameObject tutorialPanel; // 안내 메시지를 표시할 UI 패널
    public Text tutorialText; // 안내 메시지를 표시할 Text 요소
    public string message; // 표지판이 표시할 메시지

    private void Start()
    {
        tutorialPanel.SetActive(false); // 시작 시 패널 비활성화
    }

    // 상호작용 메소드
    public void Interact()
    {
        ShowTutorial(message);
    }

    public void ShowTutorial(string message)
    {
        tutorialText.text = message; // 안내 메시지를 설정
        tutorialPanel.SetActive(true); // 패널 활성화

        // 자동으로 안내 메시지를 숨기는 코루틴 시작
        StartCoroutine(HideTutorialAfterDelay(5f)); // 5초 후에 자동으로 숨김
    }

    private IEnumerator HideTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간 대기
        HideTutorial(); // 안내 메시지 숨기기
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false); // 패널 비활성화
    }
}
