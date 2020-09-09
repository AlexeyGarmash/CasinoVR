using OVRTouchSample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHolder : MonoBehaviour
{
    [SerializeField]
    public Animator handAnimator;

    [SerializeField]
    public CustomHand hand;

    [SerializeField] public HandPoseId defaultPose;

    [SerializeField] public HandPoseId ready;
    [SerializeField] public HandPoseId split;
    [SerializeField] public HandPoseId stop;
    [SerializeField] public HandPoseId give;
    [SerializeField] public HandPoseId doubleBet;

}
