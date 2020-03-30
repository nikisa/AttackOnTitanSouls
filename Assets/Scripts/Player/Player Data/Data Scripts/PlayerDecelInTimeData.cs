using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Movement Deceleration In Time Data")]
public class PlayerDecelInTimeData : ScriptableObject
{
    public AnimationCurve MovementDecelerationCurve;
    [Tooltip("In sec. How much time it takes to go from MaxSpeed to 0.")]
    public float DecelerationTime;
}
