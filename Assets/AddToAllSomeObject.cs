using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AddToAllSomeObject : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private bool _run;

    private void Update()
    {
        if (_run)
        {
            var finded = FindObjectsOfType<RoulettedBettingField>();
            foreach (var item in finded)
            {
                GameObject pref = Instantiate(_object, item.transform);
            }
            _run = false;
        }
    }
}
