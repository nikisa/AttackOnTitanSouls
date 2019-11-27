using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData/Boost Data")]
public class PlayerBoostData : ScriptableObject
{
    //Inspector
    public float BoostSpeed;
    public float DecelerationTime;
    public float BoostTimeFreeze;
    public float AngularSpeed;
    public float ChargingSpeed;
    public float StunDuration;
}
