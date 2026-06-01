using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.Events;
<<<<<<< Updated upstream
using UnityEditor.ShaderKeywordFilter;
using LootLocker.Extension.DataTypes;
=======

// Removido: using UnityEditor.ShaderKeywordFilter;
// Era um using de Editor desnecessário que causa erros em builds

>>>>>>> Stashed changes
namespace TempleRun.Player
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
<<<<<<< Updated upstream
        [SerializeField]
        private float initialPlayerSpeed = 10f;
        [SerializeField]
        private float maximumPlayerSpeed = 30f;
        [SerializeField]
        private float playerSpeedIncreaseRate = .1f;
        [SerializeField]
        private float playerSpeed;
        [SerializeField]
        private float jumpHeight = 1.0f;
        [SerializeField]
        private float initialGravityValue = -2.81f;
        [SerializeField]
        private LayerMask groundLayer;
        [SerializeField]
        private LayerMask turnLayer;
        [SerializeField]
        private UnityEvent<Vector3> turnEvent;
        [SerializeField]
        private UnityEvent<int> gameOverEvent;
        [SerializeField]
        private UnityEvent<int> scoreUpdateEvent;
        [SerializeField]
        private AnimationClip slideAnimationClip;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private LayerMask obstacleLayer;
        //[SerializeField]
        //private float scoreMultiplier = 10f;
        [SerializeField]
        private Transform characterMesh;
        [SerializeField]
        private float laneChangeDebounceDuration = 0.2f;
        private float laneChangeCooldown = 0f;
=======
        // ─────────────────────────────────────────────────────────
        //  Configuração existente (inalterada)
        // ─────────────────────────────────────────────────────────

        [SerializeField] private float initialPlayerSpeed    = 4f;
        [SerializeField] private float maximumPlayerSpeed    = 30f;
        [SerializeField] private float playerSpeedIncreaseRate = .1f;
        [SerializeField] private float playerSpeed;
        [SerializeField] private float jumpHeight            = 1.0f;
        [SerializeField] private float initialGravityValue   = -9.81f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask turnLayer;
        [SerializeField] private UnityEvent<Vector3> turnEvent;
        [SerializeField] private UnityEvent<int> gameOverEvent;
        [SerializeField] private UnityEvent<int> scoreUpdateEvent;
        [SerializeField] private AnimationClip slideAnimationClip;
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float scoreMultiplier = 10f;

        // ─────────────────────────────────────────────────────────
        //  NOVO — Configuração de Lanes
        // ─────────────────────────────────────────────────────────

        [Header("Lane Settings")]
        [Tooltip("Distância entre cada lane (esq/centro/dir)")]
        [SerializeField] private float laneWidth        = 2f;

        [Tooltip("Velocidade de suavização lateral — menor = mais rápido")]
        [SerializeField] private float laneSmoothTime   = 0.1f;

        [Tooltip("Velocidade máxima de movimento lateral")]
        [SerializeField] private float laneMaxSpeed     = 20f;

        // ─────────────────────────────────────────────────────────
        //  Estado interno existente (inalterado)
        // ─────────────────────────────────────────────────────────
>>>>>>> Stashed changes

        private float gravity;
        private Vector3 movementDirection = Vector3.forward;
        private Vector3 playerVelocity;

        private PlayerInput playerInput;
        private InputAction turnAction;
        private InputAction jumpAction;
        private InputAction slideAction;

        // ─────────────────────────────────────────────────────────
        //  NOVO — Estado interno de Lanes
        // ─────────────────────────────────────────────────────────

        private InputAction moveLeftAction;
        private InputAction moveRightAction;

        private int   currentLane       = 1;    // 0=esq  1=centro  2=dir
        private int   targetLane        = 1;
        private float targetLaneX       = 0f;   // posição X alvo em espaço local do tile
        private float laneVelocity      = 0f;   // usado internamente pelo SmoothDamp

        // ─────────────────────────────────────────────────────────
        //  Estado interno existente (inalterado)
        // ─────────────────────────────────────────────────────────

        private CharacterController controller;
