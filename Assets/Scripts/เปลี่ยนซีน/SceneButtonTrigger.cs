using System.Collections;
using UnityEngine;

public class SceneButtonTrigger : MonoBehaviour
{
    public string sceneToLoad;                      // ชื่อซีนที่จะโหลด
    public Vector2 playerSpawnPosition;             // จุดให้ตัวละครไปยืนในซีนใหม่

    public GameObject buttonVisual;                  // GameObject ของปุ่ม (เช่น UI หรือ Sprite)

    public string submitButtonName = "Submit";      // ชื่อปุ่มใน Input Manager ที่ใช้กด (แนะนำตั้งชื่อ Submit)
    public float pressCooldown = 0.25f;              // เวลากันกดซ้ำ

    bool playerInside = false;
    bool isActivating = false;
    float lastPressTime = -10f;

    void Start()
    {
        if (buttonVisual != null)
            buttonVisual.SetActive(false);          // เริ่มต้นซ่อนปุ่ม
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (buttonVisual != null) buttonVisual.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (buttonVisual != null) buttonVisual.SetActive(false);
        }
    }

    void Update()
    {
        if (!playerInside || isActivating) return;

        if (IsSubmitPressed())
        {
            if (Time.time - lastPressTime < pressCooldown) return;
            lastPressTime = Time.time;
            StartCoroutine(ActivateAndLoad());
        }
    }

    IEnumerator ActivateAndLoad()
{
    isActivating = true;

    if (buttonVisual != null) buttonVisual.SetActive(false);

    if (SceneLoader.Instance != null)
    {
        SceneLoader.Instance.StartSceneTransition(sceneToLoad, playerSpawnPosition);
    }
    else
    {
        Debug.LogWarning("SceneLoader.Instance is null! ไม่พบ SceneLoader");
    }

    isActivating = false;

    yield break;
}


    bool IsSubmitPressed()
    {
        // เช็คปุ่มที่ตั้งไว้ใน Input Manager
        if (!string.IsNullOrEmpty(submitButtonName) && Input.GetButtonDown(submitButtonName))
            return true;

        // fallback เช็คจอยสติ้กปุ่มวงกลม (JoystickButton2)
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
            return true;

        // fallback เช็คคีย์บอร์ด Enter / Space
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            return true;

        return false;
    }
}
