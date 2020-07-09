using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scipts.Roulette_table
{
    class BetGraber : OVRGrabber
    {


        public override void MoveGrabbedObject(Vector3 pos, Quaternion rot, bool forceTeleport = false)
        {           
            if (m_grabbedObj == null)
            {
                return;
            }

            Rigidbody grabbedRigidbody = m_grabbedObj.grabbedRigidbody;
            Vector3 grabbablePosition = new Vector3(pos.x, m_grabbedObj.transform.position.y, pos.z);
           

            if (forceTeleport)
            {
                grabbedRigidbody.transform.position = grabbablePosition;             
            }
            else
            {
                grabbedRigidbody.MovePosition(grabbablePosition);             
            }
        }
    }
}
