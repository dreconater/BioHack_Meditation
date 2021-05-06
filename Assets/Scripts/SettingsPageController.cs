using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TalesFromTheRift;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPageController : MonoBehaviour
{
    public Button SendEmailBtn;
    public Button CloseBtn;

    public InputField EmailField;

    public GameObject EmailErrorText;
    public GameObject EmailSuccesText;

    public OpenCanvasKeyboard VRKeyboard;

    public Action SettingsPanelClosed;

    void Awake()
    {
        SendEmailBtn.onClick.AddListener(()=> { StartCoroutine(SendEmail()); });
        CloseBtn.onClick.AddListener(Close);
    }

    IEnumerator SendEmail() {
        if (EmailField.text.Contains("@") && EmailField.text.Contains("."))
        {
            //Send To Server
            Debug.Log("Sending Email");
            SendingEmail();
            EmailErrorText.SetActive(false);
            EmailSuccesText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            Debug.Log("Wrong Email");
            EmailErrorText.SetActive(true);
            EmailSuccesText.SetActive(false);
        }
    }

    void SendingEmail()
    {

        Debug.Log("Sending Email");

        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("team@biohack.network");
        mail.To.Add("team@biohack.network");
        mail.Subject = "VR subscriber from " + EmailField.text;
        mail.Body = "VR subscriber from " + EmailField.text;

        SmtpClient smtpServer = new SmtpClient("smtp-relay.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential("team@biohack.network", "work_hard@_11!") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(mail);

    }

    void Close()
    {
        VRKeyboard.CloseKeyboard();
        EmailErrorText.SetActive(false);
        EmailSuccesText.SetActive(false);
        SettingsPanelClosed.Invoke();
    }
}
