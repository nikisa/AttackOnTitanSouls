using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class Controller : MonoBehaviour
{
    public abstract void ReadInput(InputData data);

    
    protected bool newInput;

    
    
}
