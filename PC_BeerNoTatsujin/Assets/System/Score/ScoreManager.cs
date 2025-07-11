using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameSystem gameSystem;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI StartFlagText;
    [SerializeField] GameObject CoinText;
    [SerializeField] GameObject CoinTextPoint;
    [SerializeField] GameObject Canvas;

    [SerializeField] string StandbyOK;
    [SerializeField] string StandbyOFF;

    public float AllScore =0;

    public List<float> Scores = new List<float>();
    public List<ScoreRanking> scoreRankings = new List<ScoreRanking>();

   
    
    
    public float NowScore = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = AllScore.ToString();
        if (gameSystem.BeerStanbyOK)
        {
            StartFlagText.text = StandbyOK.ToString();
        }
        else 
        {
            StartFlagText.text = StandbyOFF.ToString();
        }

        //Debug.Log(AdjustScore(NowScore ,0,10,7));

    }

    public float AdjustScore(float inputValue ,float minScore,float maxScore,float baseValue)
    {
        if (inputValue <= minScore || inputValue >= maxScore)
        {
            return 0f; // 範囲外ならスコアは0
        }

        // 距離を計算
        float distance = Mathf.Abs(inputValue - baseValue);

        // 二次的にスコアを減らす (例えば y = 1 - (distance^2 / maxDistance^2))
        float maxDistance = Mathf.Abs(maxScore - baseValue);
        float normalizedScore = Mathf.Clamp01(1 - Mathf.Pow(distance / maxDistance, 2));

        // スコアを線形補間
        return Mathf.Lerp(minScore, maxScore, normalizedScore);

    }


    public void FinishGameScore() 
    {
        Scores.Add(AllScore);
        Scores = Scores.OrderByDescending(x => x).ToList();
        int i = 0;

        foreach (ScoreRanking _scores in scoreRankings) 
        {
            _scores.RankingScore = Scores[i];
            _scores.RankingText.text = Scores[i].ToString();
            i++;
        }
    
    }
    public void isAddScore(float Score) 
    {
        AllScore += Score;
        GameObject CL_CoinText = Instantiate(CoinText, CoinTextPoint.transform.position, Quaternion.identity);
        CL_CoinText.transform.parent = Canvas.transform;
        AnimText animText = CL_CoinText.GetComponent<AnimText>();

        animText.TEXT.text = Score.ToString();
        Destroy(CL_CoinText, 3f);
    }
}


[System.Serializable]
public class ScoreRanking 
{

    public TextMeshProUGUI RankingText;
    public float RankingScore = 000000;

}
