using Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Assets.Scipts.Animators;
using Assets.Scipts.Chips;
using Assets.Scipts.BackJack;

public class CroupierBlackJackNPC : MonoBehaviourPun
{
    Animator animator;
  
    const int numberOfDancingsForWin = 8;

    [SerializeField]
    float percentageOfTakeCardToSpawnCards = 0.55f;
    [SerializeField]
    float defaultDelay = 0.1f;

    //animator triggers
    const string toTakeCard = "toTakeCard";
    const string toGiveCard = "toGiveCard";
    const string toDefaultIdle = "toDefaultIdle";
    const string WinDance = "WinDance";
    const string LoseDance = "LoseDance";
    const string EndDance = "EndDance";

    //animators float
    const string DanceNumber = "DanceNumber";
    [SerializeField]
    private Vector3 rotationTOPos1;
    [SerializeField]
    private Vector3 rotationTOPos2;
    [SerializeField]
    private Vector3 rotationTOPos3;
    [SerializeField]
    private Vector3 rotationTOPos4;
    [SerializeField]
    private Vector3 rotationTOPos5;

    private Vector3 defaultRotation;

    [SerializeField]
    private StackData LeftHandNPCStack;
    [SerializeField]
    private StackData RightHandNPCStack;

    [SerializeField]
    Transform pleyer1Pos;
    [SerializeField]
    Transform pleyer2Pos;
    [SerializeField]
    Transform pleyer3Pos;
    [SerializeField]
    Transform pleyer4Pos;
    [SerializeField]
    Transform pleyer5Pos;
    [SerializeField]
    Transform dillerPos;

    [SerializeField]
    CardCurveAnimator CardCurve;

    Dictionary<int, Transform> playersPositions;

    Dictionary<int, List<CardData>> CardsToPlayers;

    Dictionary<int, List<GameObject>> CardsToPlayersGObj;

    Dictionary<int, Vector3> npcRotations;

    public bool distributionOfCards;

    [SerializeField]
    public BlackJackPlayerCardField BlackJackDilerCardFieldOpen;
    [SerializeField]
    public BlackJackPlayerCardField BlackJackDilerCardFieldHidden;

    [HideInInspector]
    public TakeCardBehaviour takeCardBehaviour;
    [HideInInspector]
    public GiveCardAnimBehavior giveCardBehavour;
    [HideInInspector]
    public WinDanceBehavour winDanceBehavour;
    [HideInInspector]
    public LoseDanceBehaviour loseDanceBehavour;

    GameObject instantiaterdCardObj;

   
    void Start()
    {
        defaultRotation = transform.localEulerAngles;
        animator = GetComponent<Animator>();

        takeCardBehaviour = animator.GetBehaviour<TakeCardBehaviour>();
        giveCardBehavour = animator.GetBehaviour<GiveCardAnimBehavior>();
        winDanceBehavour = animator.GetBehaviour<WinDanceBehavour>();
        loseDanceBehavour = animator.GetBehaviour<LoseDanceBehaviour>();

        InitDictionarys();

    }
    [PunRPC]
    void GenerateRandomDanceForRemotePlayers(int danceNumber)
    {
        animator.SetFloat(DanceNumber, danceNumber);
    }
    public IEnumerator StartRandomWinDance()
    {
        if (photonView.IsMine)
        {
            var rnd = UnityEngine.Random.Range(0, numberOfDancingsForWin - 1);
            animator.SetInteger(DanceNumber, rnd);

            photonView.RPC("GenerateRandomDanceForRemotePlayers",  RpcTarget.OthersBuffered, rnd);
        }

        yield return new WaitForSeconds(3f);

        animator.SetTrigger(EndDance);
    }
    public void WinAnimation()
    {
        animator.SetTrigger(WinDance);
    }

    public void LoseAnimation()
    {
        animator.SetTrigger(LoseDance);
    }

    public void EndDanceAnimation()
    {
        animator.SetTrigger(EndDance);
    }
    public void ResetValues()
    {
        BlackJackDilerCardFieldOpen.ClearStacks();
        BlackJackDilerCardFieldHidden.ClearStacks();
        ResetAnimation();
    }

