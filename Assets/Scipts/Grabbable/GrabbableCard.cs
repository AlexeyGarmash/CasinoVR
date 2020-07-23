using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class GrabbableCard : OVRGrabbableCustom
{

    //private OvrAvatar avatar;
       
       
    [SerializeField]
    public Vector3 handCardsPos;

    [SerializeField]
    public Quaternion cardRotationFace;

    [SerializeField]
    public Transform handPos;

     
    float xOffset = 0.01f;
    float yOffset = -0.001f;
    float zOffset = -0.006f;

    float yRotStep = 6f;

    public override void GrabBegin(OVRGrabberCustom hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);
            
           
        var grabbleObjSpawnPoint = hand.grabbleObjSpawnPoint;          
        transform.parent = hand.grabbleObjSpawnPoint;

           
         
        //запоминаем родителя холдера ,обнуляем его обнуляем поворот холдера(необходимо для коректного задания позиции карт)
        var parent = grabbleObjSpawnPoint.parent;          
        grabbleObjSpawnPoint.parent = null;                
        grabbleObjSpawnPoint.rotation = new Quaternion();



        var m_grabbedObjs = hand.m_grabbedObjs;
        var cardNumer = m_grabbedObjs.Count;

        //вычисление стартовых позициий и поторотов для карт
        float startY = 0f;
        float startX = xOffset * cardNumer / 2;
        float startZ = 0f;

            
        float startRot = cardNumer / 2 * yRotStep;

        for (var i = 0; i < m_grabbedObjs.Count; i++)
        {
            //изменение позиции
            m_grabbedObjs[i].transform.parent = grabbleObjSpawnPoint;
            m_grabbedObjs[i].transform.localPosition = new Vector3(
                startX,
                startY,
                startZ
            );
               
            //вычисление следующей позиции
            if (startX <= 0)
                startZ += zOffset;
            startY += yOffset;
            startX -= xOffset;

            //изменение поворота по Y
            UnityEditor.TransformUtils.SetInspectorRotation(
                    m_grabbedObjs[i].transform, 
                new Vector3(0, startRot, 0)
            );              

            startRot -= yRotStep;
        }


        //возврат родителя и корректних локальних координат холдера для удержания карт
        grabbleObjSpawnPoint.parent = parent;
        grabbleObjSpawnPoint.localPosition = handCardsPos;

            
        //установка корректного поворота холдера
        UnityEditor.TransformUtils.SetInspectorRotation(
            grabbleObjSpawnPoint.transform,
            new Vector3(
                cardRotationFace.x,
                cardRotationFace.y, 
                cardRotationFace.z
            )
        );



        //установка позы для ударжания карт в руке
        //avatar = gameObject.transform.parent.parent.parent.GetComponentInChildren<OvrAvatar>();
        //avatar.RightHandCustomPose = handPos;

    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        transform.parent = null;

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        //avatar.RightHandCustomPose = null;
        //avatar.LeftHandCustomPose = null;
    }

}

