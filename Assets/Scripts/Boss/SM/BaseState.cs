using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : StateMachineBehaviour
{

    protected object context;
    protected Animator animator;
    protected BossOrbitManager bossOrbitManager;

    protected const string MASKS_COUNT = "MasksCount";

    private void Awake() {
        
    }

    public virtual void SetContext(object context, Animator animator , BossOrbitManager bossOrbitManager)
    {
        this.context = context;
        this.animator = animator;
        this.bossOrbitManager = bossOrbitManager;
    }

    public virtual void Enter()
    {

    }

    public virtual void Tick()
    {

    }

    public virtual void Exit()
    {

    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enter();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Exit();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Tick();
    }

    public void CheckVulnerability() {
        animator.SetInteger(MASKS_COUNT, bossOrbitManager.HookPointList.Count);
    }


}
