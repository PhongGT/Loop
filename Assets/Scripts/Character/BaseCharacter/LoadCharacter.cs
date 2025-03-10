using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacter : MonoBehaviour
{
    
    protected SpriteRenderer spriteRenderer;
    protected GameObject healthBar;
    protected GameObject statusBar;
    public Character currentCharacter;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Load()
    {
        spriteRenderer.sprite = currentCharacter.icon;
    }
}
