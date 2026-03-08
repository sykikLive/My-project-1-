using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{
    public class AstronautPlayerPlanet : MonoBehaviour
    {
        private Animator anim;
        private CharacterController controller;

        public float speed = 7.0f;
        private Vector3 moveDirection = Vector3.zero;
        public float gravity = 20.0f;
        private float rotationVelocity;
        private float verticalVelocity;
        private float targetAngle;
        public float maxRotationSpeed = 600f;
        public float rotationSmoothTime = 0.05f;
        public float jumpStrength = 100;
        public float alignSpeed = 10f;
        private bool wasGrounded;

        private Vector3 gravityDirection = Vector3.down;
        private Transform currentGravityCenter;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
        }

        public void SetGravitySource(Transform center)
        {
            currentGravityCenter = center;
        }

        public void ClearGravitySource(Transform center)
        {
            if (currentGravityCenter == center)
                currentGravityCenter = null;
        }

        bool IsGrounded()
        {
            if (controller.isGrounded) return true;
            // SphereCast toward planet so grounding works with any orientation
            return Physics.SphereCast(
                transform.position,
                controller.radius * 0.9f,
                gravityDirection,
                out RaycastHit hit,
                controller.height / 2f - controller.radius + 0.2f
            );
        }

        void Update()
        {
            if (currentGravityCenter != null)
                gravityDirection = (currentGravityCenter.position - transform.position).normalized;

            Vector3 planetUp = -gravityDirection;

            bool grounded = IsGrounded();

            float horiz = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");

            Vector3 inputDir = new Vector3(horiz, 0f, vert);
            bool hasInput = inputDir.magnitude > 0.1f;

            if (hasInput)
            {
                Vector3 camRight = Camera.main.transform.right;

                Vector3 movementForward = Vector3.Cross(camRight, planetUp).normalized;

                Vector3 movementRight = Vector3.Cross(planetUp, movementForward).normalized;

                Vector3 moveDir = (movementForward * vert + movementRight * horiz).normalized;

                if (moveDir != Vector3.zero)
                {
                    Quaternion targetRot = Quaternion.LookRotation(moveDir, planetUp);
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation, targetRot,
                        Mathf.Clamp01(Time.deltaTime / rotationSmoothTime)
                    );
                    moveDirection = moveDir * speed;
                }
            }
            else if (grounded)
            {
                moveDirection = Vector3.zero;

                // Standing still keeping feet alligned
                Vector3 currentForward = Vector3.ProjectOnPlane(transform.forward, planetUp).normalized;
                if (currentForward != Vector3.zero)
                {
                    Quaternion alignRot = Quaternion.LookRotation(currentForward, planetUp);
                    transform.rotation = Quaternion.Slerp(transform.rotation, alignRot, alignSpeed * Time.deltaTime);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                verticalVelocity = jumpStrength;
                anim.SetTrigger("Jump");
            }

            if (grounded && verticalVelocity < 0f) verticalVelocity = -2f;

            anim.SetBool("Grounded", grounded);

            if (grounded)
                anim.SetInteger("AnimationPar", hasInput ? 1 : 0);

            verticalVelocity -= gravity * Time.deltaTime;

            // Horizontal movement on planet surface + gravity along planet up/down axis
            Vector3 finalMove = moveDirection + planetUp * verticalVelocity;
            controller.Move(finalMove * Time.deltaTime);

            anim.SetFloat("VerticalVelocity", verticalVelocity);
        }
    }
}
