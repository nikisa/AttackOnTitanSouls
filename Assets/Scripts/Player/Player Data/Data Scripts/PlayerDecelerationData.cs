using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Movement Deceleration Data")]
public class PlayerDecelerationData : ScriptableObject
{
    [Tooltip("Decreasing rate of entry speed, in m/sec^2.")]
    public float Deceleration;
}
