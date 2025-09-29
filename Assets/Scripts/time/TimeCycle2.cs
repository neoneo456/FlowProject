using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCycle2 : MonoBehaviour
{

    public SpriteRenderer targetSprite;
    public float duration = 10f;
    public int maxAlpha = 220;

    private float timer = 0f;

    void Start()
    {
        if (targetSprite != null)
        {
            SetAlpha(0f);
        }
    }

    void Update()
    {
        if (targetSprite == null) return;

        timer += Time.deltaTime;

        float t = (timer % duration) / duration;

        float alpha01 = Mathf.PingPong(t * 2f, 1f);

        float alphaValue = Mathf.Lerp(0f, maxAlpha / 255f, alpha01);

        SetAlpha(alphaValue);
    }

    private void SetAlpha(float alphaValue)
    {
        if (targetSprite == null) return;

        Color c = targetSprite.color;
        c.a = alphaValue;
        targetSprite.color = c;
    }
 
}
