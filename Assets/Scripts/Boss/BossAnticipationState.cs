using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnticipationState : BossBaseState
{

    FirstBossController boss;
    PlayerController player;
    float TimeStart;
    //
    [Tooltip(" n frames accelerazioine , è ancora in sec")]
    public float AnticipationTime;
    public BossBaseState ExitState;
    public Material ColorAnticipation;
    public Material BossColor;


    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Enter(FirstBossController _boss, PlayerController _player)
    {
        boss = _boss;
        player = _player;
        TimeStart = 99999;
        Debug.Log("Anticipation");
        TimeStart = Time.time;
        boss.GetComponent<MeshRenderer>().material = ColorAnticipation;

    }
    public override void Tick()
    {

     
     
        if ((Time.time - TimeStart) > AnticipationTime)
        {
            boss.GetComponent<MeshRenderer>().material = BossColor;
            boss.ChangeState(ExitState);
        }
        

    }
    public override void Exit()
    {

    }

}