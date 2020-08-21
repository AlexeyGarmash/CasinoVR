using OVR.OpenVR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureListener : MonoBehaviour
{
    [SerializeField] private Animator _handAnimator;
    [SerializeField] private float _gestureTime;
    [SerializeField] private GestureLoader _gestureLoader;
    [SerializeField] private List<Gesture> _allowedGesture;
    public float currentGestureTime;

    private bool startPerformGesture = false;
    private Gesture currentGesture = null;

    private void Start()
    {
        foreach(var gesture in _allowedGesture)
        {
            gesture.ResolveGesture += OnGestureStartPerform;
        }
    }

    private void OnGestureStartPerform(Gesture gesture, bool perform)
    {
        print(string.Format("Gesture {0} {1} performed!", gesture.Name, perform ? "START" : "RESET"));
        if(perform)
        {
            _handAnimator.SetInteger("Pose",4);
        }
        else
        {
            _handAnimator.SetInteger("Pose", 0);
        }
        currentGesture = gesture;
        startPerformGesture = perform;
    }

    private void Update()
    {
        if(currentGestureTime >= _gestureTime)
        {
            PerformFinalGesture();
            
        }
        if(startPerformGesture)
        {
            StartActivateGesture();
        }
        else
        {
            ResetActivateGesture();
        }
    }

    private void PerformFinalGesture()
    {
        print(string.Format("Gesture {0} with description {1} performed finaly!", currentGesture.Name, currentGesture.Description));
        ResetActivateGesture();
    }

    private void StartActivateGesture()
    {
        currentGestureTime += Time.deltaTime;
        _gestureLoader.SetProgress(currentGestureTime, _gestureTime, false);
    }

    private void ResetActivateGesture()
    {
        currentGestureTime = 0f;
        currentGesture = null;
        _gestureLoader.SetProgress(currentGestureTime, _gestureTime, true);
    }
}