<<<<<<< Updated upstream
        private bool sliding = false;
        private int slidingAnimationId;
        private float score = 0;
        private Vector3 lastTurnPosition;
        private float accumulatedDistance = 0f;
        public float laneDistance = 4f; // Distance between each lane
        private int desiredLane = 1;
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            controller = GetComponent<CharacterController>();
            slidingAnimationId = Animator.StringToHash("Armature|Slide");
            turnAction = playerInput.actions["Turn"];
            jumpAction = playerInput.actions["Jump"];
=======
        private bool  sliding           = false;
        private int   slidingAnimationId;
        private float score             = 0;

        // ─────────────────────────────────────────────────────────
        //  Awake
        // ─────────────────────────────────────────────────────────

        private void Awake()
        {
            playerInput        = GetComponent<PlayerInput>();
            controller         = GetComponent<CharacterController>();
            slidingAnimationId = Animator.StringToHash("Sliding");

            // Actions existentes
            turnAction  = playerInput.actions["Turn"];
            jumpAction  = playerInput.actions["Jump"];
>>>>>>> Stashed changes
            slideAction = playerInput.actions["Slide"];

            // NOVO — Actions de lane (tens de as criar no Input Actions Asset, ver nota abaixo)
            moveLeftAction  = playerInput.actions["MoveLeft"];
            moveRightAction = playerInput.actions["MoveRight"];

            gravity = initialGravityValue;
        }

        // ─────────────────────────────────────────────────────────
        //  OnEnable / OnDisable
        // ─────────────────────────────────────────────────────────

        private void OnEnable()
        {
            turnAction.performed      += PlayerTurn;
            slideAction.performed     += PlayerSlide;
            jumpAction.performed      += PlayerJump;

            // NOVO
            moveLeftAction.performed  += _ => TryChangeLane(-1);
            moveRightAction.performed += _ => TryChangeLane(+1);
        }

        private void OnDisable()
        {
            turnAction.performed      -= PlayerTurn;
            slideAction.performed     -= PlayerSlide;
            jumpAction.performed      -= PlayerJump;

            // NOVO
            moveLeftAction.performed  -= _ => TryChangeLane(-1);
            moveRightAction.performed -= _ => TryChangeLane(+1);
        }

        // ─────────────────────────────────────────────────────────
        //  Start
        // ─────────────────────────────────────────────────────────

        private void Start()
        {
            playerSpeed = initialPlayerSpeed;
<<<<<<< Updated upstream
            gravity = initialGravityValue;
            lastTurnPosition = transform.position;
            accumulatedDistance = 0f;
=======
            gravity     = initialGravityValue;
>>>>>>> Stashed changes
        }

        // ─────────────────────────────────────────────────────────
        //  Update
        // ─────────────────────────────────────────────────────────

        private void Update()
        {
            if (!isGrounded(20f))
            {
                GameOver();
                return;
            }

            // Score (inalterado)
            score += scoreMultiplier * Time.deltaTime;
            scoreUpdateEvent.Invoke((int)score);

            // ── Movimento para a frente (inalterado) ──────────────
            controller.Move(transform.forward * playerSpeed * Time.deltaTime);

            // ── NOVO — Movimento lateral suavizado ────────────────
            //
            // Calcula a posição X atual do player no espaço local
            // da direcção de movimento (perpendicular ao forward).
            //
            // Usamos transform.right para saber o eixo lateral correto,
            // mesmo após um Turn (quando o forward muda de eixo).
            //
            float currentLateralPos = Vector3.Dot(transform.localPosition, transform.right);

            float newLateralPos = Mathf.SmoothDamp(
                currentLateralPos,
                targetLaneX,
                ref laneVelocity,
                laneSmoothTime,
                laneMaxSpeed
            );

            // Delta lateral → vector no espaço do mundo
            float deltaLateral = newLateralPos - currentLateralPos;
            controller.Move(transform.right * deltaLateral);

            // Atualiza currentLane quando chegou ao destino
            if (Mathf.Abs(newLateralPos - targetLaneX) < 0.01f)
                currentLane = targetLane;

            // ── Gravidade (inalterado) ─────────────────────────────
            if (isGrounded() && playerVelocity.y < 0)
                playerVelocity.y = 0f;

            playerVelocity.y += gravity * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            // ── Aceleração (inalterado) ────────────────────────────
            if (playerSpeed < maximumPlayerSpeed)
            {
                playerSpeed += Time.deltaTime * playerSpeedIncreaseRate;
                gravity      = initialGravityValue - playerSpeed;

                if (animator.speed < 1.25f)
                    animator.speed += (1 / playerSpeed) * Time.deltaTime;
            }
        }

        // ─────────────────────────────────────────────────────────
        //  NOVO — Lógica de Lanes
        // ─────────────────────────────────────────────────────────

        /// <summary>
        /// Tenta mover para a lane adjacente.
        /// Bloqueia se ainda está em trânsito (evita duplo input).
        /// </summary>
        private void TryChangeLane(int direction)
        {
            // Guard: ignora se ainda não chegou à lane destino
            if (Mathf.Abs(Vector3.Dot(transform.localPosition, transform.right) - targetLaneX) > 0.05f)
                return;

            int newLane = Mathf.Clamp(targetLane + direction, 0, 2);
            if (newLane == targetLane) return; // já no limite

            targetLane  = newLane;
            targetLaneX = (targetLane - 1) * laneWidth;
            // Lane 0 → -laneWidth  |  Lane 1 → 0  |  Lane 2 → +laneWidth
        }

        /// <summary>
        /// Reset de lane ao fazer Turn — o pivot já reposiciona o player,
        /// por isso voltamos à lane central sem animação.
        /// </summary>
        private void ResetLaneOnTurn()
        {
            currentLane  = 1;
            targetLane   = 1;
            targetLaneX  = 0f;
            laneVelocity = 0f;
        }

        // ─────────────────────────────────────────────────────────
        //  Turn (adaptado — adicionado ResetLaneOnTurn)
        // ─────────────────────────────────────────────────────────

        private void PlayerTurn(InputAction.CallbackContext context)
        {
            float swipeDirection = context.ReadValue<float>();
            Vector3? turnPosition = CheckTurn(context.ReadValue<float>());

            if (turnPosition.HasValue)
            {
                if (!isGrounded()) return;

                Vector3 targetDirection = Quaternion.AngleAxis(90 * context.ReadValue<float>(), Vector3.up) * movementDirection;
                turnEvent.Invoke(targetDirection);
                Turn(context.ReadValue<float>(), turnPosition.Value);
            }
            else
            {
                if (sliding) return;

                if (laneChangeCooldown > 0f) return;

                if (swipeDirection < 0)
                {
                    desiredLane--;
                }
                else if (swipeDirection > 0)
                {
                    desiredLane++;
                }

                desiredLane = Mathf.Clamp(desiredLane, 0, 2);
                laneChangeCooldown = laneChangeDebounceDuration;
            }
<<<<<<< Updated upstream
=======
            Vector3 targetDirection = Quaternion.AngleAxis(
                90 * context.ReadValue<float>(), Vector3.up) * movementDirection;
            turnEvent.Invoke(targetDirection);
            Turn(context.ReadValue<float>(), turnPosition.Value);
>>>>>>> Stashed changes
        }

        private Vector3? CheckTurn(float turnValue)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f, turnLayer);
            if (hitColliders.Length != 0)
            {
                Tile tile = hitColliders[0].transform.parent.GetComponent<Tile>();
                TileType type = tile.type;
                if ((type == TileType.LEFT  && turnValue == -1) ||
                    (type == TileType.RIGHT && turnValue ==  1) ||
                    (type == TileType.SIDEWAYS))
                {
                    return tile.pivot.position;
                }
            }
            return null;
        }

        private void Turn(float turnValue, Vector3 turnPosition)
        {
<<<<<<< Updated upstream
            Vector3 tempPlayerPosition = new Vector3(turnPosition.x, transform.position.y, turnPosition.z);

            accumulatedDistance += Vector3.Distance(lastTurnPosition, tempPlayerPosition);

            controller.enabled = false;
            transform.position = tempPlayerPosition;
            controller.enabled = true;

            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90 * turnValue, 0);
            transform.rotation = targetRotation;
            movementDirection = transform.forward.normalized;

            lastTurnPosition = transform.position;
            desiredLane = 1;
