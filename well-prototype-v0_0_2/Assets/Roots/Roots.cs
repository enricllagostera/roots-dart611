using UnityEngine;
using System.Collections.Generic;

public class Roots : Singleton<Roots>
{
    public GroundLayer activeLayer;
    public InputPoolConfig inputPool;
    public List<KeyCode> availableInputs;
    public KeyCode rainKey;
    public KeyCode windKey;

    protected override void Awake()
    {
        base.Awake();
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
            RandomizeInputs();
            allPlantsInLayer = activeLayer.GetComponentsInChildren<Plant>();
            foreach (var plant in allPlantsInLayer)
            {
                plant.RandomizeInputs();
            }
        }
    }

    void RandomizeInputs()
    {
        List<KeyCode> keys = new List<KeyCode>(inputPool.all);
        int assigned = 0;
        while (assigned < 2)
        {
            int i = Random.Range(0, keys.Count);
            KeyCode pick = keys[i];
            if (assigned == 0)
            {
                rainKey = pick;
            }
            else
            {
                windKey = pick;
            }
            assigned++;
            keys.RemoveAt(i);
        }
        availableInputs = keys;
    }
}