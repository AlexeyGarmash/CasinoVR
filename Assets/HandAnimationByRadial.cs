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
        menu.InvokeMenu();
       

        menu.OnRadialSectorChangeListener += (RadialSectorV2 sect) =>
        {

            if (sect.RadialSectorType == RadialMenuSectorV2.TOP_LEFT)
                hand.SetPose(ready);
            if (sect.RadialSectorType == RadialMenuSectorV2.TOP_RIGHT)
                hand.SetPose(doubleBet);
            if (sect.RadialSectorType == RadialMenuSectorV2.LEFT)
                hand.SetPose(give);

            if (sect.RadialSectorType == RadialMenuSectorV2.RIGHT)
                hand.SetPose(split);
            if (sect.RadialSectorType == RadialMenuSectorV2.BOTTOM_LEFT)
                hand.SetPose(save);

            if (sect.RadialSectorType == RadialMenuSectorV2.BOTTOM_RIGHT)
                hand.SetPose(stop);


        };
      
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //    handAnimator.SetInteger("Pose", currentAnimation);
      
       
    //}
}
