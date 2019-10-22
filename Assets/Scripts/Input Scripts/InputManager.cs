using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Range(0, 10)]
    public int axisCount;
    [Range(0, 20)]
    public int buttonCount;

    public Controller controller;

    public InputData data;

    public void PassInput(InputData _data) {
        data = _data;
        Debug.LogFormat("Movement: {0} , {1}", data.axes[0], data.axes[1]);
        Debug.LogFormat("Movement: {0} , {1}", data.Vertical, data.Horizontal);

        //controller.ReadInput(data);

    }

    public void RefreshTracker() {
        DeviceTracker dt = GetComponent<DeviceTracker>();
        if (dt != null) {
            dt.Refresh();
        }

    }
}

public struct InputData {
    public float Horizontal;
    public float Vertical;
    public float[] axes;
    public bool[] buttons;

    public InputData(int axisCount, int buttonCount) {
        Horizontal = 0;
        Vertical = 0;
        axes = new float[axisCount];
        buttons = new bool[buttonCount];
    }

    public void Reset() {
        for (int i = 0; i < axes.Length; i++) {
            axes[i] = 0f;
        }
        for (int i = 0; i < buttons.Length; i++) {
            buttons[i] = false;
        }
    }
}
