using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum WIN_STATUS
{
    WIN,
    LOSE,
    NOTHING
}

public class PlayerInformer : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMessage;
    [SerializeField] private Transform _confettiSpawnPoint;
    [SerializeField] private GameObject _confettiPrefab;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetMessage(string message, WIN_STATUS win)
    {
        if (message != null)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            _textMessage.text = message;

            if(win == WIN_STATUS.WIN)
            {
                GameObject confettiGo = Instantiate(_confettiPrefab, _confettiSpawnPoint.position, Quaternion.identity);
                Destroy(confettiGo, 2f);
            }
        }
    }

    public void HideMessage()
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }


}
