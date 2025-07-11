using UnityEngine;
using System.IO; // System.IO 名前空間をインポート
using System;

public class SAVE_Gamedata : MonoBehaviour
{
    [SerializeField] GameSystem gameSystem;
     
    public DateTime dateTime;


    public float timeRemaining = 60.5f;

    // データを保存するファイル名（実行環境に応じてパスを調整してください）
    // Application.persistentDataPath は、OSごとに永続的なデータを保存するための推奨パスです。
    // Windows: C:\Users\ユーザー名\AppData\LocalLow\会社名\ゲーム名
    // macOS: ~/Library/Application Support/会社名/ゲーム名
    // Android: /storage/emulated/0/Android/data/com.会社名.ゲーム名/files
    // iOS: Application/data/Container/Documents
    private string filePath;

    void Start()
    {
        
        // ファイルパスを初期化
        // 例えば "game_data.txt" というファイル名で保存する場合
        filePath = Path.Combine(Application.persistentDataPath, "game_data.txt");
        Debug.Log("Saving data to: " + filePath);
        
    }
    
    // データをファイルに保存するメソッド
    public void SaveData(float Score)
    {
        try
        {

            

            dateTime = DateTime.Now;
            string todayDateString = dateTime.ToString("yyyy-MM-dd");

            filePath = Path.Combine(Application.persistentDataPath, "game_data.txt"+ todayDateString);
            
            
            int newlineCount = 0;
          

            if (File.Exists(filePath))
            { // ファイルが存在する場合のみ読み込む
                string loadedData = File.ReadAllText(filePath);
                foreach (char c in loadedData)
                {
                    if (c == '\n') // 改行文字 '\n' を検出
                    {
                        newlineCount++;
                    }
                }
            }
           

            string FinishcurrentTimeString = dateTime.ToString("HH:mm:ss");

            // 保存するデータを作成
            string dataToSave = "";


            if (newlineCount == 0) 
            {
                dataToSave = $"Day,StartTime,FinishGame,Score,PlayCount \n";
                newlineCount = 1;
            }
            dataToSave += $"{todayDateString:F2},{gameSystem.StartcurrentTimeString},{FinishcurrentTimeString},{Score},{newlineCount}\n";

            // ファイルにデータを書き込む
            // true を指定すると追記モード、false を指定すると上書きモードになります
            File.AppendAllText(filePath, dataToSave); // 追記モード
            // File.WriteAllText(filePath, dataToSave); // 上書きモード

            Debug.Log("Data saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data: {e.Message}");
        }
    }

    // ファイルからデータを読み込むメソッド
    public void LoadData()
    {
        try
        {
            if (File.Exists(filePath))
            {
                // ファイルからすべてのテキストを読み込む
                string loadedData = File.ReadAllText(filePath);
                Debug.Log("Loaded Data:\n" + loadedData);

                int newlineCount = 0;
               

                foreach (char c in loadedData)
                {
                    if (c == '\n') // 改行文字 '\n' を検出
                    {
                        newlineCount++;
                    }
                }
                Debug.Log($"ファイルの改行数: {newlineCount}個");

                // ここで読み込んだデータをパースして利用する処理を記述
                // 例えば、特定のキーワードを検索して値を取り出すなど
                // 例: "Score: " の後ろの数字を取得
                string[] lines = loadedData.Split('\n');
                foreach (string line in lines)
                {
                    if (line.StartsWith("Score: "))
                    {
                        string scoreStr = line.Replace("Score: ", "").Trim();
                        if (int.TryParse(scoreStr, out int loadedScore))
                        {
                            Debug.Log($"Loaded Score: {loadedScore}");
                            // this.score = loadedScore; // 必要であれば変数に代入
                        }
                    }
                    else if (line.StartsWith("Time: "))
                    {
                        string timeStr = line.Replace("Time: ", "").Replace(" seconds", "").Trim();
                        if (float.TryParse(timeStr, out float loadedTime))
                        {
                            Debug.Log($"Loaded Time: {loadedTime}");
                            // this.timeRemaining = loadedTime; // 必要であれば変数に代入
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("Save file not found!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load data: {e.Message}");
        }
    }

    // テスト用のボタンなどにアタッチして実行
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData(100);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }
}