=======
            Vector3 tempPlayerPosition = new Vector3(
                turnPosition.x, transform.position.y, turnPosition.z);

            controller.enabled    = false;
            transform.position    = tempPlayerPosition;
            controller.enabled    = true;

            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90 * turnValue, 0);
            transform.rotation    = targetRotation;
            movementDirection     = transform.forward.normalized;

            // NOVO — reset das lanes após virar (o pivot já centrou o player)
            ResetLaneOnTurn();
>>>>>>> Stashed changes
        }

        // ─────────────────────────────────────────────────────────
        //  Slide, Jump, isGrounded, GameOver, OnControllerColliderHit
        //  (todos inalterados)
        // ─────────────────────────────────────────────────────────

        private void PlayerSlide(InputAction.CallbackContext context)
        {
            if (!sliding && isGrounded())
                StartCoroutine(Slide());
        }

        private IEnumerator Slide()
        {
            sliding = true;
            Vector3 originalControllerCenter = controller.center;
            Vector3 newControllerCenter      = originalControllerCenter;
            controller.height    /= 2;
            newControllerCenter.y -= controller.height / 2;
            controller.center    = newControllerCenter;

            animator.Play(slidingAnimationId);
            yield return new WaitForSeconds(slideAnimationClip.length / animator.speed);
<<<<<<< Updated upstream
            controller.height *= 2;
            controller.center = originalControllerCenter;
            sliding = false;
=======

            controller.height  *= 2;
            controller.center   = originalControllerCenter;
            sliding             = false;
>>>>>>> Stashed changes
        }

        private void PlayerJump(InputAction.CallbackContext context)
        {
            if (isGrounded())
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity * -3f);
                controller.Move(playerVelocity * Time.deltaTime);
            }
        }
