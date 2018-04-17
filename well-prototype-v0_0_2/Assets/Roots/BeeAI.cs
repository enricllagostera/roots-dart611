using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeeAI : MonoBehaviour
{
    private Bee bee;
    private EBeehavior previousAction;
    private EBeehavior[] possibleActions = new EBeehavior[8]{
        EBeehavior.REST,
        EBeehavior.PLAY,
        EBeehavior.SAD,
        EBeehavior.SAD,
        EBeehavior.HAPPY,
        EBeehavior.HAPPY,
        EBeehavior.SURPRISED,
        EBeehavior.SURPRISED
    };

    private bool ongoingAction;
    private bool moving;
    private EBeehavior currentAction;
    public Plant targetPlant;
    [SerializeField] private Plant[] allPlants;

    [SerializeField] private float actionDurationMin;
    [SerializeField] private float actionDurationMax;
    private float angularDelta;

    void Start()
    {
        ongoingAction = false;
        moving = false;
        bee = GetComponent<Bee>();
        previousAction = EBeehavior.IDLE;
        StartCoroutine(Execute());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // StartCoroutine(Execute());
        }
    }


    EBeehavior RandomizeAction()
    {
        EBeehavior action = possibleActions[Random.Range(0, possibleActions.Length)];
        while (action == previousAction)
        {
            action = possibleActions[Random.Range(0, possibleActions.Length)];
        }
        return action;
    }


    bool CheckActionConditions(EBeehavior targetAction, ref Plant targetPlant)
    {
        switch (targetAction)
        {
            case EBeehavior.HAPPY:
                var babyPlants = allPlants.Where(p => p.age <= 15f && p.age > 0f).ToList();
                if (babyPlants.Count > 0)
                {
                    targetPlant = babyPlants[Random.Range(0, babyPlants.Count)];
                    return true;
                }
                return false;

            case EBeehavior.SAD:
                var deadPlants = allPlants.Where(p => p.state == EPlantState.DEAD).ToList();
                if (deadPlants.Count > 0)
                {
                    targetPlant = deadPlants[Random.Range(0, deadPlants.Count)];
                    return true;
                }
                return false;

            case EBeehavior.SURPRISED:

                var viablePlants = allPlants.Where(p => p.activeGrowth || p.activeNutrient).ToList();
                if (viablePlants.Count > 0)
                {
                    targetPlant = viablePlants[Random.Range(0, viablePlants.Count)];
                    return true;
                }
                return false;

            // sleep and play can happen anytime
            default: return true;
        }
    }


    IEnumerator Execute()
    {
        bee.StopAllCoroutines();
        ongoingAction = false;
        targetPlant = null;
        allPlants = transform.parent.GetComponentsInChildren<Plant>();
        // pick action stage only if it's not already doing something
        while (!ongoingAction)
        {
            Debug.Log("randomizing action");
            var newAction = RandomizeAction();
            Debug.Log("checking action: " + newAction.ToString());
            if (CheckActionConditions(newAction, ref targetPlant))
            {
                previousAction = currentAction;
                currentAction = newAction;
                ongoingAction = true;
                Debug.Log("picked action: " + currentAction.ToString());
            }
            // limit to one try per frame
            yield return null;
        }
        // execute the action stage
        // see if the target plant exists, if not, bee does not move
        if (targetPlant == null)
        {

        }
        // if bee should move, check if it should walk of fly there (based on angle distance)
        else
        {
            // angle calculation
            Vector2 v = (Vector2)targetPlant.transform.position - Vector2.zero;
            //use atan2 to get the angle; Atan2 returns radians
            var angleRadians = Mathf.Atan2(v.y, v.x);
            //convert to degrees
            var angleDegrees = angleRadians * Mathf.Rad2Deg + 90f;


            moving = true;
            bee.targetAngle = angleDegrees;
            float angularDistance = Mathf.Abs(Mathf.DeltaAngle(bee.angle, bee.targetAngle));
            // start flying
            if (angularDistance >= 50f)
            {
                Debug.Log("start flying");
                bee.StartCoroutine(bee.Fly());
            }
            // or start walking
            else if (angularDistance >= 1f)
            {
                Debug.Log("start walking");
                bee.StartCoroutine(bee.Walk());
            }
            else
            {
                bee.isMoving = false;
            }
            // wait for movement
            while (bee.isMoving)
            {
                yield return null;
            }
        }
        Debug.Log("bee stopped");
        moving = false;
        // once movement is over, perform the action picked for a random duration of time
        float duration = Random.Range(actionDurationMin, actionDurationMax);
        // action changing command
        Debug.Log("start " + currentAction.ToString());
        switch (currentAction)
        {
            case EBeehavior.REST: bee.animator.SetBool("Sleeping", true); break;
            case EBeehavior.SAD: bee.animator.SetBool("Sad", true); break;
            case EBeehavior.HAPPY: bee.animator.SetBool("Happy", true); break;
            case EBeehavior.SURPRISED: bee.animator.SetBool("Surprised", true); break;
            case EBeehavior.PLAY: bee.animator.SetBool("Playing", true); break;
        }
        yield return new WaitForSeconds(duration);
        Debug.Log("finished " + currentAction.ToString());
        ongoingAction = false;
        bee.animator.SetBool("Sleeping", false);
        bee.animator.SetBool("Playing", false);
        bee.animator.SetBool("Sad", false);
        bee.animator.SetBool("Happy", false);
        bee.animator.SetBool("Surprised", false);
        bee.canTeleport = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        bee.canTeleport = false;
        StartCoroutine(Execute());
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (targetPlant != null)
        {
            Gizmos.DrawRay(Vector3.zero, targetPlant.transform.position - Vector3.zero);
        }
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireSphere(transform.position, _radius);
    }
}


