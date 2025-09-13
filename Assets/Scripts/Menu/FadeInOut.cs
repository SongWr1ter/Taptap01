using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image title;
    public float fadeDuration = 2f;
    public Button start;
    public Button quit;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeIn()
    {
        
        Time.timeScale = 1.0f;
        title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
        StartCoroutine(FadeInCoroutine());

    }
    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0;

        Color startColor = title.color; // ��ʼ��ɫ����ȫ͸����
        Color endColor = title.color; // ������ɫ����͸����
        endColor.a = 1; // ���ý�����ɫ��͸����Ϊ��͸��
        Debug.Log("FadeIn");
        while (elapsedTime < fadeDuration)
        {
            Time.timeScale = 1.0f;
            Debug.Log($"Time before loop: {Time.time}");
            Debug.Log($"Delta Time: {Time.deltaTime}");
            // ���㵱ǰ͸����
            float alpha = elapsedTime / fadeDuration;
            Debug.Log($"Alpha: {alpha}");
            Debug.Log($"elapsedTime: {elapsedTime}");
            title.color = Color.Lerp(startColor, endColor, alpha);
            elapsedTime += Time.deltaTime;
            Debug.Log("FadeIn2");
            // �ȴ���һ֡
            yield return null;
        }

        // ȷ����ɫ��ȫ��Ϊ��͸��
        title.color = endColor;
    }
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());

    }
    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0;
        Color startColor = title.color; // ��ʼ��ɫ����͸����
        Color endColor = title.color; // ������ɫ����ȫ͸����
        endColor.a = 0; // ���ý�����ɫ��͸����Ϊ��ȫ͸��

        while (elapsedTime < fadeDuration)
        {
            // ���㵱ǰ͸����
            float alpha = elapsedTime / fadeDuration;
            title.color = Color.Lerp(startColor, endColor, alpha);
            elapsedTime += Time.deltaTime;

            // �ȴ���һ֡
            yield return null;
        }

        // ȷ����ɫ��ȫ��Ϊ��ȫ͸��
        title.color = endColor;

    }
    private IEnumerator FadeSequence()
    {
        // �ȵ���
        yield return StartCoroutine(FadeInCoroutine());

        // �ٵ���
        yield return StartCoroutine(FadeOutCoroutine());

        // ��ť����
        start.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }
}
