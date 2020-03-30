using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossView : MonoBehaviour
{
    public BossController boss;

    //Private
    float negativeRotationSpeed;
   
    //public BossData GetBossData() {
    //    if (Data != null) {
    //        return Data;
    //    }
    //    else {
    //        Debug.Log("NEW BossData created");
    //        return new BossData();
    //    }
    //}

    public void AccelerationRotation(float _timeAcceleration, float _maxSpeed) {
        _timeAcceleration = _maxSpeed / _timeAcceleration;
        boss.RotationSpeed += _timeAcceleration * Time.deltaTime;
        boss.RotationSpeed = Mathf.Clamp(boss.RotationSpeed, -_maxSpeed, _maxSpeed); 
    }
    public void DecelerationRotation(float _timeDeceleration , float _lowSpeed) {
        _timeDeceleration = boss.MoveSpeed / _timeDeceleration;
        boss.RotationSpeed -= _timeDeceleration * Time.deltaTime;
        boss.RotationSpeed = Mathf.Clamp(boss.RotationSpeed, _lowSpeed, 100);
    }


    public void MoveRotation(float _maxSpeed) {
        var direzione = boss.Player.transform.position - transform.position;
        //float angle = Vector3.SignedAngle(transform.forward, direzione, Vector3.up);
        if (_maxSpeed > 0) {
            boss.Graphics.transform.Rotate(Vector3.up * Time.deltaTime * boss.RotationSpeed);
        }
        else {
            negativeRotationSpeed = boss.RotationSpeed;
            Mathf.Abs(negativeRotationSpeed);
            boss.Graphics.transform.Rotate(Vector3.down * Time.deltaTime * negativeRotationSpeed);
        }

        //return angle;
    }

    public void ChangeMaterial(Material _mat) {
        boss.Graphics.GetComponent<MeshRenderer>().material = _mat;
    }

}
