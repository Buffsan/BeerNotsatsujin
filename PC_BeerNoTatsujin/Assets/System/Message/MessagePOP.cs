using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MessagePOP : MonoBehaviour, IPointerClickHandler
{
    Animator animator;
    [SerializeField] AnimationClip Clip;

    [SerializeField] float DieTime = 0;
    float Count = 0;
    bool No = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Count += Time.deltaTime;
        if (Count > DieTime && !No) 
        { 
        Count = 0;
            No = true;
            animator.Play(Clip.name, 0, 0);
        }

        if (Count > 3 && No) 
        { 
        Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(this.gameObject.name + " の BoxCollider2Dがクリックされました！");

        // クリックされたオブジェクトが何だったか確認したい場合
        if (eventData.pointerEnter != null)
        {
            Debug.Log("クリックされたオブジェクトは: " + eventData.pointerEnter.name);
        }
    }
}