<<<<<<< Updated upstream
        private void Update()
        {
            if (!isGrounded(20f))
            {
                GameOver();
                return;
            }

            if (laneChangeCooldown > 0f)
            {
                laneChangeCooldown -= Time.deltaTime;
            }

            Vector3 forwardMove = transform.forward * playerSpeed * Time.deltaTime;

            float targetOffset = (desiredLane - 1) * laneDistance;

            float currentOffset = Vector3.Dot(transform.position - lastTurnPosition, transform.right);

            float laneMoveDelta = (targetOffset - currentOffset) * 10f * Time.deltaTime;
            Vector3 sideMove = transform.right * laneMoveDelta;

            Vector3 finalMovement = forwardMove + sideMove + (Vector3.up * playerVelocity.y * Time.deltaTime);

            // Score
            //score += scoreMultiplier * Time.deltaTime;

            score = accumulatedDistance + Vector3.Distance(lastTurnPosition, transform.position);
            scoreUpdateEvent.Invoke((int)score);

            animator.SetBool("isGrounded", isGrounded());

            controller.Move(finalMovement);

            if (isGrounded() && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            playerVelocity.y += gravity * Time.deltaTime;
            //controller.Move(playerVelocity * Time.deltaTime);

            if (playerSpeed < maximumPlayerSpeed)
            {
                playerSpeed += Time.deltaTime * playerSpeedIncreaseRate;
                gravity = initialGravityValue - playerSpeed;

                if (animator.speed < 1.25f)
                {
                    animator.speed += (1 / playerSpeed) * Time.deltaTime;
                }
            }
        }
=======
>>>>>>> Stashed changes

        private bool isGrounded(float length = .2f)
        {
            Vector3 raycastOriginFirst  = transform.position;
            raycastOriginFirst.y       -= controller.height / 2f;
            raycastOriginFirst.y       += .1f;

            Vector3 raycastOriginSecond = raycastOriginFirst;
            raycastOriginFirst         -= transform.forward * .2f;
            raycastOriginSecond        += transform.forward * .2f;

            return Physics.Raycast(raycastOriginFirst,  Vector3.down,
                                   out RaycastHit _,    length, groundLayer)
                || Physics.Raycast(raycastOriginSecond, Vector3.down,
                                   out RaycastHit _,    length, groundLayer);
        }

        private void GameOver()
        {
            Debug.Log("Game Over");
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlayOnce(AudioManager.Instance.Loose);
            }
            gameOverEvent.Invoke((int)score);
            gameObject.SetActive(false);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (((1 << hit.collider.gameObject.layer) & obstacleLayer) != 0)
                GameOver();
        }
    }
}