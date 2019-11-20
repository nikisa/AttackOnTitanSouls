using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossView : MonoBehaviour
{
    public BossData Data;
    
    //void Init(BossData _bossData) {
    //    if (Data != null) {
    //        Data = _bossData;
    //    }
    //    else {
    //        Data = new BossData();
    //    }
    //}

    public BossData GetBossData() {
        if (Data != null) {
            return Data;
        }
        else {
            Debug.Log("NEW BossData created");
            return new BossData();
        }
    }

    public void AccelerationRotation(float _timeAcceleration, float _maxSpeed) {
        _timeAcceleration = _maxSpeed / _timeAcceleration;
        Data.bossInfo.RotationSpeed += _timeAcceleration * Time.deltaTime;
        Data.bossInfo.RotationSpeed = Mathf.Clamp(Data.bossInfo.RotationSpeed, -_maxSpeed, _maxSpeed); 
    }
    public void DecelerationRotation(float _timeDeceleration) {
        _timeDeceleration = Data.rotationAccelerationInfo.MaxSpeed / _timeDeceleration;
        Data.bossInfo.RotationSpeed -= _timeDeceleration * Time.deltaTime;
        Data.bossInfo.RotationSpeed = Mathf.Clamp(Data.bossInfo.RotationSpeed, Data.rotationDecelerationInfo.LowSpeed, 100);
    }


    public void MoveRotation() {
        var direzione = Data.bossInfo.Player.transform.position - transform.position;
        //float angle = Vector3.SignedAngle(transform.forward, direzione, Vector3.up);
        if (Data.rotationAccelerationInfo.MaxSpeed > 0) {
            Data.bossInfo.Graphics.transform.Rotate(Vector3.up * Time.deltaTime * Data.bossInfo.RotationSpeed);
        }
        //else {
        //    Data.negativeRotationSpeed = Data.RotationSpeed;
        //    Mathf.Abs(Data.negativeRotationSpeed);
        //    Graphics.transform.Rotate(Vector3.down * Time.deltaTime * Data.negativeRotationSpeed);
        //}

        //return angle;
    }

    public void ChangeMaterial(Material _mat) {
        Data.bossInfo.Graphics.GetComponent<MeshRenderer>().material = _mat;
    }

}
