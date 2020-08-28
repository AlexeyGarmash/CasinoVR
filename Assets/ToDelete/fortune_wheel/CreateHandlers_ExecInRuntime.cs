using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CreateHandlers_ExecInRuntime : MonoBehaviour
{
    [SerializeField] private GameObject GrabCylinder;

    public bool start;

    private void Update()
    {
        if(start)
        {
            var parents = GetComponentsInChildren<FollowPhysics>();

            foreach(var par in parents)
            {
                GameObject obj = Instantiate(GrabCylinder, par.transform);
            }
            start = false;
        }
    }
}
