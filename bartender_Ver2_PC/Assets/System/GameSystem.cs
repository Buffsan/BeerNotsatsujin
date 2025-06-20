using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameSystem : MonoBehaviour
{

    public float GameCout=0;
    public float GameTimer = 100;

    
    public List<AudioClip> BGMs = new List<AudioClip>();
    public List<AudioClip> audios = new List<AudioClip>();
    AudioManager Amanager => AudioManager.instance;

    [SerializeField] Camera maincamera;
    [SerializeField] GameObject Cunvas;
    [SerializeField] GameObject Nokori50;
    bool N50 = false;
    [SerializeField] GameObject Nokori15;
    [SerializeField] Animator CountTextanimator;
    [SerializeField] ScoreManager scoreManager;
    bool N15 = false;

    int LastCount = 15;
    float StartCount = 0;
    public int StartLastCunt = 4;
    public int ResaltPhase = 0;
    float ResaltCount = 0;

    [SerializeField] Animator Kanban;
    [SerializeField] Animator ResaltB;
    [SerializeField] TextMeshProUGUI GameCountText;
    [SerializeField] GameObject CountTextPrefab;
    [SerializeField] Eight_Controller eightController;
    [SerializeField] BeerController beerController;
    [SerializeField] UDPClient udp_Client;
    [SerializeField] ResaltBoard resaltBoard;

    
     
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (gameMode == GameMode.Game)
            {
                udp_Client.SendJsonData("ビールよこせ", 0);
            }
            
        }
        if (Input.GetKeyUp(KeyCode.F)) 
        {
            if (gameMode == GameMode.Stay) 
            {
                GameStart();
                udp_Client.SendJsonData("ゲーム開始", 20);
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        { 
        ResetGame();
        }
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            GameStart();
            udp_Client.SendJsonData("ゲーム開始",20);
        }
    }
    public void GameStart() 
    {
        StartCoroutine(GraduallyShrinkDownSize(5, 0.5f));

        beerController.AllDellBeers();
        gameMode = GameMode.BeforeGame;
        scoreManager.AllScore = 0;
        Kanban.Play("消えるカンバン", 0, 0);
        eightController.animator.Play("ゲーム開始", 0,0);
        eightController.StartGameNow = true;
    
    }
    public void ResetGame()
    {


        ResaltPhase=0;
        ResaltCount=0;
        eightController.transform.position = new Vector2(-5.5f,1.71f);
        StartCoroutine(GraduallyShrinkUpSize(6, 0.5f));

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
        GameTimer = 100;
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
                if (ResaltCount > 2) 
                {
                    StartCoroutine(GraduallyShrinkDownSize(4, 4));
                    eightController.animator.Play("結果２", 0, 0);
                    ResaltPhase++;
                    ResaltCount = 0;
                }
            }
            if (ResaltPhase == 2)
            {
                if (ResaltCount > 2)
                {
                    StartCoroutine(GraduallyShrinkDownSize(3, 4));
                    eightController.animator.Play("結果３", 0, 0);
                    ResaltPhase++;
                    ResaltCount = 0;
                }
            }
            if (ResaltPhase == 3)
            {
                if (ResaltCount > 2)
                {
                    StartCoroutine(GraduallyShrinkUpSize(5, 8));
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
                    animText.TEXT.text = "業務終了";
                    gameMode = GameMode.AfterGame;
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
                animText.TEXT.text = "業務開始";
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
                resaltBoard.LostBeers.text = udp_Client.OverTime.ToString("F1");
                resaltBoard.BeerNumber.text = udp_Client.BeerNumberAdd.ToString();
                resaltBoard.BubbleNumber.text = udp_Client.BubbleNumberAdd.ToString();
                resaltBoard.BeerPercentNumber.text = beerController.SaveBeerV.ToString("F1") + "%";
                resaltBoard.BubblePercentNumber.text = beerController.SaveBubbleV.ToString("F1") + "%";
                resaltBoard.Score.text = scoreManager.AllScore.ToString("F1");
                float AllBeers = beerController.SaveBubbleV + beerController.SaveBeerV;
                resaltBoard.AllBeers.text = AllBeers.ToString("F1") + "%";
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

            ResaltB.Play("現れるリザルトボード", 0, 0);
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
