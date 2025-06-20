using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] GameObject CoinText;
    [SerializeField] GameObject CoinTextPoint;
    [SerializeField] GameObject Canvas;

    

    public float AllScore =0;

    public List<float> Scores = new List<float>();
    public List<ScoreRanking> scoreRankings = new List<ScoreRanking>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = AllScore.ToString();  
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
