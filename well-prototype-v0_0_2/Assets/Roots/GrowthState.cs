using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthState : PlantStateBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("PlantSwitch", plant.plantIndex / 11f);
        // apply decay
        plant.health -= Time.deltaTime * plant.healthDecay;
        plant.growth -= Time.deltaTime * plant.growthDecay;
        // apply input modifiers
        if (plant.activeNutrient)
        {
            plant.health += Time.deltaTime * plant.healthSpeed;
        }
        if (plant.activeGrowth && plant.health > 0.5f)
        {
            plant.growth += Time.deltaTime * plant.growthSpeed;
        }
        // keep data in coeficient range
        plant.growth = Mathf.Clamp01(plant.growth);
        plant.health = Mathf.Clamp01(plant.health);
        // control animation transitions and timing
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
}
