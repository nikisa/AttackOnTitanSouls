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
        _data.initialPosition = Temp.x + _initialRadius;
        OrbitList[_index].transform.position = new Vector3(_data.initialPosition, Temp.y, Temp.z);
       
    }
    public void MoveRadius(float _finalRadius , int _index , float _initialPosition , bool _hasPingPong)
    {
        
        if (!_hasPingPong && OrbitList[_index].transform.position.x < _finalRadius)
        {
            OrbitList[_index].transform.Translate(OrbitList[_index].transform.forward * speedRadius * Time.deltaTime);
            Mathf.Clamp(OrbitList[_index].transform.position.x, _initialPosition, _finalRadius);
        }
        if(OrbitList[_index].transform.position.x >= _finalRadius && _hasPingPong)
        {
            OrbitList[_index].transform.Translate(-OrbitList[_index].transform.forward * speedRadius * Time.deltaTime);
            Mathf.Clamp(OrbitList[_index].transform.position.x, _finalRadius, _initialPosition);
        }
        
    }
    public void RotationMove()
    {
        timeAcceleration = MaxSpeed / TimeAcceleration;
        moveSpeed += timeAcceleration * Time.deltaTime;
        transform.Rotate(Vector3.up * moveSpeed);
        moveSpeed = Mathf.Clamp(moveSpeed, 0, MaxSpeed);
    }
}
