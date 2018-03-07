using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthState : PlantStateBehaviour
{

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Wilting");
        if (plant.activeNutrient)
        {
            plant.health += Time.deltaTime * plant.healthSpeed;
        }
        plant.health -= Time.deltaTime * plant.healthDecay;
        plant.growth -= Time.deltaTime * plant.growthDecay;
        if (plant.activeGrowth && plant.health > 0.5f)
        {
            plant.growth += Time.deltaTime * plant.growthSpeed;
        }
        plant.growth = Mathf.Clamp01(plant.growth);
        plant.health = Mathf.Clamp01(plant.health);
        if (plant.growth >= 1f)
        {
            animator.Play("wiltBT", 0, 0f);
            return;
        }
        else
        {
            animator.Play("growthBT", 0, plant.growth);
        }
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("leaving growth");
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
