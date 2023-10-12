using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class BornPot : MonoBehaviour {
    public Transform TargetTF; //要跟随的目标
    //该出生点生成的怪物
    public GameObject targetEnemy;
    //生成怪物的总数量
    public int enemyTotalNum = 10;
    //生成怪物的时间间隔
    public float intervalTime = 3;
    //生成怪物的计数器
    private int enemyCounter;
    //玩家
    private GameObject targetPlayer;
    //生成怪物的计数器
 
    // Use this for initialization
    void Start () {
        //玩家
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        //初始时，怪物计数为0；
        enemyCounter = 0;
        //重复生成怪物
        InvokeRepeating("CreatEnemy", 0.5F, intervalTime);
    }
	
    // Update is called once per frame
    void Update () {
		
    }
    //方法，生成怪物
    private void CreatEnemy()
    {
        //如果玩家存活
        if (TargetTF)
        {
            //生成一只怪物
            Instantiate(targetEnemy, this.transform.position, Quaternion.identity);
            enemyCounter++;
            //如果计数达到最大值
            if (enemyCounter == enemyTotalNum)
            {
                //停止刷新
                CancelInvoke();
            }
        }
        //玩家死亡
        else
        {
            //停止刷新
            CancelInvoke();
        }
    }
}