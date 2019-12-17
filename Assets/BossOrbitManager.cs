using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOrbitManager : MonoBehaviour
{
    public List<GameObject> OrbitList;
    public float MaxSpeed;
    public float TimeAcceleration;
    float timeAcceleration;
    float speedRadius = 3;
    float moveSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetInitial(float _initialRadius , int _index , OrbitData _data)
    {
        Vector3 Temp = OrbitList[_index].transform.position;
        _data.initialPosition = Temp.z + _initialRadius;
        OrbitList[_index].transform.position = new Vector3(Temp.x, Temp.y, _data.initialPosition);
       
    }
    public void MoveRadius(float _finalRadius , int _index , float _initialPosition , bool _hasPingPong)
    {
        Debug.Log(OrbitList[_index].transform.localPosition.z);
        Debug.Log(_initialPosition + _finalRadius);
        if (!_hasPingPong && _initialPosition + _finalRadius > OrbitList[_index].transform.localPosition.z)
        {
            //Debug.Log(OrbitList[_index].transform.position.z);
             OrbitList[_index].transform.Translate(/*OrbitList[_index].transform.forward*/ Vector3.forward* speedRadius * Time.deltaTime);
            //OrbitList[_index].transform.position = new Vector3(OrbitList[_index].transform.localPosition.x, OrbitList[_index].transform.position.y, OrbitList[_index].transform.position.z + speedRadius * Time.deltaTime);
           // OrbitList[_index].transform.position  = new Vector3(OrbitList[_index].transform.position.x, OrbitList[_index].transform.position.y, Mathf.Clamp(OrbitList[_index].transform.position.z, _initialPosition, _finalRadius));
        }
        if(OrbitList[_index].transform.position.z >= _finalRadius && _hasPingPong)
        {
            OrbitList[_index].transform.Translate(-OrbitList[_index].transform.forward * speedRadius * Time.deltaTime);
            Mathf.Clamp(OrbitList[_index].transform.position.z, _finalRadius, _initialPosition);
        }
        
    }
    public void RotationMove()
    {
        timeAcceleration = MaxSpeed / TimeAcceleration;
        moveSpeed += timeAcceleration * Time.deltaTime;

        transform.Rotate(Vector3.up * moveSpeed);
        moveSpeed = Mathf.Clamp(moveSpeed, 0, MaxSpeed);
    }
    public void SetAllInitialPosition(int _index, OrbitData _data)
    {
        _data.initialPosition = OrbitList[_index].transform.localPosition.z;
        Debug.Log(OrbitList[_index].transform.localPosition.z);
    }
}
