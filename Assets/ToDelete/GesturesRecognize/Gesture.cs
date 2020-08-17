using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gesture : MonoBehaviour
{
    public Action<Gesture, bool> ResolveGesture;

    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private OVRInput.Button _firstButton;
    [SerializeField] private OVRInput.Button _secondButton;

    private bool isPressed;
    public string Name { get => _name; }
    public string Description { get => _description; }

    private void Update()
    {
        CheckAllButtonsDown();
    }

    private void CheckAllButtonsDown()
    {

            if(OVRInput.Get(_firstButton) && OVRInput.Get(_secondButton) && !isPressed)
            {
                ResolveGesture.Invoke(this, true);
                isPressed = true;
            } 
            else if(isPressed && (!OVRInput.Get(_firstButton) || !OVRInput.Get(_secondButton)))
            {
                ResolveGesture.Invoke(this, false);
                isPressed = false;
            }
    }
}
