using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleHurt : MonoBehaviour
{
    public float continueTime = 5f;
    private float timer = 0f;
    
    // Start is called before the first frame update
   public void onClickButton()
    {
        timer = 0f;
        // ·¢ËÍÐÅÏ¢
        MessageCenter.SendMessage(new CommonMessage(), MESSAGE_TYPE.SkillA);
        CoinManager.Instance.SpendCoin(500);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > continueTime) {
            MessageCenter.SendMessage(new CommonMessage(), MESSAGE_TYPE.SkillAEnd);
        }
        timer = 0f;
    }
}
