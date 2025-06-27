using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace JYL
{
    public class TitlePresenter : BaseUI
    {
        private bool anyKeyPressed = false;

        void OnEnable()
        {
            InputSystem.onAnyButtonPress.Call(ctrl => OnAnyButtonPress(ctrl));
        }
        private void Update()
        {
        }
        void OnDisable()
        {
            
        }

        private void OnAnyButtonPress(InputControl control)
        {
            // �� Ÿ�� �Է�(Vector2 ��)�� ����
            if (control is ButtonControl && (control.device is Keyboard || control.device is Gamepad gamepad))
            {
                anyKeyPressed = true;
            }
        }

        public bool IsAnyKeyPressed()
        {
            return anyKeyPressed;
        }
    }
}
