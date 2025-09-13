using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void OnEnable()
    {
        MessageCenter.AddListener(OnGameOver,MESSAGE_TYPE.GAME_OVER);
    }

    private void OnDisable()
    {
        MessageCenter.RemoveListener(OnGameOver,MESSAGE_TYPE.GAME_OVER);
    }

    public void OnGameOver(CommonMessage msg)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
