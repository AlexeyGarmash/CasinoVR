using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Newtonsoft.Json;

public class WebLoginManager : MonoBehaviour
{
    private const string PREF_REMEMBER = "remember";
    private const string PREF_EMAIL = "email";
    private const string PREF_PASSWORD = "password";
    private const string WEB_EMAIL_FIELD = "email";
    private const string WEB_PASSWORD_FIELD = "password";
    [SerializeField] private GameObject _panelLoading;
    [SerializeField] private TMP_InputField _inputEmail;
    [SerializeField] private TMP_InputField _inputPassword;
    [SerializeField] private Toggle _toggleRememberMe;
    [SerializeField] private Button _buttonNext;
    [SerializeField] private int _minPasswordLength;
    [SerializeField] private string _loginEmailErrorMessage;
    [SerializeField] private string _loginPasswordErrorMessage;
    [SerializeField] private string _webLoginEndPoint;

    private void Start()
    {
        if(_inputPassword.contentType != TMP_InputField.ContentType.Password)
        {
            _inputPassword.contentType = TMP_InputField.ContentType.Password;
        }
        _buttonNext.onClick.AddListener(OnButtonLoginClicked);

        RestorePlayerSavedData();
    }

    private void RestorePlayerSavedData()
    {
        bool rememberedLater = PlayerPrefs.GetInt(PREF_REMEMBER) == 1;

        if(rememberedLater)
        {
            _inputEmail.text = PlayerPrefs.GetString(PREF_EMAIL);
            _inputPassword.text = PlayerPrefs.GetString(PREF_PASSWORD);
            
        }
        _toggleRememberMe.isOn = rememberedLater;
    }

    private void OnButtonLoginClicked()
    {
        string inpEmail = _inputEmail.text;
        string inpPass = _inputPassword.text;

        if(ValidateEmail(inpEmail))
        {
            if (ValidatePassword(inpPass))
            {
                OnLoginCredentSuccess(inpEmail, inpPass);
            }
            else
            {
                OnLoginError(_loginPasswordErrorMessage);
            }
        }
        else
        {
            OnLoginError(_loginEmailErrorMessage);
        }

    }

    private void OnLoginError(string loginMessage)
    {
        MainMenuInformer.Instance.ShowInfoWithExitTime(loginMessage, MainMenuMessageType.Danger);
    }

    private bool ValidateEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool ValidatePassword(string password)//simple password validation
    {
        if(password.Length < _minPasswordLength)
        {
            return false;
        }
        return true;
    }

    private void OnLoginCredentSuccess(string inpEmail, string inpPass)
    {
        print("CREDENT SUCCESS");
        if(_toggleRememberMe.isOn)
        {
            PlayerPrefs.SetInt(PREF_REMEMBER, _toggleRememberMe.isOn ? 1 : 0);
            PlayerPrefs.SetString(PREF_EMAIL, inpEmail);
            PlayerPrefs.SetString(PREF_PASSWORD, inpPass);
            PlayerPrefs.Save();
            ProceedLogin(inpEmail, inpPass);
        }
    }

    private void ProceedLogin(string inpEmail, string inpPass)
    {
        StartCoroutine(PostLoginRequest(_webLoginEndPoint, inpEmail, inpPass));
    }

    IEnumerator PostLoginRequest(string loginUri, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField(WEB_EMAIL_FIELD, email);
        form.AddField(WEB_PASSWORD_FIELD, password);

        using (UnityWebRequest www = UnityWebRequest.Post(loginUri, form))
        {
            EnableLoadingPanel(true);
            yield return new WaitForSeconds(2f);
            yield return www.SendWebRequest();
            EnableLoadingPanel(false);
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                OnLoginError(www.error);
            }
            else
            {
                Debug.Log("Auth SUCCESS");
                //print(www.downloadHandler.text);
                var jsonSuccess = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(www.downloadHandler.text);
                /*dynamic jsonSuccess = JsonUtility.FromJson<dynamic>(www.downloadHandler.text);*/
                print(jsonSuccess["token"]);
                MainMenuManager.Instance.OnPlayerWebLoginSuccess();
            }
        }
    }

    private void EnableLoadingPanel(bool activate)
    {
        _panelLoading.SetActive(activate);
    }
}