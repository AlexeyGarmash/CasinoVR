using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchIndicator : MonoBehaviour
{
    

    [SerializeField]
    Material WatchIndicatorMaterial;
    [SerializeField]
    Color DefaultColor;
    [SerializeField]
    Color WarningColor;
    [SerializeField]
    Color ErrorColor;

    [SerializeField]
    Transform warningHalo;
    [SerializeField]
    Transform errorHalo;


    [SerializeField]
    float beamTime = 1f;
    [SerializeField]
    float beamPauseMult = 0.3f;

    float currentBeamTime;
    float currentTime;

    bool animationStarted = false;
    bool beamTick = true;
    WatchSounds watchSounds;
    private void Start()
    {
        watchSounds = GetComponent<WatchSounds>();
        warningHalo.gameObject.SetActive(false);
        errorHalo.gameObject.SetActive(false);

      
    }

    public void StartIndicatorAnimation(float time)
    {
        StartCoroutine(StartAnimation(time));
    }

    public void StopAnimation()
    {
        StopAllCoroutines();
        warningHalo.gameObject.SetActive(false);
        errorHalo.gameObject.SetActive(false);
        WatchIndicatorMaterial.color = DefaultColor;
        animationStarted = false;
        watchSounds.StopTimer();
    }

    IEnumerator StartAnimation(float time)   
    {
        currentTime = 0;
        currentBeamTime = beamTime;
        beamTick = true;
        animationStarted = true;
        watchSounds.PlayTimer(time);

        while (time > currentTime)
        {

            if (currentTime < time / 2 && beamTick)
            {
                beamTick = false;
              
                WatchIndicatorMaterial.color = WarningColor;
                warningHalo.gameObject.SetActive(true);
            }
            else if (currentTime < time && beamTick)
            {
                beamTick = false;
             
                WatchIndicatorMaterial.color = ErrorColor;
                errorHalo.gameObject.SetActive(true);
            }
            else {
                beamTick = true;
                WatchIndicatorMaterial.color = DefaultColor;
                errorHalo.gameObject.SetActive(false);
                warningHalo.gameObject.SetActive(false);
            }

            currentBeamTime = (time - currentTime) / time * beamPauseMult;

            yield return new WaitForSeconds(currentBeamTime);
        }

        animationStarted = false;
    }

    private void Update()
    {
        if(animationStarted)
            currentTime += Time.deltaTime;
    }








}
