using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadTracker : DeviceTracker {
    public KeyCode[] buttonKeys;
    public float Horizontal;
    public float Vertical;


    private void Reset() {
        im = GetComponent<InputManager>();
        buttonKeys = new KeyCode[im.buttonCount];
    }

    public override void Refresh() {
        im = GetComponent<InputManager>();

        KeyCode[] newButtons = new KeyCode[im.buttonCount];
        AxisKeys[] newAxes = new AxisKeys[im.axisCount];

        if (buttonKeys != null) {
            for (int i = 0; i < Mathf.Min(newButtons.Length, buttonKeys.Length); i++) {
                newButtons[i] = buttonKeys[i];
            }
        }
        buttonKeys = newButtons;
    }

    void Update() {

        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");


        // Si può allegerire togliendo il for e mettendo
        // i bottoni del gamepad su KeyboardTracker
        for (int i = 0; i < buttonKeys.Length; i++) {
            if (Input.GetKey(buttonKeys[i])) {
                data.buttons[i] = true;
                newData = true;
            }
        }

        // Check degli Input , se l'input è stato rilevato , setto newData a TRUE
        // compila InputData da passare all'InputManager

        if (newData || Horizontal != 0 || Vertical != 0) {
            data.axes[0] = Vertical;
            data.axes[1] = Horizontal;
            im.PassInput(data);
            newData = false;
            data.Reset();
        }
    }


}