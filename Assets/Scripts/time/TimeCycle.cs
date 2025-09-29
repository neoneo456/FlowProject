using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCycle : MonoBehaviour
{
   
    public Image targetImage;
    public float duration = 10f; //เวลาในเกม1วัน
    public int maxAlpha = 220;
    private float timer = 0f;

    void Start()
    {
        if (targetImage != null)
        {
            SetAlpha(0f);
        }
    }

    void Update()
    {
        if (targetImage == null) return;

        timer += Time.deltaTime;

        float t = (timer % duration) / duration;

        float alpha01 = Mathf.PingPong(t * 2f, 1f);

        float alphaValue = Mathf.Lerp(0f, maxAlpha / 255f, alpha01);

        SetAlpha(alphaValue);
    }

    private void SetAlpha(float alphaValue)
    {
        if (targetImage == null) return;

        Color c = targetImage.color;
        c.a = alphaValue;
        targetImage.color = c;
    }
}