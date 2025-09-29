using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPersist : MonoBehaviour
{
    private static PlayerPersist Instance;

    void Awake()
    {
        // กันซ้ำ: ถ้ามีตัวเก่าอยู่แล้ว ให้ทำลายตัวใหม่
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // คำสั่งสำคัญ!
    }
}
