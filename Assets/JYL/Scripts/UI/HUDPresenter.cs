using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using KYG_skyPower;

namespace JYL
{
    public class HUDPresenter : BaseUI
    {
        [SerializeField] PlayerController player;
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
        private float parryCooltime = 2f;
        private float progressTime = 10f;
        private float pgTimer = 0;

        private Slider hpBar => GetUI<Slider>("HPBar");
        private Slider pgBar => GetUI<Slider>("ProgressBar");
        private Image parry1BackImg => GetUI<Image>("ParryImgBack");
        private Image parry1Img => GetUI<Image>("ParryImgFront");
        private Image parry2BackImg => GetUI<Image>("Parry2ImgBack");
        private Image parry2Img => GetUI<Image>("Parry2ImgFront");
        private Image ultGageImg => GetUI<Image>("UltImgFront");
        private Image ultGageBackImg => GetUI<Image>("UltImgBack");
        private Image ultIllust => GetUI<Image>("UltImg");
        private Image parryIllust => GetUI<Image>("ParryImg");

        // private Animator ultAnimator => GetUI<Animator>("UltImg");
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
        //private void OnDisable()
        //{
        //    UnSubscribeEvent();
        //}

        void Update()
        {
            // TODO :  ü�¹� �׽�Ʈ. �÷��̾� �ǰݽ� amount��ŭ ����. �÷��̾� ��Ʈ�ѷ����� �̺�Ʈ �ɼ��� ����.
            // ����, �÷��̾� ��Ʈ�ѷ����� �̺�Ʈ�� �ش� ��ɵ��� �����Ѵ�
            if(Input.GetKey(KeyCode.Space))
            {
                UltGage += Time.deltaTime;
                if(UltGage>1)
                {
                    UseUltimate();
                }
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

            maxHp = player.Hp;
            CurHp = player.Hp;
            ultGage = 0;

            hpBar.value = (float)curHp / maxHp;

            ultIllust.sprite = player.mainCharController.image;
            ultGageImg.sprite = player.mainCharController.icon;
            ultGageBackImg.sprite = player.mainCharController.icon;
            parry1Img.sprite = player.sub1CharController.icon;
            parry1BackImg.sprite = player.sub1CharController.icon;
            parry2Img.sprite = player.sub2CharController.icon;
            parry2BackImg.sprite = player.sub2CharController.icon;

            parryIllust.gameObject.SetActive(false);
            ultIllust.gameObject.SetActive(false);

            //SubscribeEvents();

        }
        public void OnHpChanged()
        {
            hpBar.value = (float)curHp / maxHp;
        }
        private void OnGageChanged()
        {
            ultGageImg.fillAmount = UltGage;
        }
        //private void OnHpBarChanged(float value)
        //{
        //    // Handle HP change logic here
        //    // �ǰ� UI ȿ���� ���⼭ �� �� ����
        //}
        //private void SubscribeEvents()
        //{
        //    hpBar.onValueChanged.AddListener(OnHpBarChanged);
        //}
        //private void UnSubscribeEvent()
        //{
        //    hpBar.onValueChanged.RemoveListener(OnHpBarChanged);
        //}

        private void SetProgressBar()
        {
            pgBar.value = pgTimer / progressTime;
        }
        private void UseUltimate()
        {
            UltGage = 0;
            ultIllust.gameObject.SetActive(true);
            ultAnimator.Play("ActivateSkill");
        }

        public void UseParry1()
        {
            if (parry1CooldownRoutine == null)
            {
                parry1CooldownRoutine = StartCoroutine(Parry1Routine());
            }
        }
        IEnumerator Parry1Routine()
        {
            parryIllust.sprite = player.sub1CharController.image;
            parryIllust.gameObject.SetActive(true);
            parry1Img.fillAmount = 0;
            float timer = 0;
            parryAnimator.Play("ActiveParry");
            while (true)
            {
                if (timer > parryCooltime)
                {
                    timer = 0;
                    StopCoroutine(parry1CooldownRoutine);
                    parry1CooldownRoutine = null;
                    parryIllust.gameObject.SetActive(false);
                    break;
                }
                else
                {
                    parry1Img.fillAmount = (float)timer / parryCooltime;
                }
                timer += Time.deltaTime;
                yield return null;

            }
        }
        public void UseParry2()
        {
            if (parry2CooldownRoutine == null)
            {
                parry2CooldownRoutine = StartCoroutine(Parry1Routine());
            }
        }
        IEnumerator Parry2Routine()
        {
            parryIllust.sprite = player.sub2CharController.image;
            parryIllust.gameObject.SetActive(true);
            parry2Img.fillAmount = 0;
            float timer = 0;
            parryAnimator.Play("ActiveParry");
            while (true)
            {
                if (timer > parryCooltime)
                {
                    timer = 0;
                    StopCoroutine(parry2CooldownRoutine);
                    parry2CooldownRoutine = null;
                    parryIllust.gameObject.SetActive(false);
                    break;
                }
                else
                {
                    parry2Img.fillAmount = (float)timer / parryCooltime;
                }
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}

