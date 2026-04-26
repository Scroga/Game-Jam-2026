using UnityEngine;
using UnityEngine.InputSystem;

namespace WeaponSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovementScript : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private GameObject playerObject;
        [SerializeField] private GameObject playerAttributes;
        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private float playerSpeed = 3;
        [SerializeField] private float maxRotateSpeed = 360;
        public bool useSprinting = true;
        public string ItemTag = "Item";
        private Rigidbody2D rb;


        private Vector2 playerMoveAxis;
        private bool playerSprinting;
        private bool playerAiming;
        private float angle;
        private float currentRotationVelocity;
        [HideInInspector] public float sprintMultiplier = 2;

        [Header("Camera Settings")]
        public bool useCameraFollowScript = true;
        public bool useAiming = true;
        [HideInInspector] public CameraFollowScript cameraFollowScript;
        [HideInInspector] public float aimMultiplier = 2;

        [Header("Gun Settings")]
        [SerializeField] private GunScript gunScript;
        private bool shootingGun;
        public bool overrideGunScriptGunObject;
        [HideInInspector] public GunObject gunObjectOverride;

        //Override the selected gunObject if applicable
        private void Awake()
        {
            if (gunScript != null && overrideGunScriptGunObject == true && gunObjectOverride != null)
            {
                gunScript.gunObject = gunObjectOverride;
            }
        }

        //Check values and assign default values if required
        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            if (gunScript == null)
            {
                Debug.LogWarning("PlayerMovementScript did not have an assigned gunScript. Attempting to find suitable replacement.");
                gunScript = FindFirstObjectByType<GunScript>();
            }

            if (useCameraFollowScript == true && cameraFollowScript == null)
            {
                Debug.LogWarning("PlayerMovementScript did not have an assigned cameraFollowScript. Attempting to find suitable replacement.");
                cameraFollowScript = FindFirstObjectByType<CameraFollowScript>();
            }
        }

        private void Update()
        {
            //playerObject.transform.Translate(playerSpeed * Time.deltaTime * (Vector2)playerMoveAxis);
            PlayerRotation();
        }

        private void FixedUpdate()
        {
            //Player Movement
            //playerObject.transform.Translate(playerSpeed * Time.deltaTime * (Vector3)playerMoveAxis);

            rb.MovePosition(rb.position + playerSpeed * (Vector2)playerMoveAxis * Time.fixedDeltaTime);
        }

        //Handles the player rotation
        private void PlayerRotation()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 direction = mousePosition - transform.position;
            float targetAngle = Vector2.SignedAngle(Vector2.right, direction);
            //angle = Mathf.SmoothDampAngle(angle, targetAngle, ref currentRotationVelocity, 0.3f, maxRotateSpeed);
            angle = targetAngle;
            playerAttributes.transform.eulerAngles = new Vector3(0, 0, angle);

            playerSprite.flipY = playerAttributes.transform.right.x < 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Item item))
            {
                if (other.TryGetComponent(out HealItem healScript))
                {
                    if (gameObject.TryGetComponent(out HealthScript healthScript))
                    {
                        if (!healthScript.IsFull())
                        {
                            healScript.ApplyHeal(healthScript);
                            Destroy(other.gameObject);
                        }
                    }
                }
                else if (other.TryGetComponent(out MogItem mogScript))
                {
                    if (gameObject.TryGetComponent(out ExplosionAbility abilityScript))
                    {
                        if (!abilityScript.IsFull())
                        {
                            mogScript.ApplyMog(abilityScript);
                            Destroy(other.gameObject);
                        }
                    }
                }
            }
        }

        private void OnMove(InputValue value)
        {
            playerMoveAxis = value.Get<Vector2>();
        }

        //Increases the player speed & zooms camera out
        private void OnSprint()
        {
            if (useSprinting == true)
            {
                playerSprinting = !playerSprinting;

                if (playerSprinting)
                {
                    playerSpeed *= sprintMultiplier;
                }
                else
                {
                    playerSpeed /= sprintMultiplier;
                }

                if (cameraFollowScript != null)
                {
                    cameraFollowScript.SetPlayerSprintingCamSize(playerSprinting);
                }
            }
        }

        //Slows the player speed & zooms camera in
        private void OnAim()
        {
            if (useAiming == true)
            {
                playerAiming = !playerAiming;

                if (playerAiming)
                {
                    playerSpeed /= aimMultiplier;
                }
                else
                {
                    playerSpeed *= aimMultiplier;
                }

                if (cameraFollowScript != null)
                {
                    cameraFollowScript.SetPlayerZoomingCam(gunScript);
                }
            }
        }

        //Calls the trigger on the weapon
        private void OnShoot()
        {
            shootingGun = !shootingGun;

            gunScript.TriggerWeapon(shootingGun);
        }

        //Used for manual reloads
        private void OnReload()
        {
            gunScript.ManualReload();
        }
    }
}