    void InitDictionarys()
    {

        npcRotations = new Dictionary<int, Vector3>();

        npcRotations.Add(0, rotationTOPos1);
        npcRotations.Add(1, rotationTOPos2);
        npcRotations.Add(2, rotationTOPos3);
        npcRotations.Add(3, rotationTOPos4);
        npcRotations.Add(4, rotationTOPos5);
        npcRotations.Add(5, defaultRotation);


        playersPositions = new Dictionary<int, Transform>();


        playersPositions.Add(0, pleyer1Pos);
        playersPositions.Add(1, pleyer2Pos);
        playersPositions.Add(2, pleyer3Pos);
        playersPositions.Add(3, pleyer4Pos);
        playersPositions.Add(4, pleyer5Pos);

        playersPositions.Add(5, dillerPos);
      


        CardsToPlayers = new Dictionary<int, List<CardData>>();


        CardsToPlayers.Add(0, new List<CardData>());
        CardsToPlayers.Add(1, new List<CardData>());
        CardsToPlayers.Add(2, new List<CardData>());
        CardsToPlayers.Add(3, new List<CardData>());
        CardsToPlayers.Add(4, new List<CardData>());

        CardsToPlayers.Add(5, new List<CardData>());
        
        CardsToPlayersGObj = new Dictionary<int, List<GameObject>>();


        CardsToPlayersGObj.Add(0, new List<GameObject>());
        CardsToPlayersGObj.Add(1, new List<GameObject>());
        CardsToPlayersGObj.Add(2, new List<GameObject>());
        CardsToPlayersGObj.Add(3, new List<GameObject>());
        CardsToPlayersGObj.Add(4, new List<GameObject>());

        CardsToPlayersGObj.Add(5, new List<GameObject>());
    }
    public void AddCardToHand(int player, CardData card)
    {
        CardsToPlayers[player].Add(card);
    }

    public void ResetAnimation()
    {
        //animator.SetTrigger(triggerToDefIdleCard);
    }



    float speedRotation = 2;
    IEnumerator rotateNPC(Vector3 rotation)
    {
        var localRotation = transform.localRotation;
        float t = 0;
        while (t < 1)
        {
            transform.localRotation = Quaternion.Lerp(localRotation, Quaternion.Euler(rotation), t);
            t += Time.deltaTime * speedRotation;
            yield return null;
        }

    }

    [PunRPC]
    void InstantiateRemote(int viewID, int face, int sign, int players)
    {
        instantiaterdCardObj = Instantiate(CardUtils.Instance.GetCard((Card_Face)face, (Card_Sign)sign));
        instantiaterdCardObj.GetComponent<PhotonView>().ViewID = viewID;
        instantiaterdCardObj.GetComponent<PhotonSyncCrontroller>().SyncOff_RPC();

        CardsToPlayersGObj[players].Add(instantiaterdCardObj);

        instantiaterdCardObj.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

        var rb = instantiaterdCardObj.GetComponent<Rigidbody>();
        var chip = instantiaterdCardObj.GetComponent<CardData>();

        rb.isKinematic = true;
        chip.transform.parent = RightHandNPCStack.transform;


        RightHandNPCStack.Objects.Add(instantiaterdCardObj);
        RightHandNPCStack.StartAnim(instantiaterdCardObj);
    }
    private IEnumerator TakeCardsToRightHand()
    {
        RightHandNPCStack.ClearData();

       
        animator.SetTrigger(toTakeCard);
        takeCardBehaviour.currentPercentage = 0;

        Debug.Log("TakeCardsToRightHand npc started");
        while (true)
        {         
            if (takeCardBehaviour.currentPercentage > percentageOfTakeCardToSpawnCards)
            {
                foreach (var players in CardsToPlayers.Keys)
                {
                    instantiaterdCardObj = null;

                    foreach (var card in CardsToPlayers[players])
                    {
                        CardCurve.StopAllCoroutines();



                        if (photonView.IsMine)
                        {
                            instantiaterdCardObj = Instantiate(CardUtils.Instance.GetCard(card.Face, card.Sign));
                            var view = instantiaterdCardObj.GetComponent<PhotonView>();
                            instantiaterdCardObj.GetComponent<PhotonSyncCrontroller>().SyncOff_RPC();

                            PhotonNetwork.AllocateViewID(view);


                            photonView.RPC("InstantiateRemote", RpcTarget.Others, view.ViewID, (int)card.Face, (int)card.Sign, players);
                            PhotonNetwork.SendAllOutgoingCommands();
                        }

                        while (instantiaterdCardObj == null)
                            yield return null;



                            CardsToPlayersGObj[players].Add(instantiaterdCardObj);

                            instantiaterdCardObj.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

                            var rb = instantiaterdCardObj.GetComponent<Rigidbody>();
                            var chip = instantiaterdCardObj.GetComponent<CardData>();

                            rb.isKinematic = true;
                            chip.transform.parent = RightHandNPCStack.transform;


                            RightHandNPCStack.Objects.Add(instantiaterdCardObj);
                            RightHandNPCStack.StartAnim(instantiaterdCardObj);
                        
                        
                        
                    }

                }

                break;

            }
            yield return null;

        }
    }

