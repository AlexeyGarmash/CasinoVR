using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReadyToPlay : MonoBehaviourPun
{
    [SerializeField] private UnityEvent _onClickReady;
    [SerializeField] private float _holdTime;
    [SerializeField] private Image _progressImage;
    [SerializeField] private TMP_Text _textReady;
    [SerializeField] private RawImage _imageReady;

    [SerializeField] private Texture _readyTexture;
    [SerializeField] private Texture _inGameTexture;

    private bool inProgress = false;
    private float currentHoldTime = 0f;
    private bool inGame = false;

    private void Start()
    {
        _imageReady.texture = _inGameTexture;
        _progressImage.fillAmount = 0f;
    }

    
    private void Update()
    {
        if (inProgress)
        {
            ShowProgress();
        }
    }

    private void ShowProgress()
    {
        currentHoldTime += Time.deltaTime;
        if (currentHoldTime >= _holdTime)
        {
            //
            if (!inGame)
            {
                InvokeReadyToGame();
            }
            ResetProgress();
        }
        FillImageProgress(currentHoldTime / _holdTime);
    }

    private void InvokeReadyToGame()
    {
        photonView?.RequestOwnership();

    }

    

    private void ResetProgress()
    {
        inProgress = false;
        currentHoldTime = 0f;
        FillImageProgress(currentHoldTime);
    }

    private void FillImageProgress(float progress)
    {
        _progressImage.fillAmount = progress;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            //if (!photonView.IsMine) return;
            inProgress = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LongClickHand>() != null)
        {
            //if (!photonView.IsMine) return;
            ResetProgress();
        }
    }

}
