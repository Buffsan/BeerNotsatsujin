using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class BeerController : MonoBehaviour
{
    // Start is called before the first frame update

    public UDPClient udp;

    public float SaveBeerV = 0;
    public float SaveBubbleV = 0;

    [SerializeField] SAVE_Gamedata savedata;
    [SerializeField] GameObject BeerGlass;
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject SpawnPoint2;
    [SerializeField] Eight_Controller eightChan;
    [SerializeField] GameSystem gamesystem;
    [SerializeField] ResaltBoard resaltBoard;

    public List<string> RevieTexts = new List<string>();

    [SerializeField] ScoreManager scoreManager;

    public List<GameObject> CL_Beers = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            isBeerSpawn();
        }*/
        if (udp.A && udp.BeerV != 999 && udp.BubbleV != 999) 
        {
            isBeerSpawn();
        }
    }
    public void AllDellBeers() 
    {
        
        foreach( var beers in CL_Beers) 
        { 
            Animator beeranimator = beers.GetComponent<Animator>();
            beeranimator.Play("消える",0,0);
            Destroy(beers,5);
        }
        CL_Beers.Clear();

    }

    void isBeerSpawn() 
    {
        udp.A = false;
    
        GameObject CL_Beer = Instantiate(BeerGlass, SpawnPoint.transform.position,Quaternion.identity);
        Rigidbody2D rb = CL_Beer.GetComponent<Rigidbody2D>();

        if (gamesystem.gameStyle == GameSystem.GameStyle.DefaltGameMode &&gamesystem.gameMode ==GameSystem.GameMode.Game)
        {
            CL_Beer.transform.position = SpawnPoint2.transform.position;
        }
        else
        {
            //int RandomBeerXscale = Random.Range(-1, 1);
            float RandomX = Random.Range(10f, 20f);
            float RandomY = Random.Range(3, 12);

            //CL_Beer.transform.localScale = new Vector2(CL_Beer.transform.localScale.x * RandomBeerXscale, CL_Beer.transform.localScale.y);


            Vector2 VeloRB = new Vector2(RandomX, RandomY);
            rb.velocity = VeloRB;
        }
        CL_Beers.Add(CL_Beer);


        Beer beer = CL_Beer.GetComponent<Beer>();

        float BeerSize = udp.BeerV / 10;
        float BubbleSize = udp.BubbleV / 10;

        float OverSize=0;
        float BubbleOverSize = 0;
        float OverOverSize = 0;
        if (BeerSize + BubbleSize > 1)
        {
            Debug.Log(BeerSize + BubbleSize);
            OverSize = (BeerSize + BubbleSize) - 1;
            BubbleOverSize = OverSize;
            OverSize = BubbleSize - OverSize;
            if (OverSize < 0) 
            {
                OverOverSize = OverSize * -1;
            }
        }

        beer.BeerObject.transform.localScale = new Vector2(1, BeerSize -OverOverSize);

        if (OverOverSize == 0)
        {
            beer.BubbleObject.transform.localScale = new Vector2(1, BubbleSize - BubbleOverSize);
        }
        else 
        {
            beer.BubbleObject.transform.localScale = new Vector2(1, 0);
        }

            float TotalBeers = (udp.BeerV + udp.BubbleV) /10;
        if (TotalBeers >= 1) 
        { 
        TotalBeers = 1;
        }

        CalculateScore(udp.BeerV/10, udp.BubbleV/10, TotalBeers);

        SaveBeerV =udp.BeerV *10;
        SaveBubbleV =udp.BubbleV*10;

        udp.BubbleV = 999;
        udp.BeerV = 999;

    }

    public int CalculateScore(float liquidRatio, float foamRatio, float totalRatio)
    {int score = 0;


        //Debug.Log("スコア計算開始");

        if (gamesystem.gameStyle == GameSystem.GameStyle.DefaltGameMode)
        {
            /*
            const float BubbleValuePoint = 0.3f;
            const float BeerValuePoints = 0.7f;
            const float AllBeersValue = 80;

            float foamRatio_val;
            float liquidRatio_val;

            float SafeValue = 0;

            float minLiquidRatio = BeerValuePoints - SafeValue; // 最小値
            float maxLiquidRatio = BeerValuePoints + SafeValue; // 最大値

            float minFoamRatio = BubbleValuePoint - SafeValue;
            float maxFoamRatio = BubbleValuePoint + SafeValue;

            // liquidRatioとfoamRatioの値を制限
            liquidRatio_val = Mathf.Clamp(liquidRatio, minLiquidRatio, maxLiquidRatio);
            foamRatio_val = Mathf.Clamp(foamRatio, minFoamRatio, maxFoamRatio);


           

            Bubble = Mathf.Max(0, (BubbleValuePoint - Mathf.Abs(foamRatio_val - BubbleValuePoint)) * AllBeersValue);
            Beer = Mathf.Max(0, (BeerValuePoints - Mathf.Abs(liquidRatio_val - BeerValuePoints)) * AllBeersValue);
            */ 
            
            float Beer;
            float Bubble;

            Beer = scoreManager.AdjustScore((float)liquidRatio, 0,1,0.7f)*50;
            Bubble = scoreManager.AdjustScore((float)foamRatio, 0, 1, 0.3f)*50;
            float Beersscore = (int)Bubble + (int)Beer;
            Debug.Log("Beern:" + liquidRatio + " Bubblen" + foamRatio);
            Debug.Log("Beer:"+Beer+ " Bubble"+ Bubble);

            const float AllRuleValue = 2;

            float lostTimeScore = AllRuleValue;
            if (udp.OverTime > 2)
            {
                 lostTimeScore = Mathf.Max(0, AllRuleValue - (udp.OverTime / 1.5f));
            }
            else 
            {
                //lostTimeScore = AllRuleValue;
            }

            float Bounus=0;

            string finish_bonusText = resaltBoard.BonusTexts[2];
            if (totalRatio > 0.9f)
            {
                foreach (var BeerType in eightChan.Bounus_beerpoint)
                {
                    if (udp.BeerNumberAdd == BeerType.PutIutBeerV && udp.BubbleNumberAdd == BeerType.PutInBubbleV)
                    {
                        if (BeerType.BounusString == "１度注ぎ") 
                        {finish_bonusText = resaltBoard.BonusTexts[0]; }
                        if (BeerType.BounusString == "通常注ぎ")
                        { finish_bonusText = resaltBoard.BonusTexts[1]; }
                        Debug.Log(BeerType.BounusString);
                        Bounus = BeerType.BounusPoint;
                    }
                }
            }
            
            score = (int)Beersscore + (int)lostTimeScore + (int)Bounus;
            savedata.SaveData(score);

            //総評処理
            string finish_ReviwText = RevieTexts[7];
            if (liquidRatio < 0.65) 
            {
                finish_ReviwText = RevieTexts[0];
            }
            if (foamRatio < 0.25) 
            {
                finish_ReviwText = RevieTexts[1];
            }
            if ((foamRatio + liquidRatio) < 0.9)
            {
                finish_ReviwText = RevieTexts[2];
            }
            if ((foamRatio + liquidRatio) < 0.1) 
            {
                finish_ReviwText = RevieTexts[3];
            }
            if (foamRatio > 0.9) 
            {
                finish_ReviwText = RevieTexts[4];
            }
            if (udp.OverTime > 2) 
            {
                finish_ReviwText = RevieTexts[6];
            }
            if (score >= 103) 
            {
                finish_ReviwText = RevieTexts[5];
            }
            resaltBoard.BonusText.text = finish_bonusText;
            resaltBoard.ReviewTEXT.text = finish_ReviwText;
            //Debug.Log(finish_ReviwText);
        }
        else
        {
            // 点数の初期化


            // 全体の量が9割未満の場合はスコアなし
            if (totalRatio < 0.8f)
            {
                if (gamesystem.gameStyle == GameSystem.GameStyle.DefaltGameMode) 
                {
                    eightChan.SaveScore = score;
                } 
                else 
                { 
                eightChan.PontGet(score);
                }
                
                return score;
            }

            // ビール液7割、泡3割に近いほど点数アップ
            float idealLiquidRatio = 0.7f;
            float idealFoamRatio = 0.3f;

            // 許容範囲（±5% = 0.05）
            float tolerance = 0.05f;

            // 理想範囲内なら最大スコア
            bool isWithinTolerance = Mathf.Abs(liquidRatio - idealLiquidRatio) <= tolerance &&
                                     Mathf.Abs(foamRatio - idealFoamRatio) <= tolerance;

            if (isWithinTolerance)
            {
                // 許容範囲内なら最大スコア
                score += 5000;
            }

            // 理想の比率との差を計算（許容範囲外の場合）
            float liquidDifference = Mathf.Abs(liquidRatio - idealLiquidRatio);
            float foamDifference = Mathf.Abs(foamRatio - idealFoamRatio);

            // 比率のスコア計算
            float ratioScore = 100 * (1 - (liquidDifference + foamDifference) / 2);

            // 全体の量に応じたスコア倍率
            //float totalScoreMultiplier = Mathf.Clamp(totalRatio, 0.9f, 1.0f);

            float totalScoreMultiplier = (totalRatio >= 0.9f) ? Mathf.Clamp((totalRatio - 0.9f) / 0.1f, 0f, 1f) : 0f;

            // 総合スコアを計算
            score = Mathf.RoundToInt(ratioScore * totalScoreMultiplier);
        }

            Debug.Log("-----" + score + "-----");

        if (gamesystem.gameStyle == GameSystem.GameStyle.DefaltGameMode)
        {
            eightChan.SaveScore = score;
        }
        else
        {
            eightChan.PontGet(score);
        }

        scoreManager.AllScore += score;


        
        return score;

    }


}
