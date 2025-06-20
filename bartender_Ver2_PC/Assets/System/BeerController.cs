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

    [SerializeField] GameObject BeerGlass;
    [SerializeField] GameObject SpawnPoint;
    [SerializeField] GameObject SpawnPoint2;
    [SerializeField] Eight_Controller eightChan;
    [SerializeField] GameSystem gamesystem;

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
            beeranimator.Play("������",0,0);
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


        //Debug.Log("�X�R�A�v�Z�J�n");

        if (gamesystem.gameStyle == GameSystem.GameStyle.DefaltGameMode)
        {

            const float BubbleValuePoint = 0.3f;
            const float BeerValuePoints = 0.7f;
            const float AllBeersValue = 65;

            float foamRatio_val;
            float liquidRatio_val;

            float SafeValue = 0;

            float minLiquidRatio = BeerValuePoints - SafeValue; // �ŏ��l
            float maxLiquidRatio = BeerValuePoints + SafeValue; // �ő�l

            float minFoamRatio = BubbleValuePoint - SafeValue;
            float maxFoamRatio = BubbleValuePoint + SafeValue;

            // liquidRatio��foamRatio�̒l�𐧌�
            liquidRatio_val = Mathf.Clamp(liquidRatio, minLiquidRatio, maxLiquidRatio);
            foamRatio_val = Mathf.Clamp(foamRatio, minFoamRatio, maxFoamRatio);


            float Beer;
            float Bubble;

            Bubble = Mathf.Max(0, (BubbleValuePoint - Mathf.Abs(foamRatio_val - BubbleValuePoint)) * AllBeersValue);
            Beer = Mathf.Max(0, (BeerValuePoints - Mathf.Abs(liquidRatio_val - BeerValuePoints)) * AllBeersValue);

            float Beersscore = (int)Bubble + (int)Beer;

            const float AllRuleValue = 25;
            float lostTimeScore = Mathf.Max(0, AllRuleValue - (udp.OverTime /1.5f));

            float Bounus=0;

            if (totalRatio > 0.9f)
            {
                foreach (var BeerType in eightChan.Bounus_beerpoint)
                {
                    if (udp.BeerNumberAdd == BeerType.PutIutBeerV && udp.BubbleNumberAdd == BeerType.PutInBubbleV)
                    {
                        Debug.Log(BeerType.BounusString);
                        Bounus = BeerType.BounusPoint;
                    }
                }
            }

            score = (int)Beersscore + (int)lostTimeScore + (int)Bounus;

        }
        else
        {
            // �_���̏�����


            // �S�̗̂ʂ�9�������̏ꍇ�̓X�R�A�Ȃ�
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

            // �r�[���t7���A�A3���ɋ߂��قǓ_���A�b�v
            float idealLiquidRatio = 0.7f;
            float idealFoamRatio = 0.3f;

            // ���e�͈́i�}5% = 0.05�j
            float tolerance = 0.05f;

            // ���z�͈͓��Ȃ�ő�X�R�A
            bool isWithinTolerance = Mathf.Abs(liquidRatio - idealLiquidRatio) <= tolerance &&
                                     Mathf.Abs(foamRatio - idealFoamRatio) <= tolerance;

            if (isWithinTolerance)
            {
                // ���e�͈͓��Ȃ�ő�X�R�A
                score += 5000;
            }

            // ���z�̔䗦�Ƃ̍����v�Z�i���e�͈͊O�̏ꍇ�j
            float liquidDifference = Mathf.Abs(liquidRatio - idealLiquidRatio);
            float foamDifference = Mathf.Abs(foamRatio - idealFoamRatio);

            // �䗦�̃X�R�A�v�Z
            float ratioScore = 100 * (1 - (liquidDifference + foamDifference) / 2);

            // �S�̗̂ʂɉ������X�R�A�{��
            //float totalScoreMultiplier = Mathf.Clamp(totalRatio, 0.9f, 1.0f);

            float totalScoreMultiplier = (totalRatio >= 0.9f) ? Mathf.Clamp((totalRatio - 0.9f) / 0.1f, 0f, 1f) : 0f;

            // �����X�R�A���v�Z
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

        scoreManager.AllScore += 30000 * score;


        
        return score;

    }


}
