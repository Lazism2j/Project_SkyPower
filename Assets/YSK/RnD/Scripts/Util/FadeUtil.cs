using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.GlobalIllumination;

namespace YSK
{
    /// <summary>
    /// ���̵� ȿ���� �����ϴ� ��ƿ��Ƽ Ŭ����
    /// </summary>
    public class FadeUtil : MonoBehaviour
    {
        [Header("Transition Settings")]
        [SerializeField] private bool enableTransition = true;
        [SerializeField] private bool useFadeTransition = true;
        [SerializeField] private CanvasGroup fadePanel;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private Color fadeColor = Color.black;
        //[Header("Fade In/Out Settings")]
        //[Tooltip("���̵� �ƿ� ���̵��� ����")]
        //[SerializeField] private float fadeInDuration = 1f;
        //[SerializeField] private float fadeOutDuration = 1f;
        //[SerializeField] private float waitTime = 0.1f;

        [Header("Auto Setup")]
        [SerializeField] private bool autoCreateFadePanel = true;
        [SerializeField] private int canvasSortOrder = 999;
        
        // �̱��� ����
        public static FadeUtil Instance { get; private set; }
        
        // �̺�Ʈ
        public static System.Action OnFadeOutCompleted;
        public static System.Action OnFadeInCompleted;
        
        // ������Ƽ
        public bool IsTransitioning { get; private set; }
        public bool IsFadeOut => fadePanel != null && fadePanel.alpha >= 1f;
        public bool IsFadeIn => fadePanel != null && fadePanel.alpha <= 0f;
        
        #region Unity Lifecycle
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeFadePanel();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            if (fadePanel != null)
            {
                // �ʱ� ���� ����
                fadePanel.alpha = 0f;
                fadePanel.gameObject.SetActive(false);
            }
        }
        
        #endregion
        
        #region Public API
        
