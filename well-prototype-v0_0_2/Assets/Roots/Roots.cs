using UnityEngine;
using System.Collections.Generic;

public class Roots : Singleton<Roots>
{
    public GroundLayer activeLayer;
    public InputPoolConfig inputPool;
    public List<KeyCode> availableInputs;



    protected override void Awake()
    {
        base.Awake();
    }



    void Update()
    {

    }



    public void SetActiveLayer(GroundLayer newLayer)
    {
        if (newLayer != activeLayer)
        {
            Plant[] allPlantsInLayer;
            if (activeLayer != null)
            {
                allPlantsInLayer = activeLayer.GetComponentsInChildren<Plant>();
                foreach (var plant in allPlantsInLayer)
                {
                    plant.ClearInputs();
                }
            }
            Debug.Log("RANDOMIZE ALL");
            activeLayer = newLayer;
            allPlantsInLayer = activeLayer.GetComponentsInChildren<Plant>();
        }
    }




}