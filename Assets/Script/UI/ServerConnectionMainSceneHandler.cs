using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ServerConnectionMainSceneHandler : MonoBehaviour
{
    [FormerlySerializedAs("ㅁIpFieldInstance")]
    [SerializeField]
    TMP_InputField IPField;
    [FormerlySerializedAs("ㅁPortFieldInstance")]
    [SerializeField]
    TMP_InputField PORTField;

    NetClient server => NetClient.instance;
    
    private void Start()
    {
        if (NetClient.instance == null)
        {
            Debug.LogError("Failed TO Find Server Handler!");
            Destroy(this.gameObject);
        }
    }


    public void TryConnectToServer()
    {
        server.serverIP = IPField.text;
        if (int.TryParse(PORTField.text, out int port))
        {
            server.port = port;
        }
        else
        {
            Debug.LogWarning("Port Is Not a Number!");
            return;
        }
        
        if (server.TryConnectToServer())
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogWarning("Faile To Match Connection To Server!");
        }
    }
}
