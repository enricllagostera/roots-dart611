using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiltState : PlantStateBehaviour
{
    private float deathTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // hard reset to inert
        if (plant.state == EPlantState.INERT)
        {
            animator.Play("inertBT", 0, 0);
            return;
        }

        // normal plant death due to decay
        if (plant.state == EPlantState.DEAD)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0)
            {
                animator.Play("inertBT", 0, 0);
            }
            return;
        }

        animator.SetFloat("PlantSwitch", plant.plantIndex / 11f);
        if (plant.activeNutrient)
        {
            plant.health += Time.deltaTime * plant.healthSpeed;
        }
        plant.health -= Time.deltaTime * plant.healthDecay;
        // if the plant is healthy enough, it will count the time to reproduce
        if (plant.health >= 0.95f)
        {
            plant.reproductionTimer -= Time.deltaTime;
            if (plant.reproductionTimer < 0)
            {
                plant.reproductionTimer = plant.reproductionInterval;
                plant.Reproduce();
            }
        }
        plant.growth = Mathf.Clamp01(plant.growth);
        plant.health = Mathf.Clamp01(plant.health);
        plant.growth = plant.health;
        if (plant.health <= 0f)
        {
            plant.MakeDead();
            deathTimer = plant.info.deathTimer;
            return;
        }
        else
        {
            animator.Play("wiltBT", 0, (1f - plant.health));
        }
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
