using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSystem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] BoxCollider2D _boxCollider;
    [SerializeField] GameSystem _gameSystem;
    AudioManager _audioManager => AudioManager.instance;

    public enum ButtonType
    {

        Start,
        Exit

    }
    public ButtonType buttonType = ButtonType.Start;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider == _boxCollider)
            {
                _gameSystem.ResetGame();
            }
        }
    }

    
}
