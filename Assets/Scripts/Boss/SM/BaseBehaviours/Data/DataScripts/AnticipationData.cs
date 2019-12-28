using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AnticipationData", menuName = "BossData/AnticipationData")]
public class AnticipationData : BaseData
{
    [Tooltip("Wait for Anticipation Time seconds. Then, exit.")]
    public float AnticipationTime;
    [Tooltip("Number of loops.")]
    public int Loops;
    [Tooltip("Never stops the loop.")]
    public bool InfinteLoops;
}