    private IEnumerator TakeCardToLeftHand()
    {
        Debug.Log("TakeCardToLeftHand npc started");
        while (true)
        {
            if(takeCardBehaviour.currentPercentage < 0.9f)
                takeCardBehaviour.currentPercentage = 0;

            if (takeCardBehaviour.currentPercentage >= 1f)
            {
                var objecksToExtract = new List<GameObject>();

                RightHandNPCStack.Objects.ForEach(o => objecksToExtract.Add(o));


                RightHandNPCStack.ExtractAll();

                objecksToExtract.ForEach(o => {

                    o.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

                    var rb = o.GetComponent<Rigidbody>();
                    var chip = o.GetComponent<CardData>();

                    rb.isKinematic = true;
                    chip.transform.parent = LeftHandNPCStack.transform;

                    LeftHandNPCStack.Objects.Add(o);
                    LeftHandNPCStack.StartAnim(o);
                });


                break;
            }
            yield return null;
        }
    }

    private IEnumerator GiveCardsToPlayers(bool hideCard)
    {
        Debug.Log("GiveCardsToPlayers npc started");
        int i = 0;

        while (CardsToPlayersGObj.Values.ToList().Exists(cards => cards.Count != 0))
        {
            if (hideCard)
            {
                if (i == 0)
                {
                    BlackJackDilerCardFieldHidden.BlockField(true);
                    BlackJackDilerCardFieldOpen.BlockField(false);
                }
                else
                {
                    BlackJackDilerCardFieldHidden.BlockField(false);
                    BlackJackDilerCardFieldOpen.BlockField(true);
                }
            }

            foreach (var playerPos in CardsToPlayersGObj.Keys)
            {

                var playerCards = CardsToPlayersGObj[playerPos];

               

                if (playerCards.Count != 0)
                {
                    CardCurve.animStarted = false;
                    Debug.Log("CNP give card to " + playerPos);

                    var card = playerCards[0];
                    playerCards.Remove(card);
                    LeftHandNPCStack.ExtractOne(card);

                    card.GetComponent<PhotonSyncCrontroller>().SyncOff_Photon();

                    var rb = card.GetComponent<Rigidbody>();
                    var chip = card.GetComponent<CardData>();

                    rb.isKinematic = true;


                    StartCoroutine(rotateNPC(npcRotations[playerPos]));

                    chip.transform.parent = RightHandNPCStack.transform;

                    RightHandNPCStack.Objects.Add(card);
                    RightHandNPCStack.StartAnim(card);

                   
                   
                    animator.SetTrigger(toGiveCard);
                    giveCardBehavour.currentPercentage = 0;

                    yield return null;

                    while (true)
                    {
                       

                        if (giveCardBehavour.currentPercentage > 0.7f)
                        {

                            RightHandNPCStack.ExtractOne(card);

                            CardCurve.ObjectToAnimation.Add(card);
                            CardCurve.StartAnimCardToPlayer(playerPos);

                            while (CardCurve.animStarted)
                                yield return null;

                            break;

                        }
                        yield return null;
                    }

                }


                yield return new WaitForSeconds(defaultDelay);
            }
           
            i++;
            
        }
    }
    public IEnumerator TakeCardsToPlayers(bool hideCard)
    {
        distributionOfCards = true;


        yield return TakeCardsToRightHand();

        yield return new WaitForSeconds(defaultDelay);

        yield return TakeCardToLeftHand();
       

        yield return new WaitForSeconds(defaultDelay);

        yield return GiveCardsToPlayers(hideCard);

        yield return new WaitForSeconds(defaultDelay);

        distributionOfCards = false;

        foreach (var keys in CardsToPlayers.Keys)
        {
            CardsToPlayers[keys].Clear();
            CardsToPlayersGObj[keys].Clear();
            RightHandNPCStack.ClearData();
            LeftHandNPCStack.ClearData();
          
        }


        StartCoroutine(rotateNPC(defaultRotation));

        CardCurve.animStarted = false;
        yield return null;
    }

    public void TakeCards(bool hideCard)
    {
        
        StartCoroutine(TakeCardsToPlayers(hideCard));
    }

}
