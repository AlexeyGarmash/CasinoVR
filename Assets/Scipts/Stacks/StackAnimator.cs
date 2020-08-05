using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StackAnimator : MonoBehaviour
{

    [SerializeField]
    public const float chipsDropSpeed = 0.4f;

    [SerializeField]
    public const float chipsDropMult = 1.1f;

    [SerializeField]
    public const float pauseBeetwenChips = 0.1f;


    [SerializeField]
    public float yOffsetForAnim = 0.2f;

    [SerializeField]
    public float delayForCollidersEnabled = 2f;

    public float currentY;
    public float lastY;

    public float xOffset = 0.004f;
    public float zOffset = 0.004f;

    public float yOffset = 0.0073f;

    public StackData stack;
    private List<GameObject> toRemove;

    private void Start()
    {

        stack = GetComponent<StackData>();
        toRemove = new List<GameObject>();

    }
    Coroutine waitEnd;
    IEnumerator MoveLastChips(float chipsDropSpeed, float chipsDropMult, float pause)
    {
        EnabledColliders(false);
        bool haveUnactiveObjects = true;

        while (haveUnactiveObjects && newChipsComming)
        {
            newChipsComming = false;
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
                    toRemove.Add(currentObjects[i]);

                }

                
            }
           
        }

        yield return new WaitForSeconds(1f);

        EnabledColliders(true);

        RemovelocalChip();

        prevMoveLastChips = null;

        yield return null;


    }

    private void EnabledColliders(bool isEnabled)
    {
        foreach (GameObject chip in stack.Objects)
        {
            var GrabbableChip = chip.GetComponent<Collider>();

            if (GrabbableChip != null)
            {

                GrabbableChip.enabled = isEnabled;
            }


        }
    }
    List<GameObject> currentObjects = new List<GameObject>();

    Coroutine prevMoveLastChips;
    bool newChipsComming = false;

    
    [PunRPC]
    public void UpdateChipsList_RPC(int viewID, int color)
    {
        var currChip = currentObjects.Find(c => c.GetComponent<NetworkInfo>().ViewId == viewID);

        var newChip = Instantiate(ChipUtils.Instance.GetPrefabByColor((Chips)color), currChip.transform.position, currChip.transform.rotation);


        Destroy(currChip);

        var rb = newChip.GetComponent<Rigidbody>();
        //var collider = newChip.GetComponent<Collider>();

        rb.isKinematic = true;
        //collider.enabled = false;


        newChip.transform.parent = stack.transform;

        stack.Objects.Add(newChip);
       


    }
    public void StartAnim(GameObject chip)
    {
        //EnabledColliders(false);
        chip.SetActive(false);

      
        currentObjects.Add(chip);
        chip.GetComponent<Collider>().enabled = false;
        newChipsComming = true;

        if (prevMoveLastChips == null)
        {
            prevMoveLastChips = StartCoroutine(MoveLastChips(chipsDropSpeed, chipsDropMult, pauseBeetwenChips));
        }       
    }
    void RemovelocalChip()
    {
        Debug.Log(currentObjects.Count);
        Debug.Log(toRemove.Count);

        foreach (GameObject chip in toRemove)
        {
            var netInfo = chip.GetComponent<NetworkInfo>();

            if (netInfo.isMine)
            {

                var view = GetComponent<PhotonView>();
                var chipData = chip.GetComponent<ChipData>();
              
                view.RPC("UpdateChipsList_RPC", RpcTarget.All, netInfo.ViewId, (int)chipData.Cost);
               


            }
            
        }
       
        toRemove.Clear();
        currentObjects.Clear();
    }
    IEnumerator MoveChip(float chipsDropSpeed, float chipsDropMult, GameObject chip)
    {
        


        var currOffsetX = Random.Range(-xOffset, xOffset);
        var currOffsetZ = Random.Range(-zOffset, zOffset);

        var pos = gameObject.transform.position;

        chip.transform.rotation = new Quaternion();


        var Start = new Vector3(
                    pos.x + currOffsetX,
                    transform.position.y + currentY,
                    pos.z + currOffsetZ);
        var End = new Vector3(
                    pos.x + currOffsetX,
                    transform.position.y + currentY + yOffsetForAnim,
                     pos.z + currOffsetZ);

        var t = chipsDropSpeed * Time.deltaTime;


        chip.transform.position = End;
        currentY += yOffset;

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

            var currOffsetX = Random.Range(-xOffset, xOffset);
            var currOffsetZ = Random.Range(-zOffset, zOffset);

            var pos = gameObject.transform.position;

            stack.Objects[i].transform.rotation = new Quaternion();
            stack.Objects[i].transform.position = new Vector3(
                        pos.x + currOffsetX,
                        transform.position.y + currentY,
                        pos.z + currOffsetZ
             );

            currentY += yOffset;
        }

        //lastY = currentY;

    }

    public void Clear()
    {
        StopAllCoroutines();
        currentY = 0;
        lastY = 0;
    }
}

