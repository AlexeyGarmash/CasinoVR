using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerCardSlot : MonoBehaviour
{
    public GameObject presentedCardObject { get; set; }
    public float MoveCardTime = 3f;
    public Vector3 rotateAxis;
    public float rotateAngle;

    public void ReceiveCard(GameObject pokerCard)
    {
        if (pokerCard != null && presentedCardObject == null)
        {
            presentedCardObject = pokerCard;
            StartCoroutine(StartMoveCard());
        }
    }

    public IEnumerator StartMoveCard()
    {
        presentedCardObject.transform.parent = transform;
        float elapsedTime = 0f;
        while(elapsedTime <= MoveCardTime)
        {
            presentedCardObject.transform.position = Vector3.Lerp(presentedCardObject.transform.position, transform.position, (elapsedTime / MoveCardTime));
            presentedCardObject.transform.rotation = Quaternion.Lerp(presentedCardObject.transform.rotation, Quaternion.AngleAxis(rotateAngle, rotateAxis), (elapsedTime / MoveCardTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        presentedCardObject.transform.localPosition = Vector3.zero;
        presentedCardObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        presentedCardObject.transform.localRotation = Quaternion.AngleAxis(rotateAngle, rotateAxis);
        yield return null;
    }
}
