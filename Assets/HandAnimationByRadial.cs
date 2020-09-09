using OVRTouchSample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnimationByRadial : MonoBehaviour
{
    [SerializeField]
    Animator handAnimator;
    [SerializeField]
    RadialMenuHandV2 menu;

    CustomHand hand;

    [SerializeField]HandPoseId ready;
    [SerializeField]HandPoseId split;
    [SerializeField]HandPoseId stop;
    [SerializeField]HandPoseId save;
    [SerializeField]HandPoseId give;
    [SerializeField] HandPoseId doubleBet;

    // Start is called before the first frame update
    int currentAnimation;
    void Start()
    {
        hand = GetComponent<CustomHand>();

        menu.AddAction(new RadialActionInfo(() => { hand.SetPose(ready); }, "Ready"));
        menu.AddAction(new RadialActionInfo(() => { hand.SetPose(doubleBet); }, "doubleBet"));
        menu.AddAction(new RadialActionInfo(() => { hand.SetPose(give); }, "give"));
        //menu.AddAction(new RadialActionInfo(() => { hand.SetPose(split); }, "split"));
        //menu.AddAction(new RadialActionInfo(() => { hand.SetPose(save); }, "save"));
        //menu.AddAction(new RadialActionInfo(() => { hand.SetPose(stop); }, "stop"));

        menu.InvokeMenu();
       

    }

   
}
