using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GrabbableChip : OVRGrabbableCustom
{
    [Header("Позиции лувой руки с фишками 1-5 штуками"), Space(10)]

    [SerializeField]
    private  Transform GrabPos1ChipLeft;
    [SerializeField]
    private Transform GrabPos2ChipLeft;
    [SerializeField]
    private Transform GrabPos3ChipLeft;
    [SerializeField]
    private Transform GrabPos4ChipLeft;
    [SerializeField]
    private Transform GrabPos5ChipLeft;


    [Header("Позиции правой руки с фишками 1-5 штуками"), Space(10)]
    [SerializeField]
    private Transform GrabPos1ChipRight;
    [SerializeField]
    private Transform GrabPos2ChipRight;
    [SerializeField]
    private Transform GrabPos3ChipRight;
    [SerializeField]
    private Transform GrabPos4ChipRight;
    [SerializeField]
    private Transform GrabPos5ChipRight;


    [Header("Сдвиг холдера для каждой новой фишки правой руки 1-5 штуками"), Space(10)]

    [SerializeField]
    private Vector3 offsetPosR;
   
    [Header("Поворот холдера для правой руки"), Space(10)]

    [SerializeField]
    private  Quaternion offsetRotR;


    [Header("Сдвиг холдера для каждой новой фишки левой руки 1-5 штуками"), Space(10)]

    [SerializeField]
    private Vector3 offsetPosL;

    [Header("Поворот холдера для левой руки"), Space(10)]

    [SerializeField]  
    public Quaternion offsetRotL;



    float yOffset = 0.0075f;
    private OvrAvatar avatar;

    protected override void Start()
    {
        base.Start();

        
    }
    public override void GrabBegin(OVRGrabberCustom hand, Collider grabPoint)
    {

        base.GrabBegin(hand, grabPoint);


        var grabbleObjSpawnPoint = hand.grabbleObjSpawnPoint;

        //делаем так как фишки повернуты не правельно
        grabbleObjSpawnPoint.transform.rotation = new Quaternion(hand.transform.rotation.x - 90, hand.transform.rotation.y, hand.transform.rotation.z, hand.transform.rotation.w);

        //делаем фишку ребенком холдера
        transform.parent = hand.grabbleObjSpawnPoint;

        
        //Делаем поворот холдера 0 0 0 в глобальных координатах (необходтмо для ровного выстраивания фишек в руке)
        grabbleObjSpawnPoint.rotation = new Quaternion();

        //запоминаем родителя холдера и убераем родителя на холдера (необходтмо для ровного выстраивания фишек в руке )
        var parent = grabbleObjSpawnPoint.parent;
        grabbleObjSpawnPoint.parent = null;


        //-------------начало выстраивания фишек -------------
        var zStart = 0f;
        var m_grabbedObjs = hand.m_grabbedObjs;

        //идем по взятым фишкам
        for (var i = 0; i < m_grabbedObjs.Count; i++)
        {
            //задаем координаты фишки с шагом
            m_grabbedObjs[i].transform.rotation = new Quaternion();
            m_grabbedObjs[i].transform.parent = grabbleObjSpawnPoint;
            m_grabbedObjs[i].transform.localPosition = new Vector3(0, 0, zStart);       

            zStart -= yOffset;

        }


        grabbleObjSpawnPoint.parent = parent;


        //берем аватар с игрока и задаем ему анимацию бля коректного отображения фишек в руке
        avatar = gameObject.transform.parent.parent.parent.GetComponentInChildren<OvrAvatar>();
        SetPose(hand);
       
    }

    void SetPose(OVRGrabberCustom hand)
    {
        
        var holder = hand.grabbleObjSpawnPoint;
        if (hand.m_controller == OVRInput.Controller.RTouch)
        {
            
            if (hand.m_grabbedObjs.Count == 1)
            {
                avatar.RightHandCustomPose = GrabPos1ChipRight;
                holder.localPosition = offsetPosR;
            }
            else if (hand.m_grabbedObjs.Count == 2)
            {
                avatar.RightHandCustomPose = GrabPos2ChipRight;
                holder.localPosition = offsetPosR;
            }
            else if (hand.m_grabbedObjs.Count == 3)
            {
                avatar.RightHandCustomPose = GrabPos3ChipRight;
                holder.localPosition = offsetPosR;
            }
            else if (hand.m_grabbedObjs.Count == 4)
            {
                avatar.RightHandCustomPose = GrabPos4ChipRight;
                holder.localPosition = offsetPosR;
            }
            else if (hand.m_grabbedObjs.Count == 5)
            {
                avatar.RightHandCustomPose = GrabPos5ChipRight;
                holder.localPosition = offsetPosR;
            }

            holder.localRotation = offsetRotR;
        }
        else if (hand.m_controller == OVRInput.Controller.LTouch)
        {
            if (hand.m_grabbedObjs.Count == 1)
            {
                avatar.RightHandCustomPose = GrabPos1ChipLeft;
                holder.localPosition = offsetPosL;
            }
            else if (hand.m_grabbedObjs.Count == 2)
            {
                avatar.RightHandCustomPose = GrabPos2ChipLeft;
                holder.localPosition = offsetPosL;
            }
            else if (hand.m_grabbedObjs.Count == 3)
            {
                avatar.RightHandCustomPose = GrabPos3ChipLeft;
                holder.localPosition = offsetPosL;
            }
            else if (hand.m_grabbedObjs.Count == 4)
            {
                avatar.RightHandCustomPose = GrabPos4ChipLeft;
                holder.localPosition = offsetPosL;
            }
            else if (hand.m_grabbedObjs.Count == 5)
            {
                avatar.RightHandCustomPose = GrabPos5ChipLeft;
                holder.localPosition = offsetPosL;
            }

            holder.localRotation = offsetRotL;
        }
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        transform.parent = null;         
        avatar.RightHandCustomPose = null;     
        avatar.LeftHandCustomPose = null;
        
    }
}

