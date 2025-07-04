using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JYL
{
    public class HUDPresenter : BaseUI
    {
        private int maxHp { get; set; } // �÷��̾� ��Ʈ�ѷ����� ü�� ������

        private int curHp;
        public int CurHp
        {
            get { return curHp; }
            set
            {
                curHp = value;
                OnHpChanged();
            }

        } // �÷��̾� ��Ʈ�ѷ����� ���� ü�� ������
        private float ultGage; // ĳ���� ��Ʈ�ѷ����� ���� ������
        public float UltGage
        {
            get { return ultGage; }
            set
            {
                ultGage = value;
                OnGageChanged();
            }
        }
        private float parry1Cooltime = 2f;
        private float parry2Cooltime = 3f;
        private float progressTime = 10f;
        private float pgTimer = 0;

        private Slider hpBar => GetUI<Slider>("HPBar");
        private Slider pgBar => GetUI<Slider>("ProgressBar");
        private Image parry1Img => GetUI<Image>("ParryImgFront");
        private Image parry2Img => GetUI<Image>("Parry2ImgFront");
        private Image ultGageImg => GetUI<Image>("UltImgFront");
        private Image ultImg => GetUI<Image>("UltImg");

        // private Animator ultAnimator => GetUI<Animator>("UltImg");
        private Image parryImg => GetUI<Image>("ParryImg");
        private Animator ultAnimator => GetUI<Animator>("UltImg");
        private Animator parryAnimator => GetUI<Animator>("ParryImg");

        [SerializeField] private Sprite ultSprite;
        [SerializeField] private Sprite parry1Sprite;
        [SerializeField] private Sprite parry2Sprite;

        private Coroutine parry1CooldownRoutine;
        private Coroutine parry2CooldownRoutine;
        void Start()
        {
            // TODO: ���⼭ �̹����� ä������.

        }
        private void OnEnable()
        {
            Init();
        }
        private void OnDisable()
        {
            UnSubscribeEvent();
        }

        void Update()
        {
            // TODO :  ü�¹� �׽�Ʈ. �÷��̾� �ǰݽ� amount��ŭ ����. �÷��̾� ��Ʈ�ѷ����� �̺�Ʈ �ɼ��� ����.
            // ����, �÷��̾� ��Ʈ�ѷ����� �̺�Ʈ�� �ش� ��ɵ��� �����Ѵ�
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CurHp--;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CurHp++;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (parry1CooldownRoutine == null)
                {

                    parry1CooldownRoutine = StartCoroutine(Parry1Routine());
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (parry2CooldownRoutine == null)
                {

                    parry2CooldownRoutine = StartCoroutine(Parry2Routine());
                }
            }
            if (Input.GetKey(KeyCode.Space))
            {
                UltGage += Time.deltaTime;
                if (UltGage > 1)
                {
                    UseUltimate();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape) && !PopUpUI.IsPopUpActive && !Util.escPressed)
            {
                UIManager.Instance.ShowPopUp<StagePopUp>();
                Util.ConsumeESC();
                Time.timeScale = 0;
            }

            if (!PopUpUI.IsPopUpActive)
            {
                Time.timeScale = 1;
            }
        }
        private void LateUpdate()
        {
            // ���α׷����� ���, �������� �Ŵ������� ���̺� Ŭ���� ������ �����´�
            if (pgTimer <= progressTime)
            {
                pgTimer += Time.deltaTime;
                SetProgressBar();
            }
        }
        private void Init()
        {

            maxHp = 100;
            CurHp = 56;
            hpBar.value = (float)curHp / maxHp;
            parryImg.sprite = parry1Sprite;
            ultImg.sprite = ultSprite;
            parryImg.sprite = parry2Sprite;

            ultGage = 0;
            parryImg.gameObject.SetActive(false);
            ultImg.gameObject.SetActive(false);

            SubscribeEvents();

        }
        private void OnHpChanged()
        {
            hpBar.value = (float)curHp / maxHp;
        }
        private void OnGageChanged()
        {
            ultGageImg.fillAmount = UltGage;
        }
        private void OnHpBarChanged(float value)
        {
            // Handle HP change logic here
            // �ǰ� UI ȿ���� ���⼭ �� �� ����
        }
        private void SubscribeEvents()
        {
            hpBar.onValueChanged.AddListener(OnHpBarChanged);
        }
        private void UnSubscribeEvent()
        {
            hpBar.onValueChanged.RemoveListener(OnHpBarChanged);
        }

        private void SetProgressBar()
        {
            pgBar.value = pgTimer / progressTime;
        }
        private void UseUltimate()
        {
            UltGage = 0;
            ultImg.gameObject.SetActive(true);
            ultAnimator.Play("ActivateSkill");
        }
        IEnumerator Parry1Routine()
        {
            parryImg.gameObject.SetActive(true);
            parry1Img.fillAmount = 0;
            float timer = 0;
            parryAnimator.Play("ActiveParry");
            while (true)
            {
                if (timer > parry1Cooltime)
                {
                    timer = 0;
                    StopCoroutine(parry1CooldownRoutine);
                    parry1CooldownRoutine = null;
                    parryImg.gameObject.SetActive(false);
                    break;
                }
                else
                {
                    parry1Img.fillAmount = (float)timer / parry1Cooltime;
                }
                timer += Time.deltaTime;
                yield return null;

            }
        }
        IEnumerator Parry2Routine()
        {
            parryImg.gameObject.SetActive(true);
            parry2Img.fillAmount = 0;
            float timer = 0;
            parryAnimator.Play("ActiveParry");
            while (true)
            {
                if (timer > parry2Cooltime)
                {
                    timer = 0;
                    StopCoroutine(parry2CooldownRoutine);
                    parry2CooldownRoutine = null;
                    parryImg.gameObject.SetActive(false);
                    break;
                }
                else
                {
                    parry2Img.fillAmount = (float)timer / parry2Cooltime;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}

