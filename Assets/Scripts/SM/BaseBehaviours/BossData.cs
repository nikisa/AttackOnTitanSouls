using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "BossData", menuName = "BossGame/Boss Data")]
public class BossData : ScriptableObject
{


    #region BaseInfo
    
    public BossInfo bossInfo;
    public IdleInfo idleInfo;
    public AnticipationInfo anticipationInfo;
    public AccelerationInfo accelerationInfo;
    public GraphicsAnticipationInfo graphicsAnticipationInfo;
    public RotationAccelerationInfo rotationAccelerationInfo;
    public RecoveryInfo recoveryInfo;
    public DecelerationInfo decelerationInfo;
    public RotationDecelerationInfo rotationDecelerationInfo;
    public MoveToInfo moveToInfo;
    public OrbitInfo orbitInfo;
    public Orbit orbit;
    #endregion

    #region WallHitInfo
    public BossInfo wallBossInfo;
    public AnticipationInfo wallAnticipationInfo;
    public AccelerationInfo wallAccelerationInfo;
    public GraphicsAnticipationInfo wallGraphicsAnticipationInfo;
    public RotationAccelerationInfo wallRotationAccelerationInfo;
    public RecoveryInfo wallRecoveryInfo;
    public DecelerationInfo wallDecelerationInfo;
    public RotationDecelerationInfo wallRotationDecelerationInfo;
    public MoveToInfo wallMoveToInfo;
    #endregion

    public struct BossInfo
    {
        
        public PlayerController Player;
        public GameObject Graphics;
        public float MoveSpeed;
        public float RotationSpeed;
    }

    [System.Serializable]
    public struct IdleInfo
    {
        public float stateDuration;
    }

    [System.Serializable]
    public struct AnticipationInfo
    {
        [Tooltip("Wait for Anticipation Time seconds. Then, exit.")]
        public float AnticipationTime;
        [Tooltip("Number of loops.")]
        public int Loops;
        [Tooltip("Never stops the loop.")]
        public bool InfinteLoops;
    }

    [System.Serializable]
    public struct AccelerationInfo
    {
        [Tooltip("Maximum reachable speed, in m/sec.")]
        public float MaxSpeed;
        [Tooltip("Increasing rate of entry speed, in m/sec^2")]
        public float TimeAcceleration;
        [Tooltip("On State Enter: wait X seconds before accelerating")]
        public float WaitOnStart;
    }

    [System.Serializable]
    public struct RotationAccelerationInfo
    {
        [Tooltip("in degrees/sec. Use positive value to rotate clockwise and negative to rotate anticlockwise.")]
        public float MaxSpeed;
        [Tooltip("Increasing rate of entry rotation speed , in degrees/sec^2.")]
        public float AccelerationTime;
        [Tooltip("On State Enter: wait X seconds before accelerating")]
        public float WaitOnStart;
    }

    [System.Serializable]
    public struct RecoveryInfo
    {
        public float RecoveryTime;
    }

    [System.Serializable]
    public struct DecelerationInfo
    {
        [Tooltip("Decreasing rate of entry speed, in m/sec^2.")]
        public float TimeDeceleration;
        [Tooltip("On State Enter: wait X seconds before decelerating")]
        public float WaitOnStart;
        [Tooltip("Minimum reachable speed, in m/sec. Use negative value to reverse vector")]
        public float LowSpeed;
    }

    [System.Serializable]
    public struct RotationDecelerationInfo
    {

        public float LowSpeed;
        [Tooltip("Decrese rate of entry rotation speed , in degrees/sec^2.")]
        public float DecelerationTime;
        [Tooltip("On State Enter: wait X seconds before accelerating")]
        public float WaitOnStart;
    }

    [System.Serializable]
    public struct MoveToInfo
    {
        public Targets targets;
        [HideInInspector]
        public GameObject Target;
        [Tooltip("Exits the state when it passes over the coordinates of the target")]
        public bool StopsAtTargetOvertaking; // per testing 
    }
    [System.Serializable]
    public struct Orbit {
        public TargetsOrbit centerPoint;
        [HideInInspector]
        public GameObject CenterPoint;
        public TargetsOrbit center;
        [HideInInspector]
        public GameObject Center;      
        public TargetsOrbit tentacle;
        [HideInInspector]
        public GameObject Tentacle;
    }
    
    [System.Serializable]
    public struct OrbitInfo
    {
        public float InitialRadius;
        public bool HasDeltaRadius;
        public float FinalRadius;
        public float Angle;
        public float OrbitSpeed;
    }

    [System.Serializable]
    public struct GraphicsAnticipationInfo
    {
        public Material AnticipationMat;
        public Material NormalMat;
    }
    public enum Targets 
    {
        Player,
        
    };
    public enum TargetsOrbit
    {
        Boss,
        CenterPoint,
        Tentacle,
    }

}
