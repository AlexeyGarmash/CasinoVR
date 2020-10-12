/************************************************************************************
Copyright : Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.

Licensed under the Oculus Utilities SDK License Version 1.31 (the "License"); you may not use
the Utilities SDK except in compliance with the License, which is provided at the time of installation
or download, or which otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at
https://developer.oculus.com/licenses/utilities-1.31

Unless required by applicable law or agreed to in writing, the Utilities SDK distributed
under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
ANY KIND, either express or implied. See the License for the specific language governing
permissions and limitations under the License.
************************************************************************************/

using Assets.Scipts.Player;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scipts.Chips;
using OVRTouchSample;
/// <summary>
/// Allows grabbing and throwing of objects with the OVRGrabbable component on them.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class OVRGrabberCustom : MonoBehaviourPun
{
    #region Extention variables 
    [Header("New Settings")]
    //кнопка 1 для взятия предмета (нужна для добавления нескольких предметов в руку)
    [SerializeField]
    private OVRInput.Button GrabButton = OVRInput.Button.PrimaryHandTrigger;

    private PlayerStats playerStat;
    //кнопка 2 для взятия предмета
    [SerializeField]
    private OVRInput.Axis1D GrabAxis = OVRInput.Axis1D.PrimaryHandTrigger;

    //список предметов которые находяться в руке
    [SerializeField]
    public List<OVRGrabbableCustom> m_grabbedObjs;

    //холдер для предметов руки находиться в TrackingSpace/LeftHandAnchor для левой руки TrackingSpace/RightHandAnchor для правой
    [SerializeField]
    public Transform grabbleObjSpawnPoint;


    //число предметов для взятия в руку
    public int max_grabbed_obj = 5;

    // нажатие кнопки GrabButton
    private bool add_chip;

    #endregion

    public enum GrabberState { Default, SelectionChips }

    protected GrabberState state = GrabberState.Default;
    public GrabberState State => state;

    protected CustomHand HandPose;
    protected IndexFingerChipSelection intexFingerSelector;
    [Header("Other Settings")]
    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public float grabBegin = 0.55f;
    public float grabEnd = 0.35f;

    public float throwForce = 1.0f;

    bool alreadyUpdated = false;

    // Demonstrates parenting the held object to the hand's transform when grabbed.
    // When false, the grabbed object is moved every FixedUpdate using MovePosition.
    // Note that MovePosition is required for proper physics simulation. If you set this to true, you can
    // easily observe broken physics simulation by, for example, moving the bottom cube of a stacked
    // tower and noting a complete loss of friction.
    [SerializeField]
    protected bool m_parentHeldObject = false;

    // If true, this script will move the hand to the transform specified by m_parentTransform, using MovePosition in
    // FixedUpdate. This allows correct physics behavior, at the cost of some latency. In this usage scenario, you
    // should NOT parent the hand to the hand anchor.
    // (If m_moveHandPosition is false, this script will NOT update the game object's position.
    // The hand gameObject can simply be attached to the hand anchor, which updates position in LateUpdate,
    // gaining us a few ms of reduced latency.)
    [SerializeField]
    protected bool m_moveHandPosition = false;

    // Child/attached transforms of the grabber, indicating where to snap held objects to (if you snap them).
    // Also used for ranking grab targets in case of multiple candidates.
    [SerializeField]
    protected Transform m_gripTransform = null;
    // Child/attached Colliders to detect candidate grabbable objects.
    [SerializeField]
    protected Collider[] m_grabVolumes = null;

    // Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch.
    [SerializeField]
    public OVRInput.Controller m_controller;

    // You can set this explicitly in the inspector if you're using m_moveHandPosition.
    // Otherwise, you should typically leave this null and simply parent the hand to the hand anchor
    // in your scene, using Unity's inspector.
    [SerializeField]
    protected Transform m_parentTransform;

    [SerializeField]
    protected GameObject m_player;

    protected bool m_grabVolumeEnabled = true;
    protected Vector3 m_lastPos;
    protected Quaternion m_lastRot;
    protected Quaternion m_anchorOffsetRotation;
    protected Vector3 m_anchorOffsetPosition;
    protected float m_prevFlex;
    protected OVRGrabbableCustom m_grabbedObj = null;
    protected Vector3 m_grabbedObjectPosOff;
    protected Quaternion m_grabbedObjectRotOff;
    protected Dictionary<OVRGrabbableCustom, int> m_grabCandidates = new Dictionary<OVRGrabbableCustom, int>();
    protected bool m_operatingWithoutOVRCameraRig = true;


    float closestMagSq = float.MaxValue;
    OVRGrabbableCustom closestGrabbable = null;
    Collider closestGrabbableCollider = null;

    /// <summary>
    /// The currently grabbed object.
    /// </summary>
    public OVRGrabbableCustom grabbedObject
    {
        get { return m_grabbedObj; }
    }

    public bool IsLocalHand { get { return photonView.IsMine; } }
    

    protected virtual void Awake()
    {
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;
     
    }


    protected virtual void Start()
    {
        HandPose = GetComponent<CustomHand>();
        intexFingerSelector = GetComponentInChildren<IndexFingerChipSelection>();
        playerStat = GetComponentInParent<PlayerStats>();

        m_lastPos = transform.position;
        m_lastRot = transform.rotation;

        if (m_parentTransform == null)
        {
            m_parentTransform = gameObject.transform;
        }
        // We're going to setup the player collision to ignore the hand collision.
        SetPlayerIgnoreCollision(gameObject, true);

        m_grabbedObjs = new List<OVRGrabbableCustom>();
    }

    virtual public void Update()
    {
        alreadyUpdated = false;
    }

    virtual public void FixedUpdate()
    {
        if (m_operatingWithoutOVRCameraRig)
        {          
            OnUpdatedAnchors();          
        }
    }
    public void ForceRelease(OVRGrabbableCustom grabbable)
    {
        bool canRelease = (m_grabbedObj != null && m_grabbedObjs.Contains(grabbable));
        if (canRelease)
        {
            GrabEnd();

        }

        int value;
        if (m_grabCandidates.TryGetValue(grabbable, out value))
        {
            m_grabCandidates.Remove(grabbable);

        }
    }
    public List<GrabbableChip> selectedChips = new List<GrabbableChip>();
    void SelectionChips(float prevFlex)
    {
        if (m_prevFlex >= grabBegin && prevFlex < grabBegin && !changeStateInProcess)
        {
            if (!intexFingerSelector.IsFingerActivated)
                intexFingerSelector.ActivateFinger(true);

            HandPose.SetPose(HandPoseId.BJ_DoubleBet);         

            Debug.Log("StartChipSelection");

        }
        else if ((m_prevFlex <= grabEnd) && (prevFlex > grabEnd))
        {
            

            intexFingerSelector.ActivateFinger(false);

            selectedChips.ForEach(selected =>
            {
                closestGrabbableCollider = FindClossestGrabPoint(selected);
                closestGrabbable = selected;
                GrabBegin();
            });

            if (selectedChips.Count != 0)
                HandPose.SetPose(HandPoseId.ChipPose);
            else HandPose.ClearPose();

            selectedChips.Clear();
            state = GrabberState.Default;
            //StopAllCoroutines();
            //StartCoroutine(SetStateWithDelay(2f, GrabberState.Default));

            Debug.Log("EndChipSelection");

            

        }
    }
    bool changeStateInProcess = false;
    IEnumerator SetStateWithDelay(float delay, GrabberState state)
    {
        changeStateInProcess = true;
        yield return new WaitForSeconds(delay);
        photonView.RPC("SetState", RpcTarget.All, (int)state);
        changeStateInProcess = false;
    }

   
    [PunRPC]
    private void SetState(int State)
    {
        state = (GrabberState)State;       
    }
    // Hands follow the touch anchors by calling MovePosition each frame to reach the anchor.
    // This is done instead of parenting to achieve workable physics. If you don't require physics on
    // your hands or held objects, you may wish to switch to parenting.
    void OnUpdatedAnchors()
    {
        // Don't want to MovePosition multiple times in a frame, as it causes high judder in conjunction
        // with the hand position prediction in the runtime.
        if (alreadyUpdated) return;
        alreadyUpdated = true;

        Vector3 destPos = m_parentTransform.TransformPoint(m_anchorOffsetPosition);
        Quaternion destRot = m_parentTransform.rotation * m_anchorOffsetRotation;

        if (m_moveHandPosition)
        {
            GetComponent<Rigidbody>().MovePosition(destPos);
            GetComponent<Rigidbody>().MoveRotation(destRot);
        }

        if (!m_parentHeldObject)
        {
            //MoveGrabbedObject(destPos, destRot);
        }

        m_lastPos = transform.position;
        m_lastRot = transform.rotation;

        float prevFlex = m_prevFlex;

        if (photonView.IsMine)
        {
            // Update values from inputs
            m_prevFlex = OVRInput.Get(GrabAxis, m_controller);
            add_chip = OVRInput.GetDown(GrabButton, m_controller);

            if (m_grabCandidates.Count == 0 && m_grabbedObjs.Count == 0 && !changeStateInProcess)
                state = GrabberState.SelectionChips;

            if (state == GrabberState.Default)
            {
                if (m_grabCandidates.Count != 0)
                    SetOutlineForClosest();
                CheckForGrabOrRelease(prevFlex);
            }
            else
            {
                SelectionChips(prevFlex);
            }
        }

    }

   

    void OnTriggerEnter(Collider otherCollider)
    {
        if (state == GrabberState.Default)
        {
            // Get the grab trigger
            var netInfo = otherCollider.gameObject.GetComponent<ChipData>();
            OVRGrabbableCustom grabbable = otherCollider.GetComponent<OVRGrabbableCustom>() ?? otherCollider.GetComponentInParent<OVRGrabbableCustom>();
            if (grabbable == null) return;


            // Add the grabbable
            int refCount = 0;
            m_grabCandidates.TryGetValue(grabbable, out refCount);
            m_grabCandidates[grabbable] = refCount + 1;
        }

    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (state == GrabberState.Default)
        {
            OVRGrabbableCustom grabbable = otherCollider.GetComponent<OVRGrabbableCustom>() ?? otherCollider.GetComponentInParent<OVRGrabbableCustom>();
            if (grabbable == null) return;

            // Remove the grabbable
            int refCount = 0;
            bool found = m_grabCandidates.TryGetValue(grabbable, out refCount);
            if (!found)
            {
                return;
            }

            if (refCount > 1)
            {
                m_grabCandidates[grabbable] = refCount - 1;
            }
            else
            {

                m_grabCandidates.Remove(grabbable);
                if (grabbable.GetComponent<OutlineController>() != null)
                    grabbable.GetComponent<OutlineController>().DisableOutlines();

                ResetClosestObj();
            }
        }
    }



    /// <summary>
    /// функция для которая проверяет мы берем предмет, бросаем или ничего не делаем
    /// </summary>
    /// <param name="prevFlex"></param>
    protected void CheckForGrabOrRelease(float prevFlex)
    {

        if((m_prevFlex >= grabBegin) && prevFlex < grabBegin)
        {
            GrabBegin();
        }
        else if ((m_prevFlex <= grabEnd) && prevFlex > grabEnd)
        {
            GrabEnd();
        }
    }

    void DisableOutline(OVRGrabbableCustom closest)
    {
        foreach (var obj in m_grabbedObjs)
        {
            if(closest == obj) 
                continue;

            if (obj.GetComponent<OutlineController>() != null) 
                obj.GetComponent<OutlineController>().DisableOutlines();
        }

        foreach (var obj in m_grabCandidates.Keys)
        {
            if (closest == obj)
                continue;
            if (obj.GetComponent<OutlineController>() != null)
                obj.GetComponent<OutlineController>().DisableOutlines();
        }
    }
    private void SetOutlineForClosest()
    {
        UpdateCandidates();
        FindClosestGrabbableCandidate();

        if (photonView.IsMine)
        {
            if (closestGrabbable != null && closestGrabbable.GetComponent<OutlineController>() != null && photonView.IsMine)
            {
                var outline = closestGrabbable.GetComponent<OutlineController>();

                outline.EnableOutlines();
            }

            DisableOutline(closestGrabbable);
        }
        
    }

    void ResetClosestObj()
    {
        closestMagSq = float.MaxValue;      
        closestGrabbable = null;
        closestGrabbableCollider = null;
    }

    float MaxDistance = 0.3f;
    void UpdateCandidates()
    {
        var removeCandidaes = new List<OVRGrabbableCustom>();
        var deletedCandidates = new List<OVRGrabbableCustom>();

        foreach (OVRGrabbableCustom grabbable in m_grabCandidates.Keys)
        {
            if (!grabbable)
            {
                deletedCandidates.Add(grabbable);
                continue;
            }
            if (m_grabCandidates == null)
                continue;
            var itemNetInfo = grabbable.gameObject.GetComponent<OwnerData>();
            if (itemNetInfo != null)
                if (itemNetInfo.Owner != playerStat.PlayerNick)
                    removeCandidaes.Add(grabbable);

            //Debug.Log(Vector3.Distance(grabbable.transform.position, grabbleObjSpawnPoint.position));
            if (Vector3.Distance(grabbable.transform.position, grabbleObjSpawnPoint.position) > MaxDistance)
            {
                if (grabbable.gameObject.GetComponent<OVRGrabbableCustom>() != null)
                    removeCandidaes.Add(grabbable);
            }
        }
        deletedCandidates.ForEach(dc => m_grabCandidates.Remove(dc));
        

        for (var i = 0; i < removeCandidaes.Count; i++)
        {
            if (removeCandidaes[i].GetComponent<OutlineController>() != null)
                removeCandidaes[i].GetComponent<OutlineController>().DisableOutlines();
            m_grabCandidates.Remove(removeCandidaes[i]);
        }
    }


    private void FindClosestGrabbableCandidate()
    {
        ResetClosestObj();

        // Iterate grab candidates and find the closest grabbable candidate
        foreach (OVRGrabbableCustom grabbable in m_grabCandidates.Keys)
        {
            // если этот предмет уже есть продолжить 
            if (m_grabbedObjs.Contains(grabbable))
                continue;

            bool canGrab = !(grabbable.isGrabbed && !grabbable.allowOffhandGrab);
            if (!canGrab)
            {
                continue;
            }

            for (int j = 0; j < grabbable.grabPoints.Length; ++j)
            {
                Collider grabbableCollider = grabbable.grabPoints[j];
                // Store the closest grabbable
                if (grabbableCollider != null)
                {
                    Vector3 closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(m_gripTransform.position);
                    float grabbableMagSq = (m_gripTransform.position - closestPointOnBounds).sqrMagnitude;
                    if (grabbableMagSq < closestMagSq)
                    {                     

                        closestMagSq = grabbableMagSq;
                        closestGrabbable = grabbable;
                        closestGrabbableCollider = grabbableCollider;
                    }
                }
            }
        }
    }

    Collider FindClossestGrabPoint(OVRGrabbableCustom grabbable)
    {
        Collider collider = null;
        float closestMagSq = 9999999999;
        for (int j = 0; j < grabbable.grabPoints.Length; ++j)
        {
            Collider grabbableCollider = grabbable.grabPoints[j];
            // Store the closest grabbable
            if (grabbableCollider != null)
            {
                Vector3 closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(m_gripTransform.position);
                float grabbableMagSq = (m_gripTransform.position - closestPointOnBounds).sqrMagnitude;
                if (grabbableMagSq < closestMagSq)
                {

                    closestMagSq = grabbableMagSq;
                    collider = grabbableCollider;
                }
            }
        }

        return collider;
    }

    [PunRPC]
    protected virtual void GrabBegin_RPC(int viewID, int colliderIndex)
    {
        if (!photonView.IsMine)
        {

            var grabbableCollidrs = Physics.OverlapSphere(transform.position, 1f).FirstOrDefault(g => g.GetComponent<PhotonView>()?.ViewID == viewID);
          
            closestGrabbable = grabbableCollidrs.GetComponent<OVRGrabbableCustom>();
            Collider grabbableCollider = closestGrabbable.grabPoints[colliderIndex];
            // Store the closest grabbable


            Vector3 closestPointOnBounds = grabbableCollider.ClosestPointOnBounds(m_gripTransform.position);
            float grabbableMagSq = (m_gripTransform.position - closestPointOnBounds).sqrMagnitude;

            closestMagSq = grabbableMagSq;
            closestGrabbableCollider = grabbableCollider;
        }
              

        DisableOutline(closestGrabbable);
        m_grabCandidates.Clear();

        if (closestGrabbable != null)
        {
            
            //условие для искоренение возможности единовременно брать разные предметы в руку 
            if (m_grabbedObjs.Count != 0 && m_grabbedObjs.Exists(go => go.tag != closestGrabbable.tag))
                return;


            //если мы взяли наш же предмет
            if (closestGrabbable.isGrabbed && closestGrabbable.grabbedBy != this)
            {
                closestGrabbable.grabbedBy.OffhandGrabbed(closestGrabbable);
            }

            m_grabbedObj = closestGrabbable;          
            m_grabbedObjs.Add(m_grabbedObj);

            //в GrabBegin класса OVRGrabbableCustom и его наследника мы можем протисывать
            //каким образом несколько предметов могут выстраиваться у нас в руке (Карты, Монеты, Фишки и тп ...)
            m_grabbedObj.GrabBegin(this, closestGrabbableCollider);



            m_lastPos = transform.position;
            m_lastRot = transform.rotation;
          

            if (m_grabbedObj)
            {
                if (m_grabbedObj.snapOffset)
                    MoveGrabbedObject(grabbleObjSpawnPoint.position, grabbleObjSpawnPoint.rotation, true);



                SetPlayerIgnoreCollision(m_grabbedObj.gameObject, true);

                if (m_grabbedObjs.Count == max_grabbed_obj || closestGrabbable.tag == "Untagged")
                    GrabVolumeEnable(false);

            }
        }
        else
        {
            print("Grabbable collider not found!");
        }
    }
    
    public void ForceGrabBegin(OVRGrabbableCustom obj)
    {
        closestGrabbable = obj;
        GrabBegin();
    }

    protected virtual void GrabBegin()
    {
        if (photonView.IsMine && closestGrabbable != null)
        {
            var colliderIndex = closestGrabbable.grabPoints.ToList().IndexOf(closestGrabbableCollider);
            photonView.RPC("GrabBegin_RPC", RpcTarget.All, closestGrabbable.GetComponent<PhotonView>().ViewID, colliderIndex);
        }
        
    }

    protected virtual void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
    {
        if (m_grabbedObj == null)
        {
            return;
        }

        //Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;
        //Vector3 grabbablePosition = pos + rot * m_grabbedObjectPosOff;
        //Quaternion grabbableRotation = rot * m_grabbedObjectRotOff;

        Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;

        grabbedRigidbody.transform.localPosition = m_grabbedObj.snapOffset.localPosition;
        grabbedRigidbody.transform.localRotation = m_grabbedObj.snapOffset.localRotation;

      
    }

    [PunRPC]
    protected void GrabEnd_RPC()
    {
        if (m_grabbedObjs.Count != 0)
        {
            HandPose.ClearPose();
            OVRPose localPose = new OVRPose { position = OVRInput.GetLocalControllerPosition(m_controller), orientation = OVRInput.GetLocalControllerRotation(m_controller) };
            OVRPose offsetPose = new OVRPose { position = m_anchorOffsetPosition, orientation = m_anchorOffsetRotation };
            localPose = localPose * offsetPose;

            OVRPose trackingSpace = transform.ToOVRPose() * localPose.Inverse();
            Vector3 linearVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerVelocity(m_controller);
            Vector3 angularVelocity = trackingSpace.orientation * OVRInput.GetLocalControllerAngularVelocity(m_controller);

            linearVelocity *= throwForce;


           
            GrabbableReleaseAll(linearVelocity, angularVelocity);

        }
        DisableOutline(null);
        m_grabCandidates.Clear();
       
        // Re-enable grab volumes to allow overlap events
        GrabVolumeEnable(true);
    }

    protected void GrabEnd()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("GrabEnd_RPC", RpcTarget.All);
        }
    }

    protected void GrabbableRelease(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        m_grabbedObj.GrabEnd(linearVelocity, angularVelocity);
        if (m_parentHeldObject) m_grabbedObj.transform.parent = null;
        SetPlayerIgnoreCollision(m_grabbedObj.gameObject, false);

        m_grabbedObjs.Remove(m_grabbedObj);
        m_grabbedObj = null;
    }

    protected void GrabbableReleaseAll(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        for (var i = 0; i < m_grabbedObjs.Count; i++)
        {

            m_grabbedObjs[i].GrabEnd(linearVelocity, angularVelocity);
            if (m_parentHeldObject) m_grabbedObjs[i].transform.parent = null;
            SetPlayerIgnoreCollision(m_grabbedObjs[i].gameObject, false);
            
        }
     
        m_grabbedObjs.Clear();
        m_grabbedObj = null;

    }

    protected virtual void GrabVolumeEnable(bool enabled)
    {
        if (m_grabVolumeEnabled == enabled)
        {
            return;
        }

        m_grabVolumeEnabled = enabled;
        for (int i = 0; i < m_grabVolumes.Length; ++i)
        {
            Collider grabVolume = m_grabVolumes[i];
            grabVolume.enabled = m_grabVolumeEnabled;
        }

        if (!m_grabVolumeEnabled)
        {
            foreach (OVRGrabbableCustom cand in m_grabCandidates.Keys)
                DisableOutline(cand);
             
        }
    }

    protected virtual void OffhandGrabbed(OVRGrabbableCustom grabbable)
    {
        if (m_grabbedObjs.Contains(grabbable))
        {
            GrabbableRelease(Vector3.zero, Vector3.zero);
        }
    }

    protected void SetPlayerIgnoreCollision(GameObject grabbable, bool ignore)
    {
        if (m_player != null)
        {
            Collider[] playerColliders = m_player.GetComponentsInChildren<Collider>();
            foreach (Collider pc in playerColliders)
            {
                Collider[] colliders = grabbable.GetComponentsInChildren<Collider>();
                foreach (Collider c in colliders)
                {
                    Physics.IgnoreCollision(c, pc, ignore);
                }
            }
        }  
       
    }

    void OnDestroy()
    {
        if (m_grabbedObj != null)
        {
            GrabEnd();
        }
    }
}

