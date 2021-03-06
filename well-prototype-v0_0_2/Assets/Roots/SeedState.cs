﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedState : PlantStateBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    // override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     plant.MakeSeed();
    // }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // hard reset to inert
        if (plant.state == EPlantState.INERT)
        {
            animator.Play("inertBT", 0, 0);
            return;
        }

        if (plant.activeNutrient)
        {
            plant.health += Time.deltaTime * plant.healthSpeed;
        }

        if (plant.activeGrowth && plant.health > 0.5f)
        {
            animator.SetTrigger("Growing");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        plant.growth = 0f;
        plant.health = 1f;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
