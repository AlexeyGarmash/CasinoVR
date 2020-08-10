using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StackAnimator : MonoBehaviour
{

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

    PlayerPlace pp;
    public float currentY = 0;
    public float lastY;

    public float xOffset = 0.004f;
    public float zOffset = 0.004f;

    public float yOffset = 0.0073f;

    public StackData stack;

    List<GameObject> currentObjects = new List<GameObject>();

    Coroutine prevMoveLastChips, waitToEnd;

    EventManager<ChipFieldEvents> evenmManager;
    private void Start()
    {
        evenmManager = GetComponentInParent<ChipsField>().FieldEventManager;
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
    public int AnimationFlag { get  { if (AnimationEnded) return 1; else return 0; } }
    IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(2f);
        evenmManager.PostNotification(ChipFieldEvents.StackAnimationEnded, this);
        AnimationEnded = true;
      
    }

    public void ChangeStateOfItem(bool collider, bool InAnimation)
    {
        foreach (GameObject chip in stack.Objects)
        {
            var GrabbableChip = chip.GetComponent<Collider>();
            var netInfo = chip.GetComponent<ChipData>();
            var view = chip.GetComponent<PhotonView>();
            if (GrabbableChip != null)
            {
                chip.GetComponent<Collider>().enabled = collider;

                netInfo.InAnimation = InAnimation;          

            }
        }
    }

    

    
 
    
    public void StartAnim(GameObject chip)
    {
        AnimationEnded = false;
        

        var view = chip.GetComponent<PhotonView>();

        if (view.IsMine)
            view.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

        currentObjects.Add(chip);
        chip.SetActive(false);
        chip.GetComponent<Collider>().enabled = false;
        var ItemNetworkInfo = chip.GetComponent<ChipData>();
        ItemNetworkInfo.InAnimation = true;    

        if (waitToEnd != null)
        {
            StopCoroutine(waitToEnd);
            waitToEnd = StartCoroutine(WaitToEnd());
        }
        //ChangeStateOfItem(false, true, ViewSynchronization.Off);
        if (prevMoveLastChips == null)
        {
            //UpdateStackInstantly();          
            evenmManager.PostNotification(ChipFieldEvents.StackAnimationStarted, this);
            prevMoveLastChips = StartCoroutine(MoveLastChips(chipsDropSpeed, chipsDropMult, pauseBeetwenChips));
        }       
    }
   
    IEnumerator MoveChip(float chipsDropSpeed, float chipsDropMult, GameObject chip)
    {
        
        //var currOffsetX = Random.Range(-xOffset, xOffset);
        //var currOffsetZ = Random.Range(-zOffset, zOffset);

        var pos = gameObject.transform.position;

        chip.transform.rotation = new Quaternion();

        currentY += yOffset;

        var Start = new Vector3(
                    pos.x/* + currOffsetX*/,
                    transform.position.y + currentY,
                    pos.z/* + currOffsetZ*/);
        var End = new Vector3(
                    pos.x /*+ currOffsetX*/,
                    transform.position.y + currentY + yOffsetForAnim,
                     pos.z /*+ currOffsetZ*/);

        var t = chipsDropSpeed * Time.deltaTime;

        

        chip.transform.position = End;
        

        while (Start != chip.transform.position)
        {
            yield return null;
            chipsDropSpeed *= chipsDropMult;


            chip.transform.position = Vector3.Lerp(End, Start, t);
            t += chipsDropSpeed * Time.deltaTime;
        }

        yield return null;
    }



    public void UpdateStackInstantly()
    {
        StopAllCoroutines();


        currentY = 0;
        lastY = 0;

        for (var i = 0; i < stack.Objects.Count; i++)
        {
           
            //var currOffsetX = Random.Range(-xOffset, xOffset);
            //var currOffsetZ = Random.Range(-zOffset, zOffset);

            var pos = gameObject.transform.position;

            stack.Objects[i].transform.rotation = new Quaternion();
            stack.Objects[i].transform.position = new Vector3(
                        pos.x /*+ currOffsetX*/,
                        transform.position.y + currentY,
                        pos.z /*+ currOffsetZ*/
             );
            currentY += yOffset;

        }
        currentY -= yOffset;
        //lastY = currentY;

    }

    public void Clear()
    {
        StopAllCoroutines();
        currentY = -1;
        lastY = 0;
    }
}

