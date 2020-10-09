using Assets.Scipts.Chips;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scipts
{
    public class CurveAnimator : MonoBehaviourPun
    {
        [SerializeField]
        Vector3 rotation;
        [SerializeField]
        bool RandomRotation = true;
        [SerializeField]
        public BezierCurve[] curves;

        [SerializeField]
        protected float speed = 0.1f;
        [SerializeField]
        protected float MinSpeed = 1;

        [SerializeField]
        protected float speedSlowingStep = 0.5f;

        [SerializeField]
        public List<GameObject> ObjectToAnimation;

        [SerializeField]
        public bool animStarted = true;

        protected Dictionary<int, bool> playerdCoroutinesEnded;
        protected Dictionary<int, Coroutine> movingCoroutines;
        protected void Start()
        {
            ObjectToAnimation = new List<GameObject>();
            curves = GetComponentsInChildren<BezierCurve>();
            playerdCoroutinesEnded = new Dictionary<int, bool>();
            movingCoroutines = new Dictionary<int, Coroutine>();

        }
    
        public void StopObjectAnimation(int viewID)
        {
            Coroutine coroutine;
         
            if (movingCoroutines.TryGetValue(viewID, out coroutine))
            {
                StopCoroutine(coroutine);
                movingCoroutines[viewID] = null;
                playerdCoroutinesEnded[viewID] = true;
            }
        }


        [PunRPC]
        public void StartAnimation_RPC(int curveIndex)
        {
            StartCoroutine(MoveObjectWithCurve(curveIndex));
        }
        IEnumerator MoveObjectWithCurve(int curveIndex)
        {
            playerdCoroutinesEnded = new Dictionary<int, bool>();
            movingCoroutines = new Dictionary<int, Coroutine>();

            yield return new WaitForSeconds(0.1f);
            int i = 0;
            while (ObjectToAnimation.Count > i)
            {
                var curvePurpel = curves[curveIndex];
                var g_object = ObjectToAnimation[i];
                i++;
                //winChips.Remove(chip);
                var viewID = g_object.GetComponent<PhotonView>().ViewID;

                movingCoroutines.Add(viewID,  StartCoroutine(MoveOneObject(curvePurpel, g_object, viewID)));
                yield return new WaitForSeconds(0.1f);

            }

            while (playerdCoroutinesEnded.Values.ToList().Exists(c => !c))
                yield return null;

            ObjectToAnimation.Clear();
            playerdCoroutinesEnded.Clear();
            movingCoroutines.Clear();
            animStarted = false;

        }

        IEnumerator MoveOneObject(BezierCurve curvePurpel, GameObject chip, int viewID)
        {
            playerdCoroutinesEnded.Add(viewID, false);

            chip.GetComponent<OwnerData>().animator = this;

            var localSpeed = speed;
            float t = 0;
            t += Time.deltaTime * speed;
            chip.SetActive(true);


            chip.GetComponent<SoundsPlayer>().PlayRandomClip();



            chip.GetComponent<Collider>().enabled = false;
            chip.GetComponent<Rigidbody>().isKinematic = true;

            chip.transform.rotation = Quaternion.Euler(rotation);





            while (t != 1)
            {


                if (t > 0.95)
                    t = 1;

                chip.transform.position = curvePurpel.GetPointAt(t);
                if (RandomRotation)
                    chip.transform.Rotate(Random.Range(10f, 30f), Random.Range(10f, 30f), Random.Range(10f, 30f));
                yield return null;
                t += Time.deltaTime * localSpeed;

                if (t > 0.95)
                {
                    t = 1;
                    chip.transform.position = curvePurpel.GetPointAt(t);
                    if (RandomRotation)
                        chip.transform.Rotate(Random.Range(10f, 30f), Random.Range(10f, 30f), Random.Range(10f, 30f));
                }
            }

            chip.GetComponent<Collider>().enabled = true;
            chip.GetComponent<Rigidbody>().isKinematic = false;
            chip.GetComponent<OwnerData>().animator = null;
            yield return null;

           
            playerdCoroutinesEnded[viewID] = true;
            
            chip.GetComponent<OwnerData>().animator = null;
        }
     

    }
}


