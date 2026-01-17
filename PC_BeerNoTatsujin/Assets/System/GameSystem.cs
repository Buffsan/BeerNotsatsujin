using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameSystem : MonoBehaviour
{

    public float GameCout=0;
    public float GameTimer = 100;

    private Coroutine currentZoomCoroutine;

    public List<BeerEvaluation> beerEvaluations = new List<BeerEvaluation>();

    
    public List<AudioClip> BGMs = new List<AudioClip>();
    public List<AudioClip> audios = new List<AudioClip>();
    AudioManager Amanager => AudioManager.instance;

    Queue<bool> F_push = new Queue<bool>();
    bool F_pop = false;

    [SerializeField] Camera maincamera;
    [SerializeField] GameObject Cunvas;
    [SerializeField] GameObject Nokori50;
    bool N50 = false;
    [SerializeField] GameObject Nokori15;
    [SerializeField] Animator CountTextanimator;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] SAVE_Gamedata gamedata;
    bool N15 = false;

    int LastCount = 15;
    float StartCount = 0;
    public int StartLastCunt = 4;
    public int ResaltPhase = 0;
    float ResaltCount = 0;

    public int ScoreNow=0;

    public bool BeerStanbyOK = false;

    [SerializeField] Animator Kanban;
    [SerializeField] Animator ResaltB;
    [SerializeField] TextMeshProUGUI GameCountText;
    [SerializeField] GameObject CountTextPrefab;
    [SerializeField] Eight_Controller eightController;
    [SerializeField] BeerController beerController;
    [SerializeField] UDPClient udp_Client;
    [SerializeField] ResaltBoard resaltBoard;

    public static GameSystem Instans;

    public  List<TextMeshProUGUI> AllCoolScoreTexts = new List<TextMeshProUGUI>();
    public List<float> AllCoolNumbers = new List<float>();
    public void AddCoolText(int ID) 
    {
        if (AllCoolNumbers.Count < ID) return;
        AllCoolNumbers[ID]++;
        AllCoolScoreTexts[ID].text = AllCoolNumbers[ID].ToString();
    }


    [SerializeField] GameObject SabMassage;
    [SerializeField] List<GameObject> MassageList;

    private DateTime currentDateTime;
    public string StartcurrentTimeString;
    public string FinishcurrentTimeString;

    public enum GameMode 
    { 
    
        Stay,
        BeforeGame,
        Game,
        AfterGame,
    
    }
    public GameMode gameMode = GameMode.Stay;

    public enum GameStyle
    {

        DefaltGameMode,
        OldGameMode

    }
    public GameStyle gameStyle = GameStyle.OldGameMode;

    // Start is called before the first frame update
    void Start()
    {
        Instans = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)) 
        {
            if (ResaltPhase == 5) 
            {
                ResaltCount = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            BeerStanbyOK = false;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            BeerStanbyOK = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ゲーム終了
            QuitGame();
        }
        if (Input.GetKey(KeyCode.F)) 
        {
            
        }
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (gameMode == GameMode.Game && StartCount > 1)
            {
                udp_Client.SendJsonData("ビールよこせ", 0);
            }
            F_pop = true;
        }
        if (Input.GetKeyUp(KeyCode.F)) 
        {
            F_pop = false;
        }

        int f_Count = 0 ;
        foreach (var v in F_push) { if (!v) { f_Count++; } }
        if (gameMode == GameMode.Stay && BeerStanbyOK && f_Count >10)
        {
            GameStart();
            udp_Client.SendJsonData("ゲーム開始", 20);
        }
        F_push.Enqueue(F_pop);
        if (F_push.Count > 20) 
        { 
        F_push.Dequeue();
        }
        if (f_Count == 0 && F_push.Count > 15) 
        {
            BeerStanbyOK = true;
        }


        if (Input.GetKeyDown(KeyCode.R)) 
        { 
        ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            //GameStart();
            //udp_Client.SendJsonData("ゲーム開始",20);
        }
    }
    public void GameStart() 
    {
        StartZoomCoroutine(GraduallyShrinkDownSize(5f, 0.5f));


        currentDateTime = DateTime.Now;
        StartcurrentTimeString = currentDateTime.ToString("HH:mm:ss");

        beerController.AllDellBeers();
        gameMode = GameMode.BeforeGame;
        scoreManager.AllScore = 0;
        Kanban.Play("消えるカンバン", 0, 0);
        eightController.animator.Play("ゲーム開始", 0,0);
        eightController.StartGameNow = true;
    
    }
    void SetAllQueueTrue(Queue<bool> queue)
    {
        int count = queue.Count;

        for (int i = 0; i < count; i++)
        {
            queue.Dequeue();      // 古い値を捨てる
            queue.Enqueue(true);  // true を入れる
        }
    }
    public void ResetGame()//ゲームの初期化
    {
        resaltBoard.allDisperBracks();
        currentDateTime = DateTime.Now;

        gamedata.SaveData(ScoreNow,"");

        FinishcurrentTimeString = currentDateTime.ToString("HH:mm:ss");

        ResaltPhase =0;
        ResaltCount=0;
        eightController.transform.position = new Vector2(-5.5f,1.71f);
        eightController.StopAllCoroutines();
        StartZoomCoroutine(GraduallyShrinkUpSize(6f, 0.5f));


        udp_Client.SendJsonData("ビール消せ", 0);

        udp_Client.SendJsonData("終了",0);
        beerController.AllDellBeers();
        if (ResaltB.GetCurrentAnimatorStateInfo(0).IsName("現れるリザルトボード"))
        {
            ResaltB.Play("消えるリザルトボード", 0, 0);
        }
        Kanban.Play("出てくるカンバン", 0, 0);

        eightController.animator.Play("開始前待機", 0, 0);
        scoreManager.AllScore = 0;
        gameMode = GameMode.Stay;
        GameCout = 0;
        //GameTimer = 100;
        N50 = false;
        N15 = false;
        LastCount = 15;
        StartCount = 0;
        StartLastCunt = 4;

        GameCountText.text = "100";
    }

    public void ResetGame_Another()
    {

        currentDateTime = DateTime.Now;

        gamedata.SaveData(ScoreNow, "AllDATA");
        eightController.StopAllCoroutines();
        FinishcurrentTimeString = currentDateTime.ToString("HH:mm:ss");

        ResaltPhase = 0;
        ResaltCount = 0;
        eightController.transform.position = new Vector2(-5.5f, 1.71f);
        StartZoomCoroutine(GraduallyShrinkUpSize(6f, 0.5f));


        udp_Client.SendJsonData("ビール消せ", 0);

        udp_Client.SendJsonData("終了", 0);
        beerController.AllDellBeers();
        if (ResaltB.GetCurrentAnimatorStateInfo(0).IsName("現れるリザルトボード"))
        {
            ResaltB.Play("消えるリザルトボード", 0, 0);
        }
        Kanban.Play("出てくるカンバン", 0, 0);

        eightController.animator.Play("開始前待機", 0, 0);
        scoreManager.AllScore = 0;
        gameMode = GameMode.Stay;
        GameCout = 0;
        //GameTimer = 100;
        N50 = false;
        N15 = false;
        LastCount = 15;
        StartCount = 0;
        StartLastCunt = 4;

        GameCountText.text = "100";
    }
    private void FixedUpdate()
    {

        switch (gameMode) 
        { 
        
                case GameMode.Stay:
                GameMode_Stay();//スタート画面
                break;
                case GameMode.BeforeGame:
                GameMode_BeforeGame();//ゲーム開始直前
                break;
                case GameMode.Game:
                GameMode_Game();//ゲーム
                break;
                case GameMode.AfterGame:
                GameMode_AfterGame();//リザルト
                break;
        }

        
    }



    void GameMode_AfterGame() 
    {
        if (gameStyle == GameStyle.DefaltGameMode)
        {
            GameCout = 100;
            ResaltCount += Time.deltaTime;
            if (ResaltPhase == 0)
            {
                
                Amanager.PlayBGM(null);
                eightController.animator.Play("結果１", 0, 0);
                ResaltPhase++;
            }
            if (ResaltPhase == 1) 
            {
                if (ResaltCount > 1f) 
                {
                    StartZoomCoroutine(GraduallyShrinkDownSize(4f, 4f));
                    eightController.animator.Play("結果２", 0, 0);
                    ResaltPhase++;
                    ResaltCount = 0;
                }
            }
            if (ResaltPhase == 2)
            {
                if (ResaltCount > 1f)
                {
                    StartZoomCoroutine(GraduallyShrinkDownSize(3f, 4f));
                    eightController.animator.Play("結果３", 0, 0);
                    ResaltPhase++;
                    ResaltCount = 0;
                }
            }
            if (ResaltPhase == 3)
            {
                if (ResaltCount > 1.5f)
                {
                    StartZoomCoroutine(GraduallyShrinkUpSize(5f, 8f));
                    eightController.PontGet(eightController.SaveScore);
                    ResaltPhase++;
                    ResaltCount = 0;
                }
            }
            if (ResaltPhase == 4) 
            {
                

                if (ResaltCount > 1)
                {ResaltPhase++;
                    GameCout = GameTimer;

                    ResaltB.Play("現れるリザルトボード", 0, 0);
                    GameObject CL_Text = Instantiate(CountTextPrefab);
                    RectTransform rect = CL_Text.GetComponent<RectTransform>();
                    CL_Text.transform.parent = Cunvas.transform;
                    rect.anchoredPosition = Vector3.zero;

                    AnimText animText = CL_Text.GetComponent<AnimText>();

                    scoreManager.FinishGameScore();
                    Amanager.isPlaySE(audios[2]);
                    animText.TEXT.fontSize = 390;
                    animText.TEXT.text = "ビール注ぎ\n終了！";
                    gameMode = GameMode.AfterGame;
                }
            }
            if (ResaltPhase == 5) 
            {
                if (F_pop)
                {
                    if (ResaltCount > 20)
                    {
                        resaltBoard.BrackFinishCount.SetActive(true);
                        resaltBoard.BrackDaiza.SetActive(false);
                        resaltBoard.BrackFinishCountTEXT.text = (30 - ResaltCount).ToString("F0"); 
                    }
                    else 
                    {
                        resaltBoard.BrackFinishCount.SetActive(false);
                        resaltBoard.BrackDaiza.SetActive(false);
                    }
                    if (ResaltCount > 30) 
                    {
                        SetAllQueueTrue(F_push);
                        ResetGame();
                    }
                }
                else 
                {
                    if (ResaltCount > 15)
                    {
                        ResaltCount = 200;
                        resaltBoard.BrackFinishCount.SetActive(false);
                        resaltBoard.BrackDaiza.SetActive(true);
                        
                    }
                }
                
            }
        }
    }
    void GameMode_Stay() 
    {

        Amanager.PlayBGM(BGMs[1]);
    
    }
    void GameMode_BeforeGame() 
    {
        Amanager.PlayBGM(BGMs[0]);
        StartCount += Time.deltaTime;
        if (4 - StartCount < StartLastCunt) 
        {

            StartLastCunt--;
            //StartCount = 0;
            GameObject CL_Text = Instantiate(CountTextPrefab);
            RectTransform rect = CL_Text.GetComponent<RectTransform>();
            CL_Text.transform.parent = Cunvas.transform;
            rect.anchoredPosition = Vector3.zero;
            
            AnimText animText = CL_Text.GetComponent<AnimText>();

            

            if (StartLastCunt != 0)
            {Amanager.isPlaySE(audios[1]);
                animText.TEXT.text = (StartLastCunt).ToString();
            }
            else 
            {
                Amanager.isPlaySE(audios[3]);
                animText.TEXT.fontSize = 390;
                animText.TEXT.text = "ビール注ぎ\n開始！";
            }

            Destroy(CL_Text, 2) ;
        }

        if (StartLastCunt == 0) 
        { 
        gameMode = GameMode.Game;
            StartLastCunt = 4;
        }

    }

    void GameMode_Game() 
    {
        if (gameStyle == GameStyle.DefaltGameMode) 
        {

            if (beerController.CL_Beers.Count != 0) 
            {
                resaltBoard.LostBeers.text = udp_Client.OverTime.ToString("F1") + "秒";
                resaltBoard.BeerNumber.text = udp_Client.BeerNumberAdd.ToString();
                resaltBoard.BubbleNumber.text = udp_Client.BubbleNumberAdd.ToString();
                resaltBoard.BeerPercentNumber.text = beerController.SaveBeerV.ToString("F1") + "%";
                resaltBoard.BubblePercentNumber.text = beerController.SaveBubbleV.ToString("F1") + "%";
                resaltBoard.Score.text = scoreManager.AllScore.ToString("F1");
                float AllBeers = beerController.SaveBubbleV + beerController.SaveBeerV;
                resaltBoard.AllBeers.text = AllBeers.ToString("F1") + "%";
                Destroy(SabMassage);
                //GameCout = 100;
                gameMode = GameMode.AfterGame;
            }
        
        }
        if (GameCout < GameTimer)
        {
            GameCout += Time.deltaTime;
            if (!N50 && (GameTimer - GameCout) < 50)
            {
                N50 = true;
                GameObject CL_N50 = Instantiate(Nokori50);
                CL_N50.transform.parent = Cunvas.transform;
                CountTextanimator.Play("急ぎ", 0, 0);
                RectTransform rect = CL_N50.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector3.zero;
                Amanager.isPlaySE(audios[0]);


                Destroy(CL_N50, 3);
            }
            if ((GameTimer - GameCout) < 15)
            {

                if ((GameTimer - GameCout) < LastCount)
                {
                    LastCount--;
                    Amanager.isPlaySE(audios[1]);
                    CountTextanimator.Play("急ぎ", 0, 0);
                }


                if (!N15)
                {
                    N15 = true;
                    GameObject CL_N15 = Instantiate(Nokori15);
                    CL_N15.transform.parent = Cunvas.transform;

                    RectTransform rect = CL_N15.GetComponent<RectTransform>();
                    rect.anchoredPosition = Vector3.zero;
                    Amanager.isPlaySE(audios[0]);
                    CountTextanimator.Play("急ぎ", 0, 0);
                    Destroy(CL_N15, 3);
                }
            }

        }
        else
        {
            GameCout = GameTimer;

            //ResaltB.Play("現れるリザルトボード", 0, 0);
            GameObject CL_Text = Instantiate(CountTextPrefab);
            RectTransform rect = CL_Text.GetComponent<RectTransform>();
            CL_Text.transform.parent = Cunvas.transform;
            rect.anchoredPosition = Vector3.zero;

            AnimText animText = CL_Text.GetComponent<AnimText>();

            scoreManager.FinishGameScore();
            Amanager.isPlaySE(audios[2]);
            animText.TEXT.text = "業務終了";
            gameMode = GameMode.AfterGame;
        }
        GameCountText.text = (GameTimer - GameCout).ToString("F2");
    }

    void QuitGame()
    {
        Debug.Log("ゲームを終了します");

        // エディタ上での実行停止（Unityエディタ用）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド後の実行ファイルを終了
        Application.Quit();
#endif
    }

public void StartZoomCoroutine(IEnumerator coroutine)
    {
        // 前のコルーチンが動いていたら停止
        if (currentZoomCoroutine != null)
        {
            StopCoroutine(currentZoomCoroutine);
        }

        // 新しいコルーチンを開始
        currentZoomCoroutine = StartCoroutine(coroutine);
    }
    private IEnumerator GraduallyShrinkDownSize(float value,float Speed)
    {
        while (maincamera.orthographicSize > value) // サイズが 1 になるまで減少
        {
            maincamera.orthographicSize -= Speed * Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator GraduallyShrinkUpSize(float value, float Speed)
    {
        while (maincamera.orthographicSize < value) // サイズが 1 になるまで減少
        {
            maincamera.orthographicSize += Speed * Time.deltaTime;
            yield return null;
        }
    }
}

[System.Serializable]
public class BeerEvaluation 
{

    public string Evaluation;

    public bool Set = false;

}
