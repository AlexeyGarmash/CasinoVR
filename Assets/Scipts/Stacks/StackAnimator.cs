using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StackAnimator : MonoBehaviour
{
    [SerializeField]
    public bool useObjectRotation = true;
    [SerializeField]
    public bool useZOffset = false;
    [SerializeField]
    public bool useYOffset = true;
    [SerializeField]
    public bool useXOffset = false;
    [SerializeField]

    public bool useRandomXZOfset = true;
    [SerializeField]



    protected Vector3 objectRotation;



    [SerializeField]
    public const float chipsDropSpeed = 0.9f;

    [SerializeField]
    public const float chipsDropMult = 1.1f;

    [SerializeField]
    public const float pauseBeetwenChips = 0.1f;


    [SerializeField]
    public float yOffsetForAnim = 0.2f;

    [SerializeField]
    public float delayForCollidersEnabled = 2f;

    public float currentY = 0;
    public float currentX = 0;
    public float currentZ = 0;
    public float lastY;

    public float xOffset = 0.004f;
    public float zOffset = 0.004f;

    public float yOffset = 0.0073f;

    public StackData stack;

    List<GameObject> currentObjects = new List<GameObject>();

    Coroutine prevMoveLastChips, waitToEnd;

    EventManager<AbstractFieldEvents> evenmManager;
    private void Start()
    {
        evenmManager = GetComponentInParent<AbstractField>().FieldEventManager;
        if (evenmManager == null)
            Debug.LogError("event manager is null");
        stack = GetComponent<StackData>();


    }
    IEnumerator MoveLastChips(float chipsDropSpeed, float chipsDropMult, float pause)
    {
        bool haveUnactiveObjects = true;

        while (haveUnactiveObjects)
        {

            haveUnactiveObjects = false;
            for (var i = 0; i < currentObjects.Count; i++)
            {
                if (!currentObjects[i].activeSelf)
                {
                    currentObjects[i].SetActive(true);

                    stack.StartCoroutine(
                            MoveChip(chipsDropSpeed, chipsDropMult, currentObjects[i])
                        );
                    haveUnactiveObjects = true;
                    yield return new WaitForSeconds(pause);


                }
            }

        }

        prevMoveLastChips = null;
        StartCoroutine(WaitToEnd());

        yield return null;

    }

    private bool AnimationEnded = true;
    public int AnimationFlag { get { if (AnimationEnded) return 1; else return 0; } }
    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(2f);
        currentObjects.Clear();
        //stack.Objects.ForEach(s =>
        //{
        //    s.GetComponent<PhotonSyncCrontroller>().SyncOn_Photon();
        //    s.GetComponent<Collider>().enabled = true;
        //    s.GetComponent<ChipData>().InAnimation = false;
        //});
        evenmManager.PostNotification(AbstractFieldEvents.StackAnimationEnded, this);
        AnimationEnded = true;

    }

    public void ChangeStateOfItem(bool collider)
    {
        foreach (GameObject chip in stack.Objects)
        {
            var GrabbableChip = chip.GetComponent<Collider>();

            if (GrabbableChip != null)
            {
                chip.GetComponent<Collider>().enabled = collider;

            }
        }
    }






    public void StartAnim(GameObject chip)
    {
        AnimationEnded = false;


        var view = chip.GetComponent<PhotonView>();
        view.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

        currentObjects.Add(chip);
        chip.SetActive(false);
        chip.GetComponent<Collider>().enabled = false;

        if (waitToEnd != null)
        {
            StopCoroutine(waitToEnd);
            waitToEnd = StartCoroutine(WaitToEnd());
        }

        if (prevMoveLastChips == null)
        {
            //UpdateStackInstantly();          
            evenmManager.PostNotification(AbstractFieldEvents.StackAnimationStarted, this);
            prevMoveLastChips = StartCoroutine(MoveLastChips(chipsDropSpeed, chipsDropMult, pauseBeetwenChips));
        }
    }

    private (Vector3 Start, Vector3 End) GetStartEndPointsForAnimation()
    {
       

        Vector3 Start, End;
        Start = transform.position;

        if (useYOffset)       
            Start = new Vector3(Start.x, Start.y + currentY, Start.z);        
        if(useXOffset)
            Start = new Vector3(Start.x + currentX, Start.y, Start.z);
        if (useZOffset)
            Start = new Vector3(Start.x, Start.y, Start.z + zOffset);

        End = new Vector3(Start.x, Start.y + yOffsetForAnim, Start.z);

        currentY += yOffset;
        currentX += xOffset;
        currentZ += zOffset;

        return (End, Start);
    }

    private void ZeroCurrentXYZ()
    {
        currentY = 0;
        currentX = 0;
        currentZ = 0;
    }
    IEnumerator MoveChip(float chipsDropSpeed, float chipsDropMult, GameObject chip)
    {
        
        //var currOffsetX = Random.Range(-xOffset, xOffset);
        //var currOffsetZ = Random.Range(-zOffset, zOffset);

        chip.transform.localRotation = Quaternion.Euler(objectRotation.x, objectRotation.y, objectRotation.z);

            

        var t = chipsDropSpeed * Time.deltaTime;

        

        var point = GetStartEndPointsForAnimation();

        chip.transform.position = point.Start;

        while (point.End != chip.transform.position)
        {
            yield return null;
            chipsDropSpeed *= chipsDropMult;


            chip.transform.position = Vector3.Lerp(point.Start, point.End, t);
            t += chipsDropSpeed * Time.deltaTime;
        }

        yield return null;
    }



    public void UpdateStackInstantly()
    {

        stack.Objects.ForEach(c => c.SetActive(true));
        StopAllCoroutines();


        currentY = 0;
        lastY = 0;
        ZeroCurrentXYZ();

        for (var i = 0; i < stack.Objects.Count; i++)
        {

            //var currOffsetX = Random.Range(-xOffset, xOffset);
            //var currOffsetZ = Random.Range(-zOffset, zOffset);

            var pos = GetStartEndPointsForAnimation().End;

            stack.Objects[i].transform.localRotation = Quaternion.Euler(objectRotation.x, objectRotation.y, objectRotation.z);
            stack.Objects[i].transform.position = pos;           

        }
        currentY -= yOffset;
        //lastY = currentY;

    }

    public void Clear()
    {
        StopAllCoroutines();
        currentY = 0;
        lastY = 0;
    }
}

