using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetClient : MonoBehaviour
{
    public static NetClient instance;
    private TcpClient client;
    private NetworkStream stream;
    private byte[] receiveBuffer = new byte[1024];

    [Header("Server Settings")]
    public string serverIP = "127.0.0.1"; 
    public int port = 7777;

    [Header("World Data")]
    [SerializeField] private float _globalBestScore = 0f;
    [SerializeField] private float _totalDistance = 0f;
    [SerializeField] private int _serverPlayerCount = 0;

    public float GlobalBestScore => _globalBestScore;
    public float TotalDistance => _totalDistance;
    public int ServerPlayerCount => _serverPlayerCount; 

    public Action<float> OnBestScoreReceived;
    public Action<int> OnPlayerCountUpdated;
    public Action<float> OnTotalDistanceUpdated;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            //ConnectToServer();
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public bool TryConnectToServer()
    {
        try
        {
            client = new TcpClient();
            client.Connect(serverIP, port);
            stream = client.GetStream();
            Debug.Log("<color=green>서버 접속 성공!</color>");
            stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnRead, null);
            
            RequestBestScore();
            RequestTotalDistance();
            RequestServerPlayerCount();
            return true;
        }
        catch (Exception e) 
        { 
            Debug.LogWarning($"<color=red>서버 접속 실패! 게임을 중단합니다.</color> 사유: {e.Message}");
            return false;
        }
    }

    void OnRead(IAsyncResult ar) // 해석
    {
        try
        {
            if (stream == null || !stream.CanRead) return;

            int bytesRead = stream.EndRead(ar);
            if (bytesRead <= 0) return;

            string message = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
            string[] packets = message.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string packet in packets)
            {
                Debug.Log("<color=blue>서버 수신:</color> " + packet);
                UnpackDataFromServer(packet); 
            }

            if (client.Connected && stream != null && stream.CanRead)
            {
                stream.BeginRead(receiveBuffer, 0, receiveBuffer.Length, OnRead, null);
            }
        }
        catch (Exception e) 
        { 
            if (client != null && client.Connected)
                Debug.LogError("데이터 수신 에러: " + e.Message); 
        }
    }

    void UnpackDataFromServer(string msg) // 분류 및 실행
    {
        string[] parts = msg.Split(':');
        string head = parts[0];

        if (head == "GLOBAL_BEST") 
        {
            if (float.TryParse(parts[1], out float receivedScore)) 
            {
                _globalBestScore = receivedScore;  
                Debug.Log("최고점 데이터를 성공적으로 수신하였습니다.!!!");
                Debug.Log($"<color=yellow>전세계 최고점 업데이트: {_globalBestScore}</color>");
                OnBestScoreReceived?.Invoke(_globalBestScore);
            }
        }
        else if (head == "PLAYER_COUNT")
        {
            if (int.TryParse(parts[1], out int count)) {
                _serverPlayerCount = count;
                OnPlayerCountUpdated?.Invoke(_serverPlayerCount);
            }
        }
        else if (head == "TOTAL_DIST")
        {
            if (float.TryParse(parts[1], out float dist)) {
                _totalDistance = dist;
                OnTotalDistanceUpdated?.Invoke(_totalDistance);
            }
        }
    }

    public void ManualSendData(string message)
    {
        if (stream == null || !client.Connected) return;
        byte[] buffer = Encoding.UTF8.GetBytes(message + "\n");
        stream.Write(buffer, 0, buffer.Length);
    }

    public void SendHighestScoreToServer(float score)
    {
        Debug.Log($"<color=white>서버로 점수를 전송해봅니다!: {score}</color>");
        ManualSendData($"SCORE:{score}");
    }

    public void AddDistanceToServer(float distance)
    {
        Debug.Log($"<color=white>서버에게 기록 누적을 요청합니다!: {distance}</color>");
        ManualSendData($"ADD_DIST:{distance}");
    }

    public void RequestBestScore()
    {
        Debug.Log("서버에게 최고점 데이터 송신을 요청했습니다.");
        ManualSendData("GET_HIGHEST_SCORE"); 
    }

    public void RequestTotalDistance()
    {
        Debug.Log("서버에게 누적 거리 송신을 요청했습니다.");
        ManualSendData("GET_TOTAL_DIST");
    }

    public void RequestServerPlayerCount()
    {
        ManualSendData("GET_PLAYER_COUNT");
    }
    
    

    void OnApplicationQuit() { if (client != null) client.Close(); }
}