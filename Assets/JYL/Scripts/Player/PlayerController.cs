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
        [SerializeField] PlayerModel playerModel; // -> ĳ���� ��Ʈ�ѷ��� ��ü ����
        [SerializeField] HUDPresenter hud;
        [Header("Set References")]
        [SerializeField] List<ObjectPool> bulletPools;
        [SerializeField] public Transform muzzlePoint { get; set; }
        [SerializeField] RectTransform leftUI;
        [SerializeField] RectTransform rightUI;
        [SerializeField] Animator animator;


        [Header("Set Value")]
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
        private float attackSpeed { get; set; }
        private float moveSpeed { get; set; }


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

        private void Awake()
        {
            Init();
        }
        private void OnEnable()
        {
            rig = GetComponent<Rigidbody>();
            SubscribeEvents();
        }
        private void Update()
        {
            PlayerHandler();
            if (attackInputTimer > 0)
            {
                attackInputTimer -= Time.deltaTime;
            }
            if (parryTimer > 0)
            {
                parryTimer -= Time.deltaTime;
            }
        }

        private void FixedUpdate() { }

        private void LateUpdate() 
        {
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void Init()
        {
            animator = GetComponent<Animator>();
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
            
            // ������Ʈ Ǯ ����
            bulletPools[0].poolObject = mainCharController.bulletPrefab;
            bulletPools[0].ClearPool();
            bulletPools[0].CreatePool();
            bulletPools[1].poolObject = mainCharController.ultBulletPrefab;
            bulletPools[1].ClearPool();
            bulletPools[1].CreatePool();

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
            attackSpeed = mainCharController.attackSpeed;
            moveSpeed = mainCharController.moveSpeed;
        }

        private void PlayerHandler()
        {
            SetMove();
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
            rig.velocity = moveDir * moveSpeed;
        }

        private Vector2 ClampMoveInput(Vector2 inputDirection)
        {
            if (inputDirection == Vector2.zero)
            {
                return Vector2.zero;
            }
            
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewportPos.z <= 0) return Vector2.zero;

            if (viewportPos.x <= leftMargin && inputDirection.x < 0) inputDirection.x = 0;
            if (viewportPos.x >= rightMargin && inputDirection.x > 0) inputDirection.x = 0;

            if (viewportPos.y <= 0 && inputDirection.y < 0) inputDirection.y = 0;
            if (viewportPos.y >= 1 && inputDirection.y > 0) inputDirection.y = 0;

            return inputDirection;
        }

        public void OnMove(InputValue value)
        {
            inputDir = value.Get<Vector2>();
        }

        private void Fire(InputAction.CallbackContext ctx)
        {
            if (fireCounter <= fireAtOnce && fireCounter > 0 && 0.5f * canAttackTime > attackInputTimer && attackInputTimer > 0)
            {
                fireCounter += fireAtOnce;
                isAttack = true;
                attackInputTimer = canAttackTime;
            }
            else if (fireCounter <= 0)
            {
                fireCounter = fireAtOnce;
                attackInputTimer = canAttackTime;
            }

            if (fireCounter > 0 && fireRoutine == null)
            {
                fireRoutine = StartCoroutine(FireRoutine());
            }

        }
        IEnumerator FireRoutine()
        {
            while (fireCounter > 1)
            {
                fireCounter--;
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
                        info.bulletController.attackPower = this.attackPower;
                    }
                    else if(poolIndex == 1)
                    {
                        info.bulletController.attackPower = (int)mainCharController.ultDamage;
                        // info.bulletController.canDeactive = false; �ٴ���Ʈ�� �� Ȱ��ȭ
                    }
                    info.rig.AddForce(playerModel.fireSpeed * info.trans.forward, ForceMode.Impulse); // �� �κ��� Ŀ�����ϸ� ��
                }
                yield return new WaitForSeconds(attackSpeed);
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
            if(mainCharController.defense>0)
            {
                mainCharController.defense -= 1;
                return;
            }
            if(mainCharController.defense <=0)
            {
                Hp-=damage;
                if(Hp <= 0)
                {
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
                hud.UltGage = (float)ultGage / 100;
            }
            
        }

        private void UseUlt(InputAction.CallbackContext ctx)
        {
            if (ultGage >= 100)
            {
                mainCharController.UseUlt();
                ultGage = 0;
            }
        }
        private void UseParry1(InputAction.CallbackContext ctx)
        {
            if (sub1CharController!= null && parryTimer <= 0)
            {
                sub1CharController.UseParry();
                parryTimer = sub1CharController.parryCool;
            }
        }
        private void UseParry2(InputAction.CallbackContext ctx)
        {
            if (sub2CharController != null && parryTimer <= 0)
            {
                sub2CharController.UseParry();
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

// ��ư ������ �°� �˾Ƽ� ��ġ�ǰ�

// Dictionary<string,Scene> sceneList;
// sceneList  ������ ��, ���忡 ���� �� �� ���� ����
// public int curScene  = sceneList[0]; // sceneList[scene.Title];

// 
// void SceneChange(string sceneName)
// {
//      �� ��ȯ ������ �Ͼ
//      �����°� �������� = 
//      -> ������ ���⼭ ����
//      ���������� ��ũ���ͺ� ������Ʈ�� �ʿ���
//      �������� ����
//      stage_1,1
//      string.split('_',',') -> string[] s = "stage","1","1" 
//      s[0] => curScene
//      int 
//      s[1],[2] => int.parse
//      curscene = 
//      1-1 => ���� ����, �������� ����
//      string ���� �޾Ƥ������
//      curScene = sceneName
// }
// 
// ������������ �޶����� �ϴ°�, �����;� �ϴ� ��
// ����Ʈ, ���ʹ�(������), ���ʹ���  ������, ����, ����Ʈ����
// �ʵ�����, �÷��̾� ������(�ڵ�), <= SceneChange
// 