        /// <summary>
        /// ���̵� �ƿ� ȿ�� ����
        /// </summary>
        /// <param name="duration">���̵� �ð� (�⺻��: ������ �ð� ���)</param>
        /// <param name="curve">�ִϸ��̼� Ŀ�� (�⺻��: ������ Ŀ�� ���)</param>
        public void FadeOut(float? duration = null, AnimationCurve curve = null)
        {
            if (!enableTransition || !useFadeTransition)
            {
                Debug.Log("���̵� ȿ���� ��Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
                return;
            }
            
            if (IsTransitioning)
            {
                Debug.LogWarning("�̹� ��ȯ ���Դϴ�!");
                return;
            }
            
            StartCoroutine(FadeOutCoroutine(duration ?? fadeDuration, curve ?? fadeCurve));
        }
        
        /// <summary>
        /// ���̵� �� ȿ�� ����
        /// </summary>
        /// <param name="duration">���̵� �ð� (�⺻��: ������ �ð� ���)</param>
        /// <param name="curve">�ִϸ��̼� Ŀ�� (�⺻��: ������ Ŀ�� ���)</param>
        public void FadeIn(float? duration = null, AnimationCurve curve = null)
        {
            if (!enableTransition || !useFadeTransition)
            {
                Debug.Log("���̵� ȿ���� ��Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
                return;
            }
            
            if (IsTransitioning)
            {
                Debug.LogWarning("�̹� ��ȯ ���Դϴ�!");
                return;
            }
            
            StartCoroutine(FadeInCoroutine(duration ?? fadeDuration, curve ?? fadeCurve));
        }
        
        /// <summary>
        /// ���̵� �ƿ� �� ���̵� �� ����
        /// </summary>
        /// <param name="fadeOutDuration">���̵� �ƿ� �ð�</param>
        /// <param name="fadeInDuration">���̵� �� �ð�</param>
        /// <param name="waitTime">�߰� ��� �ð�</param>
        public void FadeOutThenIn(float fadeOutDuration = 1f, float fadeInDuration = 1f, float waitTime = 0.1f)
        {
            StartCoroutine(FadeOutThenInCoroutine(fadeOutDuration, fadeInDuration, waitTime));
        }
        
        /// <summary>
        /// ��� ���̵� �ƿ� (���İ��� 1�� ����)
        /// </summary>
        public void FadeOutImmediate()
        {
            if (fadePanel != null)
            {
                fadePanel.alpha = 1f;
                fadePanel.gameObject.SetActive(true);
                OnFadeOutCompleted?.Invoke();
            }
        }
        
        /// <summary>
        /// ��� ���̵� �� (���İ��� 0���� ����)
        /// </summary>
        public void FadeInImmediate()
        {
            if (fadePanel != null)
            {
                fadePanel.alpha = 0f;
                fadePanel.gameObject.SetActive(false);
                OnFadeInCompleted?.Invoke();
            }
        }
        
        /// <summary>
        /// ��ȯ ���� ������Ʈ
        /// </summary>
        public void SetTransitionSettings(bool enable, bool useFade, CanvasGroup panel, float duration, AnimationCurve curve, Color color)
        {
            enableTransition = enable;
            useFadeTransition = useFade;
            fadePanel = panel;
            fadeDuration = duration;
            fadeCurve = curve;
            fadeColor = color;
            
            if (fadePanel != null)
            {
                fadePanel.gameObject.SetActive(true);
                fadePanel.alpha = 0f;
                
                // Image ������Ʈ ���� ����
                Image fadeImage = fadePanel.GetComponent<Image>();
                if (fadeImage != null)
                {
                    fadeImage.color = fadeColor;
                }
                else
                {
                    Debug.LogWarning("���̵� �гο� Image ������Ʈ�� �����ϴ�!");
                }
            }
        }
        
        /// <summary>
        /// ���̵� �г� ���� ����
        /// </summary>
        public void SetFadeColor(Color color)
        {
            fadeColor = color;
            if (fadePanel != null)
            {
                Image fadeImage = fadePanel.GetComponent<Image>();
                if (fadeImage != null)
                {
                    fadeImage.color = color;
                }
            }
        }
        
        /// <summary>
        /// ���̵� ���� �ð� ����
        /// </summary>
        public void SetFadeDuration(float duration)
        {
            fadeDuration = Mathf.Max(0.1f, duration);
        }
        
        #endregion
        
        #region Private Methods
        
        private void InitializeFadePanel()
        {
            if (fadePanel == null && autoCreateFadePanel)
            {
                CreateFadePanel();
            }
        }
        
        private void CreateFadePanel()
        {
            // Canvas ã�� �Ǵ� ����
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("FadeCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = canvasSortOrder;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                DontDestroyOnLoad(canvasObj);
            }
            
            // ���̵� �г� ����
            GameObject fadeObj = new GameObject("FadePanel");
            fadeObj.transform.SetParent(canvas.transform, false);
            
            // Image ������Ʈ �߰�
            Image fadeImage = fadeObj.AddComponent<Image>();
            fadeImage.color = fadeColor;
            
            // RectTransform ����
            RectTransform rectTransform = fadeObj.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            // CanvasGroup �߰�
            fadePanel = fadeObj.AddComponent<CanvasGroup>();
            fadePanel.alpha = 0f;
            fadePanel.gameObject.SetActive(false);
            
            Debug.Log("���̵� �г��� �ڵ����� �����Ǿ����ϴ�.");
        }
        
        private IEnumerator FadeOutCoroutine(float duration, AnimationCurve curve)
        {
            if (fadePanel == null)
            {
                Debug.LogError("���̵� �г��� null�Դϴ�!");
                yield break;
            }
            
            IsTransitioning = true;
            fadePanel.gameObject.SetActive(true);
            
            float elapsed = 0f;
            float startAlpha = fadePanel.alpha;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                fadePanel.alpha = Mathf.Lerp(startAlpha, 1f, curve.Evaluate(t));
                yield return null;
            }
            
            fadePanel.alpha = 1f;
            IsTransitioning = false;
            OnFadeOutCompleted?.Invoke();
        }
        
        private IEnumerator FadeInCoroutine(float duration, AnimationCurve curve)
        {
            if (fadePanel == null)
            {
                Debug.LogError("���̵� �г��� null�Դϴ�!");
                yield break;
            }
            
            IsTransitioning = true;
            fadePanel.gameObject.SetActive(true);
            
            float elapsed = 0f;
            float startAlpha = fadePanel.alpha;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                fadePanel.alpha = Mathf.Lerp(startAlpha, 0f, curve.Evaluate(t));
                yield return null;
            }
            
            fadePanel.alpha = 0f;
            fadePanel.gameObject.SetActive(false);
            IsTransitioning = false;
            OnFadeInCompleted?.Invoke();
        }
        
        private IEnumerator FadeOutThenInCoroutine(float fadeOutDuration, float fadeInDuration, float waitTime)
        {
            // ���̵� �ƿ�
            yield return StartCoroutine(FadeOutCoroutine(fadeOutDuration, fadeCurve));
            
            // �߰� ���
            yield return new WaitForSeconds(waitTime);
            
            // ���̵� ��
            yield return StartCoroutine(FadeInCoroutine(fadeInDuration, fadeCurve));
        }
        
        #endregion
        
        #region Static Utility Methods
        
        /// <summary>
        /// ���� �޼���� ���̵� �ƿ� ����
        /// </summary>
        public static void StaticFadeOut(float duration = 1f)
        {
            if (Instance != null)
            {
                Instance.FadeOut(duration);
            }
            else
            {
                Debug.LogError("FadeUtil �ν��Ͻ��� �����ϴ�!");
            }
        }
        
        /// <summary>
        /// ���� �޼���� ���̵� �� ����
        /// </summary>
        public static void StaticFadeIn(float duration = 1f)
        {
            if (Instance != null)
            {
                Instance.FadeIn(duration);
            }
            else
            {
                Debug.LogError("FadeUtil �ν��Ͻ��� �����ϴ�!");
            }
        }
        
        #endregion
    }
}