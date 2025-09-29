using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Fade")]
    public Image fadeImage;           // UI Image สีดำเต็มจอ
    public float fadeDuration = 1f;

    private Vector2 nextPlayerPosition;
    private const string PLAYER_TAG = "Player";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // เผื่อผู้ใช้กดเปลี่ยนซีนด้วยวิธีอื่น เราจะรีบอินด์กล้องทุกครั้งที่ซีนโหลด
            SceneManager.sceneLoaded += OnSceneLoadedSafeSetup;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoadedSafeSetup;
    }

    public void StartSceneTransition(string sceneName, Vector2 playerPos)
    {
        nextPlayerPosition = playerPos;
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        // เตรียม fade image ถ้ายังไม่ได้เสียบใน Inspector
        EnsureFadeImage();

        // เฟดดำ
        yield return StartCoroutine(Fade(1f));

        // โหลดซีนแบบ async
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // เซ็ตตำแหน่ง Player ถ้ามี
        RepositionPlayer(nextPlayerPosition);

        // รีบอินด์ Cinemachine ให้ตาม Player เสมอ
        RetargetAllCinemachineCameras();

        // เฟดกลับสว่าง
        yield return StartCoroutine(Fade(0f));
    }

    private void OnSceneLoadedSafeSetup(Scene scene, LoadSceneMode mode)
    {
        // กันพลาด: ถ้ามีการเปลี่ยนซีนด้วยวิธีอื่น ให้กล้องตามทันที
        RetargetAllCinemachineCameras();
    }

    // ---------- Helpers ----------

    private void RepositionPlayer(Vector2 pos)
    {
        Transform player = FindPlayerTransform();
        if (player != null)
        {
            player.position = pos;
            // ถ้ามี Rigidbody2D ก็ตัดโมเมนตัมออกด้วย
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb) rb.velocity = Vector2.zero;
        }
    }

    private void RetargetAllCinemachineCameras()
    {
        Transform player = FindPlayerTransform();
        if (player == null) return;

        // ให้ทุก vcam ในซีนชี้ Follow (และ LookAt ถ้าต้องการ) ไปที่ Player เสมอ
        var vcams = FindObjectsOfType<CinemachineVirtualCamera>(true);
        foreach (var vcam in vcams)
        {
            vcam.Follow = player;
            // ถ้าเป็น 3D/ต้องการหันกล้องใส่เป้าหมายด้วย ให้เปิดบรรทัดนี้
            // vcam.LookAt = player;
        }

        // ย้ำว่ามีกล้องหลักที่ติด CinemachineBrain อยู่
        EnsureCinemachineBrainOnMainCamera();
    }

    private Transform FindPlayerTransform()
    {
        // แนะนำให้ตั้ง Tag = "Player" ไว้ที่ตัวละครที่ DontDestroyOnLoad
        GameObject playerGO = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        return playerGO ? playerGO.transform : null;
    }

    private void EnsureCinemachineBrainOnMainCamera()
    {
        Camera cam = Camera.main; // ต้องมี Tag = MainCamera
        if (cam == null) return;

        var brain = cam.GetComponent<CinemachineBrain>();
        if (brain == null) cam.gameObject.AddComponent<CinemachineBrain>();
    }

    private void EnsureFadeImage()
    {
        if (fadeImage != null) return;

        // สร้าง Canvas + Image สีดำทับจอแบบอัตโนมัติ
        var canvasGO = new GameObject("FadeCanvas (Auto)");
        DontDestroyOnLoad(canvasGO);

        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue; // ให้อยู่บนสุด

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        var imgGO = new GameObject("FadeImage");
        imgGO.transform.SetParent(canvasGO.transform, false);
        fadeImage = imgGO.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // โปร่งใสเริ่มต้น

        var rt = fadeImage.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        fadeImage.raycastTarget = false; // ไม่บล็อกอินพุต
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null) yield break;

        float startAlpha = fadeImage.color.a;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(startAlpha, targetAlpha, t / fadeDuration);
            var c = fadeImage.color;
            c.a = a;
            fadeImage.color = c;
            yield return null;
        }

        // Snap ค่าให้เป๊ะ
        var final = fadeImage.color;
        final.a = targetAlpha;
        fadeImage.color = final;
    }
}
