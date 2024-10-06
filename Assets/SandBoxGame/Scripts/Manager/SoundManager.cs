using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("-------Sound System-------")]
    public AudioSource BGMaudioSource; //�������
    public AudioSource SFXaudioSource; //ȿ����

    //���带 �̸����� ���� �� �� �ֵ��� Dic ���
    Dictionary<string, AudioClip> BGMClips = new Dictionary<string, AudioClip>();
    Dictionary<string, AudioClip> SFXClips = new Dictionary<string, AudioClip>();

    //Inspector���� ���� Ŭ���� �߰��� �� �ֵ��� ����Ʈ ����
    [System.Serializable]
    public struct NamedAudio
    {
        public string name;
        public AudioClip clip;
    }

    public NamedAudio[] BGMClipList;
    public NamedAudio[] SFXClipList;

    private Coroutine currentBGMCoroutine; //���� �������� �ڷ�ƾ �����ϴ� ����
    private string nextSceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializedAudioClips();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Audio Clip ����Ʈ�� Dic�� ��ȯ�Ͽ� �̸����� ���� �����ϰ� ����� �Լ�
    private void InitializedAudioClips()
    {
        foreach (var bgm in BGMClipList) //�����
        {
            if (!BGMClips.ContainsKey(bgm.name))
            {
                BGMClips.Add(bgm.name, bgm.clip);
            }
        }

        foreach (var sfx in SFXClipList) //ȿ����
        {
            if (!SFXClips.ContainsKey(sfx.name))
            {
                SFXClips.Add(sfx.name, sfx.clip);
            }
        }
    }

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name; //�� �̸��� ������
    }

    public void OnSceneLoaded(string sceneName) //�� �̸��� �޾Ƽ� BGM�� �����ϴ� �Լ�
    {
        //�� �̸��� ���� ������� ����ϴ� �ڵ�
        if (sceneName == "GameScene")
        {
            PlayBGM("Background", 1f);
        }
        else
        {
            StopBGM();
        }
    }

    public void PlayBGM(string clipName, float fadeDuration = 1f) //����� ����Լ�
    {
        if (BGMClips.ContainsKey(clipName))
        {
            if (currentBGMCoroutine != null)
            {
                StopCoroutine(currentBGMCoroutine); //���� ���̵� �ڷ�ƾ�� ������ �ߴ��ϴ� �Լ�
            }

            //���� BGM�� �ִ� ��� ���̵�ƿ� �� ���ο� BGM���� ���̵���
            //���ٸ� ����� BGM�� ���
            currentBGMCoroutine = StartCoroutine(FadeOutBGM(fadeDuration, () =>
            {
                BGMaudioSource.clip = BGMClips[clipName];
                BGMaudioSource.Play();
                currentBGMCoroutine = StartCoroutine(FadeInBGM(fadeDuration));
            }));
        }
        else
        {
            Debug.LogWarning("�ش� �̸��� ������� �������� �ʽ��ϴ�. : " + clipName);
        }
    }

    public void PlaySFX(string clipName) //ȿ���� ����Լ�
    {
        if (SFXClips.ContainsKey(clipName))
        {
            SFXaudioSource.PlayOneShot(SFXClips[clipName]);
        }
        else
        {
            Debug.LogWarning("�ش� �̸��� ȿ������ �������� �ʽ��ϴ�. : " + clipName);
        }
    }

    public void StopBGM() //����� ���ߴ� �Լ�
    {
        BGMaudioSource.Stop();
    }

    public void StopSFX() //ȿ���� ���ߴ� �Լ�
    {
        SFXaudioSource.Stop();
    }

    public void SetBGMVolume(float volume) //����� ���� �����ϴ� �Լ�
    {
        BGMaudioSource.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
    }
    public void SetSFXVolume(float volume) //ȿ���� ���� �����ϴ� �Լ�
    {
        SFXaudioSource.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
    }

    //BGM�� ���̵�ƿ� ��Ű�� �ڷ�ƾ
    private IEnumerator FadeOutBGM(float duration, Action onFadeComplete)
    {
        float startVolume = BGMaudioSource.volume; //BGM�� ���纼������ ������

        for (float t = 0; t < duration; t += Time.deltaTime) //���� �Ҹ��� �۾���
        {
            BGMaudioSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        BGMaudioSource.volume = 0f;
        onFadeComplete?.Invoke(); //���̵� �ƿ��� �Ϸ�Ǹ� ���� �۾� ����
    }

    //BGM�� ���̵��� ��Ű�� �ڷ�ƾ
    private IEnumerator FadeInBGM(float duration)
    {
        float startVolume = 0f;
        BGMaudioSource.volume = 0f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            BGMaudioSource.volume = Mathf.Lerp(startVolume, 1f, t / duration);
            //Mathf.Lerp : �� float �� ������ ������ float ���
            yield return null;
        }
        BGMaudioSource.volume = 1f;
    }
}