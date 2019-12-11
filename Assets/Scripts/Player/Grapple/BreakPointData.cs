using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BreakPointData", menuName = "BreakPointData/BreakPointData")]
public class BreakPointData : ScriptableObject
{
    public float lifeMax;
    public float InitialElasticK;
    public float FinalElasticK;
    public float RestoreLifeRatio;
}
