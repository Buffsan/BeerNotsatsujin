using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResaltBoard : MonoBehaviour
{

    public TextMeshProUGUI BeerNumber;
    public TextMeshProUGUI BubbleNumber;
    public TextMeshProUGUI BeerPercentNumber;
    public TextMeshProUGUI BubblePercentNumber;
    public TextMeshProUGUI AllBeers;
    public TextMeshProUGUI LostBeers;
    public TextMeshProUGUI Score;

    public Sprite BrackStar;
    public Sprite MaxStar;

    public List<Image> StarImages = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
