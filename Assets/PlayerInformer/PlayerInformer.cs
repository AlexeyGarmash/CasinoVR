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
    [SerializeField] private GameObject PlayerController;
    [SerializeField] private TMP_Text _textMessage;
    [SerializeField] private Transform _confettiSpawnPoint;
    [SerializeField] private GameObject _confettiPrefab;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
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

            if(win == WIN_STATUS.WIN || win == WIN_STATUS.LOSE)
            {
                /*var rotation = transform.rotation;
                var eulerRoatation = rotation.eulerAngles;
                eulerRoatation.y += 90;*/
                GameObject confettiGo = Instantiate(_confettiPrefab, _confettiSpawnPoint.position, _confettiPrefab.transform.rotation);
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
