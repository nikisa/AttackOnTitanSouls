using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossState : BaseState
{

    protected BossOrbitManager bossOrbitManager;
    protected FirstBossController boss;

    //SateMachine Parameters
    protected const string IDLE = "Idle";
    protected const string ANTICIPATION = "Anticipation";
    protected const string MOVETO = "MoveTo";
    protected const string RECOVERY = "Recovery";
    protected const string MASKS_COUNT = "MasksCount";
    protected const string DECELERATION = "Deceleration";
    protected const string END_STATE_TRIGGER = "EndState";
    protected const string TIMER = "Timer";

    float timer = 0;

    public override void Tick()
    {
        timer += Time.deltaTime;
        animator.SetFloat(TIMER, timer);
    }

    public override void Enter() {

        animator.SetFloat(TIMER, timer=0);

        BossOrbitManager.ChangedStateEvent();
    }


    public void SetContext(object context, Animator animator, BossOrbitManager bossOrbitManager)
    {
        //base.SetContext(context, animator , bossOrbitManager);
        boss = context as FirstBossController;
        this.animator = animator;
        this.bossOrbitManager = bossOrbitManager;

    }
    protected void TriggerExitState()
    {
        animator.SetTrigger(END_STATE_TRIGGER);
    }


    //Updates the MASK_COUNT SM Parameters when a Boss' Mask is detroyed
    public void CheckVulnerability() {
        animator.SetInteger(MASKS_COUNT, bossOrbitManager.HookPointList.Count);
    }

    //Set the tag to choose the next OrbitState
    public void OrbitTag(BaseData _baseData)
    {
        animator.SetInteger("OrbitTag", _baseData.OrbitTag);
    }
    public void Timer(BaseData _baseData)
    {
        /*
        _baseData.Time += Time.deltaTime;
        animator.SetFloat(TIMER, _baseData.Time);
        */
    }
    public void ResetTimer(BaseData _baseData)
    {
        /*
        _baseData.Time = 0;
        animator.SetFloat(TIMER, _baseData.Time);
        */
    }
    public void SetCycleTimer()
    {
        boss.CycleTimer += Time.deltaTime;
        animator.SetFloat("CycleTimer", boss.CycleTimer);
    }
    public void ResetCycleTimer()
    {

        boss.CycleTimer = 0;
        animator.SetFloat("CycleTimer", boss.CycleTimer);
    }

}