using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GrabbableChip : OVRGrabbableCustom
{
    

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
       
    }

   

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);

        var rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;

        transform.parent = null;         
        //avatar.RightHandCustomPose = null;     
        //avatar.LeftHandCustomPose = null;
        
    }
}

