using System;
using System.Collections;
using System.Collections.Generic;
using MemoFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    public bool gameOver = false;

    private void OnEnable()
    {
        MessageCenter.AddListener(GameOver,MESSAGE_TYPE.GAME_OVER);
    }

    private void OnDisable()
    {
        MessageCenter.RemoveListener(GameOver,MESSAGE_TYPE.GAME_OVER);
    }

    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1;
                gameOver = false;
                ObjectPoolRegister.Instance.RemoveCache();
            }
        }
    }

    public void GameOver(CommonMessage msg)
    {
        Time.timeScale = 0;
        gameOver = true;
        ObjectPoolRegister.Instance.RemoveAllPool();
    }
}
