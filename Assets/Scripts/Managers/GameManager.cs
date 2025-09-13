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

    public void GameRestartBtn()
    {
        if (gameOver)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
            gameOver = false;
            ObjectPoolRegister.Instance.RemoveCache();
            SoundManager.WhenSceneReload();
            ObjectPoolRegister.Instance.RemoveAllPool();
        }
    }

    public void GameOver(CommonMessage msg)
    {
        if (gameOver == false)
        {
            Time.timeScale = 0;
            gameOver = true;
        }
        
    }
}
