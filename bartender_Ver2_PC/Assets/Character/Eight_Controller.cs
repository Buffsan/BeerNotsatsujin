using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Eight_Controller : MonoBehaviour
{

    [SerializeField]Sprite Nomal;
    [SerializeField] Sprite Fine1;
    [SerializeField] Sprite Fine2;
    [SerializeField] Sprite Bad1;
    [SerializeField] Sprite Bad2;

    [SerializeField] TextMeshProUGUI ExcellentText;
    [SerializeField] TextMeshProUGUI GreatText;
    [SerializeField] TextMeshProUGUI NiceText;
    [SerializeField] TextMeshProUGUI SosoText;

    [SerializeField] Animator AllScoreAnimator;
    [SerializeField] Animator CoolAnimator;
    [SerializeField] GameObject ExcellentEffectObj;
    [SerializeField] GameObject GreatEffectObj;
    [SerializeField] GameObject NiceEffectObj;

    public int ExcellentCount = 0;
    public int GreatCount = 0;
    public int NiceCount = 0;
    public int SosoCount = 0;
    public int BadCount = 0;
    public int TerribleCount = 0;

    public bool StartGameNow = false;
    float StartGameCount = 0;

    public float SaveScore = 0;

    [SerializeField] ScoreManager scoreManager;
    [SerializeField] ResaltBoard resaltBoard;

    public List<Sprite> Cool = new List<Sprite>();
    public List<AudioClip> Coolaudio = new List<AudioClip>();
  public List<BounusBeerPoint> Bounus_beerpoint = new List<BounusBeerPoint>();

    [SerializeField] GameObject CoolObject;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    AudioManager audiomanager => AudioManager.instance;

    float WaitCount = 0;

    public enum Mode 
    { 
    
        Nomal,
         Move
    
    }

    Mode mode = Mode.Nomal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) 
        {
            PontGet(100);
            //scoreManager.AllScore += 100;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            PontGet(80);
            //scoreManager.AllScore += 80;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PontGet(60);
            //scoreManager.AllScore += 60;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PontGet(40);
            //scoreManager.AllScore += 25;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PontGet(15);
            //scoreManager.AllScore += 25;
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            PontGet(0);
            //scoreManager.AllScore += 25;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        switch (mode) 
        {
            
            case Mode.Nomal:
                isNomal();
                break;
            case Mode.Move:
                isMove();
                break;
        
        }
    }
    void isNomal() 
    {
        animator.SetInteger("Anim",0);
        spriteRenderer.sprite = Nomal;

        if (StartGameNow) 
        {
            StartGameCount += Time.deltaTime;
            if (StartGameCount > 3) 
            {
                StartGameCount = 0;
                StartGameNow = false;
                StartCoroutine(EightRightMove(new Vector2( 1,0), 3));
            }
        }

    }

    public void isMove()
    {
        WaitCount += Time.deltaTime;
        if (WaitCount > 4) 
        {
            WaitCount = 0;
            mode = Mode.Nomal;
        }
    }
    void CoolEffectSpawn(GameObject coolObj ) 
    {
        GameObject CL_ExcellentEffectObj = Instantiate(coolObj, transform.position, Quaternion.identity);
        Destroy(CL_ExcellentEffectObj, 3);
    }
    public void PontGet(float Getpoint) 
    {

        //scoreManager.isAddScore(Getpoint);

        WaitCount = 0;
        mode = Mode.Move;

        Debug.Log(Getpoint);

        for (int i = 0; i < resaltBoard.StarImages.Count; i++) 
        {

            if (Getpoint > 20 * (i+1)) 
            {
                resaltBoard.StarImages[i].sprite = resaltBoard.MaxStar;
                Debug.Log("HOSI"+i);
            }
        
        }

        //animator.Play("表情変化",0,0);
        animator.SetInteger("Anim",1);
        if (Getpoint != 0) 
        {
            AllScoreAnimator.Play("全点数加算",0,0);
        }

        GameObject CL_Cool = Instantiate(CoolObject, new Vector2(Random.Range(-5f,0f), Random.Range(-2f,2f)), Quaternion.identity);
        CoolController cool = CL_Cool.GetComponent<CoolController>();

        if (Getpoint >= 100)
        {
            cool.sprite.sprite = Cool[0]; cool.animator.Play("エクセレント評価"); audiomanager.isPlaySE(Coolaudio[0]);
            animator.Play("表情変化エクセレント", 0, 0);
            CoolAnimator.Play("エクセレント",0,0);
            CoolEffectSpawn(ExcellentEffectObj);
            CL_Cool.transform.position = new Vector2(-2.5f,3);

            ExcellentCount++;
        } else if(Getpoint < 100 && Getpoint >=85)
            {
             cool.sprite.sprite = Cool[1]; cool.animator.Play("グレート評価"); audiomanager.isPlaySE(Coolaudio[1]);
            animator.Play("表情変化グレート", 0, 0);
            CoolAnimator.Play("グレート", 0, 0);
            GreatCount++;
            CoolEffectSpawn(GreatEffectObj);
        }
        else if (Getpoint < 85 && Getpoint >= 70)
        {
              cool.sprite.sprite = Cool[2];cool.animator.Play("ナイス評価"); audiomanager.isPlaySE(Coolaudio[2]);
            animator.Play("表情変化ナイス", 0, 0);
            NiceCount++;
            CoolAnimator.Play("ナイス", 0, 0);
            CoolEffectSpawn(NiceEffectObj);
        }
        else if (Getpoint < 70 && Getpoint >= 60)
        {

             cool.animator.Play("ソーソー評価"); audiomanager.isPlaySE(Coolaudio[4]);
            animator.Play("表情変化ソーソー", 0, 0);
        }
        else if (Getpoint < 60 && Getpoint > 50)
        {
            cool.sprite.sprite = Cool[3];cool.animator.Play("バット評価"); audiomanager.isPlaySE(Coolaudio[3]);
            animator.Play("表情変化バット", 0, 0);
            CoolAnimator.Play("バット", 0, 0);

        }
        else if (Getpoint <= 50)
        {
            cool.sprite.sprite = Cool[3]; cool.animator.Play("テリブル評価"); audiomanager.isPlaySE(Coolaudio[5]);
            animator.Play("表情変化テリブル", 0, 0);
            CoolAnimator.Play("テリブル", 0, 0);

        }

    }
    private IEnumerator EightRightMove(Vector2 value, float Speed)
    {
        animator.Play("移動", 0, 0);
        while (transform.position.x < value.x) // サイズが 1 になるまで減少
        {
            transform.localScale = new Vector2(-6, 6);
            transform.position =  new Vector2( transform.position.x + Speed * Time.deltaTime ,transform.position.y);
            yield return null;
        }
        transform.localScale = new Vector2(6, 6);
        animator.Play("待機", 0, 0);
    }

}

[System.Serializable]
public class BounusBeerPoint 
{

    public string BounusString = "入力ナシ";
    public int PutInBubbleV = 0;
    public int PutIutBeerV = 0;
    public float BounusPoint = 1;

}
