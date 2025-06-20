using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;

using System.Text;
using UnityEngine;
using System.Collections; // 必須
using UnityEngine;       // Unity関連クラスを使うための必須ディレクティブ
using UnityEditor.VersionControl;
using System.Collections.Generic;

public class UDPClient : MonoBehaviour
{
    [SerializeField] PlayerUDP playerUDP;
    [SerializeField] AudioTest Atest;

    public List<LocalIPClass> localIPClasses = new List<LocalIPClass>();
    public int ChoiceIP = 0;

    public float BubbleV;
    public float BeerV;

    public int BeerNumberAdd =0;
    public int BubbleNumberAdd = 0;

    public bool A = false;

    public float OverTime = 0;


    // IPアドレスは自分のではなくホスト（Android）をいれる
    private UdpClient udpClient;
    public string serverIP = "192.168.3.29"; // Androidホスト端末のローカルIPアドレス
    public int serverPort = 12345;
    public int messageCode = 0;
    public String AutLocal_IP;

    [SerializeField] AudioSource audioSource;
    [SerializeField] GameSystem gameSystem;

    void Start()
    {
        BubbleV = 999;
        BeerV = 999;

        int i = 0;
        foreach (LocalIPClass l in localIPClasses) 
        {

            if (i == ChoiceIP) { AutLocal_IP = l.IP; }
            i++;
        }

        SentStartMessage();

        /*AutLocal_IP = Dns.GetHostEntry(Dns.GetHostName())
                 .AddressList
                 .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                 .ToString();*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            SentStartMessage();
        }

        if (Input.GetMouseButtonDown(2)) // 右クリック検知
        {
            SendJsonData("磁石接触", 0);
        }
        if (Input.GetMouseButtonUp(2)) // 右クリック検知
        {
            SendJsonData("磁石分離", 0);
        }


        if (Input.GetMouseButton(1)) // 右クリック検知
        {
            messageCode = 1;
        }
        else if (Input.GetMouseButton(0)) // 左クリック検知
        {
            messageCode = 2;
        }
        else 
        {
            messageCode = 0;
        }
        
        SendMessage(messageCode.ToString()); // メッセージ送信
        
        /*
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SentStartMessage();
            PlayAudio();
        }*/

        if (Input.GetKeyDown(KeyCode.Space))
        {/*
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                PlayAudio();
            });*/
        }
    }

    public void SentStartMessage() 
    {

        //Debug.Log("通信開始");
    udpClient = new UdpClient();
        SendMessage("Hello from PC Client!");
        udpClient.BeginReceive(OnDataReceived, null);
    }

    public void SendMessage(string message)
    {
        /*serverIP = Dns.GetHostEntry(Dns.GetHostName())
                 .AddressList
                 .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                 .ToString();*/

        //serverIP = AutLocal_IP;

        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.Send(data, data.Length, AutLocal_IP, serverPort);
        //Debug.Log("Sent: " + message);

        udpClient.BeginReceive(OnDataReceived, null);
    }

    public void SendJsonData(string variableName, float value)
    {
        if (serverPort != null)
        {
            // JSON形式でデータを送信
            Debug.Log("Json送信");
            string jsonData = $"{{\"variable\":\"{variableName}\",\"value\":{value}}}";
            byte[] data = Encoding.UTF8.GetBytes(jsonData);

            udpClient.Send(data, data.Length, AutLocal_IP, serverPort);
            Console.WriteLine($"Sent: {jsonData}");
        }
        else
        {
            Debug.LogWarning("No client endpoint available to send the PLAY_AUDIO command.");
        }
    }

    void OnDataReceived(IAsyncResult result)
    {
        
        try
        {
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
            byte[] data = udpClient.EndReceive(result, ref serverEndPoint);
            string message = Encoding.UTF8.GetString(data);

            //Debug.Log("Received from host: " + message);

            // JSON解析を試行
            try
            {
                var receivedData = JsonUtility.FromJson<ReceivedData>(message);
                // 変数が "Beer" の場合に処理を実行
                
                if (receivedData.variable == "BeerV")
                {
                    //Debug.Log("Variable is Beer! Value: " + receivedData.value);
                    BeerV = receivedData.value;
                    // 必要な処理を追加
                    /*
                    if (receivedData.value == 1)
                    {
                        Debug.Log("Let's drink some beer!");
                    }
                    else
                    {
                        Debug.Log("No beer value matched.");
                    }*/
                }
                if (receivedData.variable == "BubbleV")
                {
                    //Debug.Log("Variable is Bubble! Value: " + receivedData.value);
                    BubbleV = receivedData.value;
                }
                if (receivedData.variable == "PutInBubbleV")
                {
                    Debug.Log("泡が入れられた回数: " + receivedData.value);
                    BubbleNumberAdd = (int)receivedData.value;
}
                if (receivedData.variable == "PutInBeerV")
                {
                    Debug.Log("ビールが入れられた回数: " + receivedData.value);
                    BeerNumberAdd = (int)receivedData.value;
                }
                if (receivedData.variable == "開始")
                {
                    gameSystem.GameStart();
                    Debug.Log("ゲーム開始Android");
                }
                if (receivedData.variable == "OverBeers")
                {


                    OverTime = receivedData.value;
                   
                }
            }
            catch
            {
                // JSONデータでない場合、単純なメッセージを処理
                if (message == "PLAY_AUDIO")
                {
                    A = true; // フラグを立てる
                    //Debug.Log("Flag A is set to true via plain message");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error" + ex.Message);
        }
        finally
        {
            udpClient.BeginReceive(OnDataReceived, null); // 再度受信待機
        }
    }
    

    void OnDestroy()
    {
        udpClient?.Close();
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient.Dispose();
        }
    }
    

    public void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Playing audio on client side");
            Debug.Log("オーディオソースあるよ");
        }
        else 
        {
            Debug.Log("オーディオソースないよ");
        }
    }
    [System.Serializable]
    public class ReceivedData
    {
        public string variable; // 変数名
        public float value; // 値
    }

}

[System.Serializable]
public class LocalIPClass
{

    public string IPName;
    public string IP;

}