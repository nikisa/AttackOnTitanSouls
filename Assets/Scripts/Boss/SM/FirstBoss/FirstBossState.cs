﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossState : BaseState
{

    protected BossOrbitManager bossOrbitManager;
    protected FirstBossController boss;
    protected FirstBossMask bossMask;

    //SateMachine Parameters
    protected const string IDLE = "Idle";
    protected const string ANTICIPATION = "Anticipation";
    protected const string MOVETO = "MoveTo";
    protected const string RECOVERY = "Recovery";
    protected const string MASKS_COUNT = "MasksCount";
    protected const string DECELERATION = "Deceleration";
    protected const string END_STATE_TRIGGER = "EndState";
    protected const string TIMER = "Timer";

    protected float timer = 0;
    int layerResult = 0;


    public override void Enter() {
        animator.SetFloat(TIMER, timer = 0);
        BossOrbitManager.ChangedStateEvent();
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        animator.SetFloat(TIMER, timer);
    }

    public override void Exit() {
        layerResult = 0;
    }


    public void SetContext(object context, object secondContext , Animator animator, BossOrbitManager bossOrbitManager)
    {
        //base.SetContext(context, animator , bossOrbitManager);
        boss = context as FirstBossController;
        bossMask = secondContext as FirstBossMask;
        this.animator = animator;
        this.bossOrbitManager = bossOrbitManager;

    }

    protected void TriggerExitState()
    {
        animator.SetTrigger(END_STATE_TRIGGER);
    }


    //Updates the MASK_COUNT SM Parameters when a Boss' Mask is detroyed
    public void CheckVulnerability() {
        animator.SetInteger(MASKS_COUNT, bossOrbitManager.MasksList.Count);
    }

    //Set the tag to choose the next OrbitState
    public void OrbitTag(BaseData _baseData)
    {
        animator.SetInteger("OrbitTag", _baseData.OrbitTag);
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