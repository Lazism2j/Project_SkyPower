using LJ2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using KYG_skyPower;

namespace JYL
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Set Scriptable Object")]
        [SerializeField] HUDPresenter hud;
        [Header("Set References")]
        [SerializeField] List<ObjectPool> bulletPools;
        [field:SerializeField] public Transform muzzlePoint { get; set; }
        [SerializeField] RectTransform leftUI;
        [SerializeField] RectTransform rightUI;
        public static bool canAttack = true; // �ñر� ��� ��, �ܺο��� ���� ���� ����

        [field:Header("Set Value")]
        [field:Range(10f,50f)][field:SerializeField] private float bulletSpeed { get; set; } = 20f;
        [Range(0.1f, 5)][SerializeField] float bulletReturnTimer = 2f;

        public UnityEvent<int> onHpChanged;
        private PlayerInput playerInput;
        private Rigidbody rig;
        private InputAction attackAction;
        private InputAction parryAction1;
        private InputAction parryAction2;
        private InputAction ultAction;

        public CharactorController mainCharController;
        public CharactorController sub1CharController;
        public CharactorController sub2CharController;
        private CharacterSaveLoader charDataLoader;

        public CharactorController inGameController;

        private int hp;
        public int Hp
        {
            get { return hp; }
            private set 
            {
                hp = value;
                onHpChanged?.Invoke(hp);
                curBulletPool.ObjectOut();
            }
        }

        private int attackPower { get; set; }
        private float moveSpeed { get; set; }
        private bool isDead { get; set; } = false;


        private int fireAtOnce { get; set; } = 3;
        private int fireCounter { get; set; }
        private float canAttackTime { get; set; } = 0.4f;
        private int ultGage { get; set; } = 0;

        // ��, �� UI ������
        private float leftMargin;
        private float rightMargin;

        public int poolIndex { get; set; } = 0;
        private Vector2 inputDir;

        private bool isAttack;

        private float parryTimer { get; set; } = 0;
        private float attackInputTimer;

        public ObjectPool curBulletPool => bulletPools[poolIndex];
        private Coroutine fireRoutine;

        private void Awake()=> Init();
        private void OnEnable()
        {
            rig = GetComponent<Rigidbody>();
            SubscribeEvents();
        }
        private void Update()
        {
            SetMove();
            if (attackInputTimer > 0)
            {
                attackInputTimer -= Time.deltaTime;
            }
            if (parryTimer > 0)
            {
                parryTimer -= Time.deltaTime;
            }

            if(Input.GetKeyDown(KeyCode.T))
            {
                GetUltGage(50);
            }
        }

        private void FixedUpdate() { }

        private void LateUpdate() { }

        private void OnDisable() => UnSubscribeEvents();
        private void Init()
        {
            playerInput = GetComponent<PlayerInput>();
            charDataLoader = GetComponent<CharacterSaveLoader>();
            charDataLoader.GetCharPrefab();
           
            mainCharController = charDataLoader.mainController;
            
            if(charDataLoader.sub1Controller.grade != Grade.R)
            {
                sub1CharController = charDataLoader.sub1Controller;
            }

            if(charDataLoader.sub1Controller.grade != Grade.R)
            {
                sub2CharController = charDataLoader.sub2Controller;
            }
            inGameController = Instantiate(mainCharController.gameObject, transform).GetComponent<CharactorController>();
            // CharactorController character = gameObject.AddComponent<CharactorController>();

            // ������Ʈ Ǯ ����
            bulletPools[0].poolObject = mainCharController.bulletPrefab;
            bulletPools[0].CreatePool();
            // �ñر� ź�� ������Ʈ Ǯ
            if(mainCharController.ultBulletPrefab != null)
            {
                bulletPools[1].poolObject = mainCharController.ultBulletPrefab;
                bulletPools[1].CreatePool();
            }

            // Input System ����
            attackAction = playerInput.actions["Attack"];
            ultAction = playerInput.actions["Ult"];
            parryAction1 = playerInput.actions["Parry1"];
            parryAction2 = playerInput.actions["Parry2"];



            if (leftUI != null)
            {
                leftMargin = leftUI.rect.width / Camera.main.pixelWidth;
            }
            else
            {
                leftMargin = 0.1f;
                Debug.LogWarning("���� UI ���� �ȵ���");
            }
            if (rightUI != null)
            {
                rightMargin = 1 - rightUI.rect.width / Camera.main.pixelWidth;
            }
            else
            {
                rightMargin = 0.9f;
                Debug.LogWarning("������ UI ���� �ȵ���");
            }

            CharacterParameterSetting();

        }
        // ĳ���� �ʵ� ����
        private void CharacterParameterSetting()
        {
            //mainCharController.model ����
            hp = mainCharController.Hp;
            attackPower = mainCharController.attackDamage;
            moveSpeed = mainCharController.moveSpeed;
        }
               

        private void SetMove()
        {
            Vector2 clampInput = ClampMoveInput(inputDir);
            if (clampInput == Vector2.zero)
            {
                rig.velocity = Vector3.zero;
                return;
            }
            Vector3 moveDir = new Vector3(clampInput.x, 0f, clampInput.y);
            rig.velocity = moveDir * moveSpeed*0.05f;
        }

        private Vector2 ClampMoveInput(Vector2 inputDirection)
        {
            if (inputDirection == Vector2.zero)
            {
                return Vector2.zero;
            }
            
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPos.z <= 0) return Vector2.zero;

            if (viewportPos.x <= leftMargin+0.01f && inputDirection.x < 0) inputDirection.x = 0;
            if (viewportPos.x >= rightMargin-0.01f && inputDirection.x > 0) inputDirection.x = 0;

            if (viewportPos.y-0.01f <= 0 && inputDirection.y < 0) inputDirection.y = 0;
            if (viewportPos.y+0.01f >= 1 && inputDirection.y > 0) inputDirection.y = 0;

            return inputDirection;
        }

        public void OnMove(InputValue value)
        {
            inputDir = value.Get<Vector2>();
        }

        private void Fire(InputAction.CallbackContext ctx)
        {
            if (canAttack)
            {
                if (fireCounter <= fireAtOnce && fireCounter > 0 && 1f * canAttackTime > attackInputTimer && attackInputTimer > 0)
                {
                    fireCounter += fireAtOnce;
                    isAttack = true;
                    attackInputTimer = canAttackTime;
                }
                else if (fireCounter <= 0 && fireRoutine == null)
                {
                    fireCounter = fireAtOnce;
                    attackInputTimer = canAttackTime;
                    fireRoutine = StartCoroutine(FireRoutine());
                }
            }
        }
        IEnumerator FireRoutine()
        {
            while (fireCounter>0)
            {
                fireCounter--;
                AudioManager.Instance.PlaySFX($"{mainCharController.attackSound}");
                BulletPrefabController bulletPrefab = curBulletPool.ObjectOut() as BulletPrefabController;
                bulletPrefab.transform.position = muzzlePoint.position;
                bulletPrefab.ReturnToPool(bulletReturnTimer);
                foreach (BulletInfo info in bulletPrefab.bulletInfo)
                {
                    if (info.rig == null)
                    {
                        continue;
                    }
                    info.trans.gameObject.SetActive(true);
                    info.trans.localPosition = info.originPos;
                    info.rig.velocity = Vector3.zero;
                    if(poolIndex == 0)
                    {
                        if (info.bulletController == null) Debug.Log($"�ҷ���Ʈ�ѷ� Null");
                        info.bulletController.attackPower = this.attackPower;
                    }
                    else if(poolIndex == 1)
                    {
                        info.bulletController.attackPower = (int)mainCharController.ultDamage;
                        // info.bulletController.canDeactive = false; �ٴ���Ʈ�� �� Ȱ��ȭ
                    }
                    info.rig.AddForce(bulletSpeed * info.trans.forward, ForceMode.Impulse); // �� �κ��� Ŀ�����ϸ� ��
                    info.bulletController.OnFire(); // �߻�� ���ÿ� �÷��û���
                }
                yield return new WaitForSeconds(mainCharController.attackSpeed*0.1f);
            }
            if (isAttack)
            {
                isAttack = false;
            }
            StopCoroutine(fireRoutine);
            fireRoutine = null;
        }

        public void TakeDamage(int damage)
        {
            Debug.Log($"ü�� �̸�ŭ ���� : {damage}");
            if(mainCharController.defense>0)
            {
                mainCharController.defense -= 1;
                return;
            }
            if(mainCharController.defense <=0)
            {
                Hp-=damage;
                Debug.Log($"���� ü�� : {Hp}");
                if(Hp <= 0&&!isDead)
                {
                    isDead = true;
                    Hp = 0;
                    Manager.Game.SetGameOver();
                }
                hud.CurHp = Hp;
            }
        }

        public void GetUltGage(int amount)
        {
            if(ultGage + amount >100)
            {
                ultGage = 100;
                hud.UltGage = 1f;
            }
            else
            {
                ultGage += amount;
                if(hud == null)
                {
                    Debug.Log("hud�� ��");
                }
                hud.UltGage = (float)ultGage / 100;
            }
            
        }

        private void UseUlt(InputAction.CallbackContext ctx)
        {
            if (ultGage >= 100)
            {
                hud.UseUltimate();
                inGameController.UseUlt();
                ultGage = 0;
            }
        }
        private void UseParry1(InputAction.CallbackContext ctx)
        {
            if (sub1CharController!= null && parryTimer <= 0)
            {
                inGameController.UseParry(sub1CharController.parry);
                parryTimer = sub1CharController.parryCool;
            }
        }
        private void UseParry2(InputAction.CallbackContext ctx)
        {
            if (sub2CharController != null && parryTimer <= 0)
            {
                inGameController.UseParry(sub2CharController.parry);
                parryTimer = sub2CharController.parryCool;
            }
        }
        private void SubscribeEvents()
        {
            attackAction.started += Fire;
            ultAction.started += UseUlt;
            parryAction1.started += UseParry1;
            parryAction2.started += UseParry2;
            ScoreManager.Instance.onScoreChanged.AddListener(GetUltGage);
        }
        private void UnSubscribeEvents()
        {
            attackAction.started -= Fire;
            ultAction.started -= UseUlt;
            parryAction1.started -= UseParry1;
            parryAction2.started -= UseParry2;
            ScoreManager.Instance.onScoreChanged.RemoveListener(GetUltGage);
        }
    }

}