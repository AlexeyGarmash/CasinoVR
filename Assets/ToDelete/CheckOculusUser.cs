using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOculusUser : MonoBehaviour
{
    private void Start()
    {
        Oculus.Platform.Core.Initialize("3300730370034575");
        var userRequest = Oculus.Platform.Users.GetLoggedInUser();
        if(userRequest != null)
        {
            userRequest.OnComplete(GetLoggedInUserCallback);
        }
        else
        {
            print("USER GET FAILED");
        }
    }

    private void GetLoggedInUserCallback(Message<User> message)
    {
        if (!message.IsError)
        {
            print($"[OCULUS CHECKER] Oculus user = {message.GetUser().ID}");
            print($"[OCULUS CHECKER] Oculus user = {message.GetUser().OculusID}");
        }
        else
        {
            print($"[OCULUS CHECKER] Error while get user = {message.GetError().Message}");
        }
    }
}
