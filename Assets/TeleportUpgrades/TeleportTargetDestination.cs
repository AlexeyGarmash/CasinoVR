using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTargetDestination : MonoBehaviour
{
    [SerializeField] private Light PlaceLight;

    
    private MeshRenderer destMeshRender;
    private AudioSource audioSrc;


    private void Start()
    {
        if (PlaceLight == null) PlaceLight = GetComponentInChildren<Light>();
        destMeshRender = GetComponent<MeshRenderer>();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = false;
        ActivateDestination(false);
        
    }

    bool playerStay = false;
    IEnumerator WaitEnd()
    {
        yield return new WaitForSeconds(0.2f);
        playerStay = false;
    }
    private void OnTriggerStay(Collider other)
    {     
        
        var teleportArrow = other.GetComponent<TeleportDestination>();
        if (teleportArrow != null && other.tag == "TeleportTarget")
        {
            playerStay = true;
            ActivateDestination(true);
            //PlaySound();
            StopAllCoroutines();
            StartCoroutine(WaitEnd());
        }

    }

    private void Update()
    {
        if (!playerStay)
        {
            ActivateDestination(false);
            playerStay = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        var teleportArrow = other.GetComponent<TeleportDestination>();
        if(teleportArrow != null)
        {
            ActivateDestination(true);
            PlaySound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var teleportArrow = other.GetComponent<TeleportDestination>();
        if (teleportArrow != null)
        {
            ActivateDestination(false);

        }
    }

    private void ActivateDestination(bool activate)
    {
        destMeshRender.enabled = activate;
        PlaceLight.enabled = activate;
    }

    private void PlaySound()
    {      
        audioSrc.Play();
    }
}
