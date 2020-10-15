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
    public float OffsetForAnim = 0.2f;

    [SerializeField]
    public bool animByX = false;
    [SerializeField]
    public bool animByY = false;
    [SerializeField]
    public bool animByZ = false;

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

    protected BoxCollider BoxCollider;
    float startedColliderZPos;
    public bool AnimationInitialized => stack != null;
    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        if (BoxCollider)
            startedColliderZPos = BoxCollider.center.z;

        if (!AnimationInitialized)
            InitStackAnimator();


    }

    void InitStackAnimator()
    {
        evenmManager = GetComponentInParent<AbstractField>().FieldEventManager;
        if (evenmManager == null)
            Debug.LogError("event manager is null");
        stack = GetComponent<StackData>();
    }
    IEnumerator MoveLastObject(float chipsDropSpeed, float chipsDropMult, float pause)
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
                            MoveObject(chipsDropSpeed, chipsDropMult, currentObjects[i])
                        );
                    haveUnactiveObjects = true;
                    yield return new WaitForSeconds(pause);


                }
            }

        }        

        if (BoxCollider)
        {
            BoxCollider.center = new Vector3(BoxCollider.center.x, BoxCollider.center.y, startedColliderZPos + currentZ);
            BoxCollider.enabled = true;
        }
        prevMoveLastChips = null;

       

        StartCoroutine(WaitToEnd());
       
        yield return null;

    }

    private int numberEndedAnimations;
    private bool AnimationEnded = true;
    public int AnimationFlag { get { if (AnimationEnded) return 1; else return 0; } }
    IEnumerator WaitToEnd()
    {
        while (currentObjects.Count != numberEndedAnimations)
        {
            yield return null;
        }
      
        currentObjects.Clear();
        if(evenmManager != null)
         evenmManager.PostNotification(AbstractFieldEvents.StackAnimationEnded, this);
        AnimationEnded = true;
        numberEndedAnimations = 0;

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

        if (!AnimationInitialized)
            InitStackAnimator();

        if (BoxCollider)
            BoxCollider.enabled = false;

        AnimationEnded = false;


        var view = chip.GetComponent<PhotonView>();
        view.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

        currentObjects.Add(chip);
        chip.SetActive(false);
        chip.GetComponent<Collider>().enabled = false;

       

        if (prevMoveLastChips == null)
        {
            //UpdateStackInstantly();
            if(evenmManager != null)
                evenmManager.PostNotification(AbstractFieldEvents.StackAnimationStarted, this);
            prevMoveLastChips = StartCoroutine(MoveLastObject(chipsDropSpeed, chipsDropMult, pauseBeetwenChips));
        }
    }

    private (Vector3 Start, Vector3 End) GetStartEndPointsForAnimation()
    {
       

        Vector3 Start, End;
        Start = Vector3.zero;

        if (useYOffset)       
            Start = new Vector3(Start.x, Start.y + currentY, Start.z);        
        if(useXOffset)
            Start = new Vector3(Start.x + currentX, Start.y, Start.z);
        if (useZOffset)
            Start = new Vector3(Start.x, Start.y, Start.z + currentZ);

        if(animByX)
            End = new Vector3(Start.x + OffsetForAnim, Start.y , Start.z);
        else if (animByY)
            End = new Vector3(Start.x, Start.y + OffsetForAnim, Start.z);
        else if (animByZ)
            End = new Vector3(Start.x, Start.y, Start.z + OffsetForAnim);
        else End = new Vector3(Start.x, Start.y, Start.z);

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
    IEnumerator MoveObject(float chipsDropSpeed, float chipsDropMult, GameObject chip)
    {
         

        chip.transform.localRotation = Quaternion.Euler(objectRotation.x, objectRotation.y, objectRotation.z);

        var t = chipsDropSpeed * Time.deltaTime;

        var point = GetStartEndPointsForAnimation();

        chip.transform.localPosition = point.Start;

        while (point.End != chip.transform.localPosition)
        {
            yield return null;
            chipsDropSpeed *= chipsDropMult;


            chip.transform.localPosition = Vector3.Lerp(point.Start, point.End, t);
            t += chipsDropSpeed * Time.deltaTime;

            chip.GetComponent<SoundsPlayer>().PlayRandomClip();
        }

        numberEndedAnimations++;
        yield return null;
    }



    public void UpdateStackInstantly()
    {

        stack.Objects.ForEach(c => c.SetActive(true));
        StopAllCoroutines();


        //currentZ -= zOffset;
        ZeroCurrentXYZ();

        for (var i = 0; i < stack.Objects.Count; i++)
        {
            //var currOffsetX = Random.Range(-xOffset, xOffset);
            //var currOffsetZ = Random.Range(-zOffset, zOffset);

            var pos = GetStartEndPointsForAnimation().End;

            stack.Objects[i].transform.localRotation = Quaternion.Euler(objectRotation.x, objectRotation.y, objectRotation.z);
            stack.Objects[i].transform.localPosition = pos;           

        }
        //currentZ = zOffset;


        if (BoxCollider)
            BoxCollider.center = new Vector3(BoxCollider.center.x, BoxCollider.center.y, startedColliderZPos + currentZ);
       

    }

    public void Clear()
    {
        StopAllCoroutines();
        ZeroCurrentXYZ();
        currentObjects.Clear();
    }
}

