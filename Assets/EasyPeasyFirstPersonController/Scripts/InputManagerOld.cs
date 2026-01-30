public class MobileInput
{
    public static float MouseX;
    public static float MouseY;
    public static float horizontal;
    public static float vertical;
    public static bool interact;
    public static bool mask;
}

namespace EasyPeasyFirstPersonController
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class InputManagerOld : MonoBehaviour
    {
        public Vector2 moveInput;
        public Vector2 lookInput;
        public bool jump;
        public bool sprint;
        public bool crouch;
        public bool slide;
        public bool interact;
        public bool mask;
        void Update()
        {
            moveInput = new Vector2(MobileInput.horizontal,MobileInput.vertical);//new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            lookInput = new Vector2(MobileInput.MouseX,MobileInput.MouseY);//new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            jump = Input.GetKey(KeyCode.Space);
            interact = Input.GetKey(KeyCode.E) || MobileInput.interact;
            mask = Input.GetKey(KeyCode.Q) || MobileInput.mask;
        }
    }
}