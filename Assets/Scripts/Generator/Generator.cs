using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class Generator : MonoBehaviour
{
    [Header("位置设定")]
    [SerializeField]private Transform genPointLeft;
    [SerializeField]private Transform genPointRight;
    [Header("计时器")]
    private float timer = 0f;
    [SerializeField]private float scale = 1f;
    private float event_timer;
    private float prob = 0.3f;
    private float event_interval = 55f;
    private float event_duration = 5f;
    private int genCount;
    [SerializeField]
    private float factor = 0.04f;
    private bool event_triggered = false;
    [Header("数据存储")] [SerializeField] private BattleUnitData normal;
    [SerializeField]private BattleUnitData varient;
    [SerializeField]private BattleUnitData ultra;
    private float impluse_timer = 0f;
    [SerializeField] private TMP_Text text;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale += 0.5f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale -= 0.5f;
        }
        if (Math.Abs(timer - float.MaxValue) < 1e-5)
        {
            return;
        }
        timer += Time.deltaTime * scale;
        text.text = "Time: " + Mathf.FloorToInt(timer) + "s";
        impluse_timer += Time.deltaTime * scale;
        if (impluse_timer >= 1f)
        {
            impluse_timer = 0f;
            prob = f(timer);
             // 生成多少个怪物
             genCount = Mathf.FloorToInt(timer * factor) < 1 ? 1 : Mathf.FloorToInt(timer * factor);
             // 怪物当中有多少个变异
             int varCount = Mathf.FloorToInt(genCount * prob);
             // 究极怪物事件
             if (event_triggered == false)
             {
                 event_timer += Time.deltaTime;
                 if (event_timer >= event_interval)
                 {
                     event_timer = 0f;
                     event_triggered = true;
                 }
                 //生成varCount个变异怪物
                 for (int i = 0; i < varCount; i++)
                 {
                     Generate(1);
                 }
                 //生成genCount-varCount个普通怪物
                 for (int i = 0;i < genCount - varCount; i++)
                 {
                     Generate(0);
                 }
             }
             else
             {
                 event_timer += Time.deltaTime;
                 if (event_timer >= event_duration)
                 {
                     event_timer = 0f;
                     event_triggered = false;
                 }
                 int ultraCount = Mathf.FloorToInt(genCount * 0.1f);
                 //生成varCount个变异怪物
                 for (int i = 0; i < varCount; i++)
                 {
                     Generate(1);
                 }
                 //生成ultraCount个究极怪物
                 for (int i = 0; i < ultraCount; i++)
                 {
                     Generate(2);
                 }
                 //生成genCount-varCount-ultraCount个普通怪物
                 for (int i = 0; i < genCount - varCount; i++)
                 {
                     Generate(3);
                 }
             }   
        }
        
    }

    private float f(float t)
    {
        return 0.3f;
    }

    public void Generate(int type)
    {
        //根据type选择不同的FSMConfigSO
        BattleUnitData so;
        if (type == 0)
        {
            so = normal;
        }else if (type == 1)
        {
            so = varient;
        }
        else
        {
            so = ultra;
        }
        //生成位置在genPointLeft和genPointRight之间随机
        Vector3 genPos = new Vector3(UnityEngine.Random.Range(genPointLeft.position.x, genPointRight.position.x), genPointLeft.position.y, 0f);
        var trans = ObjectPoolRegister.Instance._objectPool.Spawn(PoolName.Monster.ToString(), genPos, Quaternion.identity, so);
        trans.localScale = new Vector3(3, 3, 3);
        //微调z轴坐标
        trans.position = new Vector3(trans.position.x,trans.position.y,UnityEngine.Random.Range(-0.1f,0.1f));
    }
}
