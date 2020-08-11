﻿using Assets.Scipts.Chips;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scipts
{
    public class CurveAnimator : MonoBehaviourPun
    {
        [SerializeField]
        protected BezierCurve[] curves;

        [SerializeField]
        protected float speed = 0.1f;
        [SerializeField]
        protected float MinSpeed = 1;

        [SerializeField]
        protected float speedSlowingStep = 0.5f;

        protected void Start()
        {
            ObjectToAnimation = new List<GameObject>();
            curves = GetComponentsInChildren<BezierCurve>();

        }
        protected List<GameObject> ObjectToAnimation;

        [PunRPC]
        public void StartAnimation_RPC(int curveIndex)
        {
            StartCoroutine(MoveObjectWithCurve(curveIndex));
        }
        IEnumerator MoveObjectWithCurve(int curveIndex)
        {

            yield return new WaitForSeconds(1f);
      
            int i = 0;
            while (ObjectToAnimation.Count > i)
            {
                var curvePurpel = curves[curveIndex];
                var g_object = ObjectToAnimation[i];
                i++;
                //winChips.Remove(chip);

                StartCoroutine(MoveOneObject(curvePurpel, g_object));
                yield return new WaitForSeconds(0.1f);

            }

            yield return null;

        }
        IEnumerator MoveOneObject(BezierCurve curvePurpel, GameObject chip)
        {
            var localSpeed = speed;
            float t = 0;
            t += Time.deltaTime * speed;
            chip.SetActive(true);

            chip.GetComponent<Collider>().enabled = false;
            chip.GetComponent<Rigidbody>().isKinematic = true;
            while (t != 1)
            {
                //localSpeed -= speedSlowingStep;
                if (t > 0.95)
                    t = 1;
                chip.transform.position = curvePurpel.GetPointAt(t);
                chip.transform.Rotate(UnityEngine.Random.Range(10f, 30f), Random.Range(10f, 30f), Random.Range(10f, 30f));
                yield return null;
                t += Time.deltaTime * localSpeed;
                if (t > 0.95)
                    t = 1;
            }

            chip.GetComponent<Collider>().enabled = true;
            chip.GetComponent<Rigidbody>().isKinematic = false;

            yield return null;

        }
        private void OnTriggerEnter(Collider other)
        {

            var chip = other.gameObject.GetComponent<OwnerData>();
            var view = other.gameObject.GetComponent<PhotonView>();

            if (chip != null && view != null)
            {
                ObjectToAnimation.Add(other.gameObject);

                other.gameObject.SetActive(false);
                view.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

            }

        }

    }
}
