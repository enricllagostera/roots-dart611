using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu()]
public class PoolConfig : ScriptableObject
{
    public List<Transform> trees;
    public List<Transform> grass;
    public List<Transform> props;
}