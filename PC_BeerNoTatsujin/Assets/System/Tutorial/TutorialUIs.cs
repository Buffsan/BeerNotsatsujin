using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUIs : MonoBehaviour
{
    [SerializeField] GameSystem gameSystem;
    [SerializeField] UDPClient udp;
    [SerializeField] Beer beer;
    public Animator animator;
    public TextMeshProUGUI StatMeshText;

    [SerializeField] List<string> list = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameSystem.gameMode == GameSystem.GameMode.Stay)
        {
            StatMeshText.text = list[0];
            animator.Play("èoåª");
            beer.gameObject.SetActive(false);
        }
        else 
        {
            StatMeshText.text = list[1];
            animator.Play("è¡é∏");
            beer.gameObject.SetActive(true);
        }
    }

    public void ChangeUI()
    {
        float BeerSize = udp.BeerV / 10;
        float BubbleSize = udp.BubbleV / 10;

        float OverSize = 0;
        float BubbleOverSize = 0;
        float OverOverSize = 0;
        beer.ChangePoint();
        beer.BeerGrass.transform.rotation =  Quaternion.Euler(0, 0,udp.RotateV);
        beer.BeerObject.transform.localScale = new Vector2(beer.BubbleObject.transform.localScale.x, BeerSize - OverOverSize);

        if (OverOverSize == 0)
        {
            beer.BubbleObject.transform.localScale = new Vector2(beer.BubbleObject.transform.localScale.x, BubbleSize - BubbleOverSize);
        }
        else
        {
            beer.BubbleObject.transform.localScale = new Vector2(beer.BubbleObject.transform.localScale.x, 0);
        }
    }
}
