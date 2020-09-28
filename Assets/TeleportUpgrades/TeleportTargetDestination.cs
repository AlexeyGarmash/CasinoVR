using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTargetDestination : MonoBehaviour
{
    [SerializeField] private AudioClip clipAbleTeleport;

    private MeshRenderer destMeshRender;
    private AudioSource audioSrc;


    private void Start()
    {
        destMeshRender = GetComponent<MeshRenderer>();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = false;
        ActivateDestination(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("some trigger me");
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
    }

    private void PlaySound()
    {
        audioSrc.clip = clipAbleTeleport;
        audioSrc.Play();
    }
}
