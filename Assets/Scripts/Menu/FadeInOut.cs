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

        Color startColor = title.color; // 起始颜色（完全透明）
        Color endColor = title.color; // 结束颜色（不透明）
        endColor.a = 1; // 设置结束颜色的透明度为不透明
        Debug.Log("FadeIn");
        while (elapsedTime < fadeDuration)
        {
            Time.timeScale = 1.0f;
            Debug.Log($"Time before loop: {Time.time}");
            Debug.Log($"Delta Time: {Time.deltaTime}");
            // 计算当前透明度
            float alpha = elapsedTime / fadeDuration;
            Debug.Log($"Alpha: {alpha}");
            Debug.Log($"elapsedTime: {elapsedTime}");
            title.color = Color.Lerp(startColor, endColor, alpha);
            elapsedTime += Time.deltaTime;
            Debug.Log("FadeIn2");
            // 等待下一帧
            yield return null;
        }

        // 确保颜色完全变为不透明
        title.color = endColor;
    }
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());

    }
    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0;
        Color startColor = title.color; // 起始颜色（不透明）
        Color endColor = title.color; // 结束颜色（完全透明）
        endColor.a = 0; // 设置结束颜色的透明度为完全透明

        while (elapsedTime < fadeDuration)
        {
            // 计算当前透明度
            float alpha = elapsedTime / fadeDuration;
            title.color = Color.Lerp(startColor, endColor, alpha);
            elapsedTime += Time.deltaTime;

            // 等待下一帧
            yield return null;
        }

        // 确保颜色完全变为完全透明
        title.color = endColor;

    }
    private IEnumerator FadeSequence()
    {
        // 先淡入
        yield return StartCoroutine(FadeInCoroutine());

        // 再淡出
        yield return StartCoroutine(FadeOutCoroutine());

        // 按钮出现
        start.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
    }
}
