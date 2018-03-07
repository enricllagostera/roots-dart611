using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu()]
public class PoolConfig : ScriptableObject
{
    public List<PlantInfo> trees;
    public List<PlantInfo> flowers;
    public List<PlantInfo> grass;
    public List<PlantInfo> props;
}