using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialsManager : MonoBehaviour
{
    public GameObject tutorialPanel; // �ȳ� �޽����� ǥ���� UI �г�
    public Text tutorialText; // �ȳ� �޽����� ǥ���� Text ���
    public string message; // ǥ������ ǥ���� �޽���

    private void Start()
    {
        tutorialPanel.SetActive(false); // ���� �� �г� ��Ȱ��ȭ
    }

    // ��ȣ�ۿ� �޼ҵ�
    public void Interact()
    {
        ShowTutorial(message);
    }

    public void ShowTutorial(string message)
    {
        tutorialText.text = message; // �ȳ� �޽����� ����
        tutorialPanel.SetActive(true); // �г� Ȱ��ȭ

        // �ڵ����� �ȳ� �޽����� ����� �ڷ�ƾ ����
        StartCoroutine(HideTutorialAfterDelay(5f)); // 5�� �Ŀ� �ڵ����� ����
    }

    private IEnumerator HideTutorialAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // ������ �ð� ���
        HideTutorial(); // �ȳ� �޽��� �����
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false); // �г� ��Ȱ��ȭ
    }
}
