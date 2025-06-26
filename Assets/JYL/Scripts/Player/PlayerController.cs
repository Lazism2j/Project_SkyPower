using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace JYL
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Set Scriptable Object")]
        [SerializeField] PlayerModel playerModel; // -> ĳ���� ��Ʈ�ѷ��� ��ü ����
        // CharacterController[] character -> ĳ���� 3���� �迭�� ����. ���ӸŴ����� ��Ƽ������ ������

        [Header("Set References")]
        [SerializeField] List<ObjectPool> bulletPools;
        [SerializeField] Transform muzzlePoint;
        [SerializeField] RectTransform leftUI;
        [SerializeField] RectTransform rightUI;
        // TODO : UI �ý��� ���� ��, �ý��ۿ��� �ҷ����� ������ ���� ��Ų��


        [Header("Set Value")]
        [Range(0.1f, 5)][SerializeField] float bulletReturnTimer = 2f;
        [Range(0.1f, 3)][SerializeField] float fireDelay = 2f;

        private PlayerInput playerInput;
        private Rigidbody rig;
        private InputAction attackAction;
        //private InputAction parryAction1;
        //private InputAction parryAction2;
        //private InputAction ultAction;
        //private InputAction menuAction;

        // ��, �� UI ������
        private float leftMargin;
        private float rightMargin;

        //private int level;
        //private int hp;
        private int poolIndex = 0;
        private Vector2 inputDir;
        //private bool isAttack;

        private ObjectPool curBulletPool => bulletPools[poolIndex];
        private void Awake()
        {
            Init();
        }
        private void OnEnable()
        {
            //CreatePlayer();
            rig = GetComponent<Rigidbody>();
            SubscribeEvents();
        }
        private void Update()
        {
            PlayerHandler();
        }

        private void FixedUpdate()
        {

        }

        private void LateUpdate()
        {
            // �ִϸ��̼� - �ñر� ��
        }

        private void OnCollisionEnter(Collision collision)
        {
            // �� �Ѿ˿� ������ �ǰ�
        }
        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        //private void CreatePlayer()
        //{
        //   �÷��̾� ���� - ���ӸŴ����� ��Ƽ������ ������. 
        //   Instantiate(character[0].prefab, transform); -> ĳ���� ��Ʈ�ѷ��� �ִ� �������� ���� ĳ���� ����
        //   
        //}
        private void Init()
        {
            playerInput = GetComponent<PlayerInput>();
            attackAction = playerInput.actions["Attack"];
            // parryAction1 = playerInput.actions["parry1"];
            // parryAction2 = playerInput.actions["parry2"];
            // ultAction = playerInput.actions["Ult"];
            // menuAction = playerInput.actions["menu"];


            // TODO: UI �ý��� ���� ��, UI Manager���� �����ϴ� ������ ����
            if(leftUI!= null)
            {
                leftMargin = leftUI.rect.width/Camera.main.pixelWidth;
            }
            else
            {
                leftMargin = 0.1f;
                Debug.LogWarning("���� UI ���� �ȵ���");
            }
            if (rightUI != null)
            {
                rightMargin = 1- rightUI.rect.width/Camera.main.pixelWidth;
            }
            else
            {
                rightMargin = 0.9f;
                Debug.LogWarning("������ UI ���� �ȵ���");
            }
        }
        private void SubscribeEvents()
        {
            attackAction.started += Fire;
        }
        private void UnSubscribeEvents()
        {
            attackAction.started -= Fire;
        }

        private void PlayerHandler()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                poolIndex = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                poolIndex = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                poolIndex = 2;
            }
            SetMove();
            //UseUlt
            //Parry
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
            rig.velocity = moveDir * playerModel.playerSpeed;
        }

        private Vector2 ClampMoveInput(Vector2 inputDirection)
        {
            if (inputDirection == Vector2.zero)
            {
                return Vector2.zero;
            }

            // ī�޶� ���� ��ũ�� ��ǥ�� �Ǵ�
            //Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            // �÷��̾ ī�޶� �ڿ� �ִٴ� ��
            //if (screenPos.z <= 0) return Vector2.zero;

            //if (screenPos.x <= 0 && inputDirection.x < 0) inputDirection.x = 0;
            //if (screenPos.x >= Camera.main.pixelWidth && inputDirection.x > 0) inputDirection.x = 0;
            //if (screenPos.y <= 0 && inputDirection.y < 0) inputDirection.y = 0;
            //if (screenPos.y >= Camera.main.pixelHeight && inputDirection.y > 0) inputDirection.y = 0;

            // ����Ʈ ���� ��ǥ�� �� �� ����ȭ ����
            // ����Ʈ�� 0~1������ �����θ� ������ �ִ�. �ſ� ��Ȯ�ϰ� �������� �ʰ�����, ������� Ŀ���� �ȴ�

            Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
            if(viewportPos.z<=0) return Vector2.zero;

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
            BulletPrefabController bullet = curBulletPool.ObjectOut() as BulletPrefabController;
            bullet.transform.position = muzzlePoint.position;
            bullet.ReturnToPool(bulletReturnTimer);
            foreach (BulletInfo info in bullet.bullet)
            {
                if (info.rig == null)
                {
                    continue;
                }
                info.trans.gameObject.SetActive(true);
                info.trans.localPosition = info.originPos;
                info.rig.velocity = Vector3.zero;
                info.rig.AddForce(playerModel.fireSpeed * info.trans.forward, ForceMode.Impulse);
            }
        }
        //private void UseUlt()
        //{
        //  �ñر� -  �Է��� ���� ����
        //  if(ultGage>=100)
        //  character[0].Ult();
        //}

        //private void Parry(int index)
        //{
        //  ���� ĳ���Ϳ� ���� �и���ų ���
        //  character[index].Parry();
        //}
    }
}