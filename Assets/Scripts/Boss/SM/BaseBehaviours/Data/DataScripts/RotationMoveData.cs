using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RotationMoveData", menuName = "BossData/RotationMoveData")]
public class RotationMoveData : ScriptableObject
{
    [Tooltip("in degrees/sec. Use positive value to rotate clockwise and negative to rotate anticlockwise.")]
    public float MaxSpeed;
    [Tooltip("The value to add (or subtract, if negative) to the entry rotation speed parameter")]
    public float AddToRotationSpeed;
}
