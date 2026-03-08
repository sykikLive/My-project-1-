using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


namespace AstronautPlayer
{

	public class AstronautPlayer : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;

		public float speed = 600.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;
		private float rotationVelocity;
		private float verticalVelocity;
		private float targetAngle;
		public float maxRotationSpeed = 600f;
		public float rotationSmoothTime = 0.05f;
		public float jumpStrength = 100;
		private bool wasGrounded;
		private bool paused = false;

		public void Bounce(float force)
        {
            verticalVelocity = force;
        }

		void Start () {

			controller = GetComponent<CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();

		}

		void PauseGame()
		{
			SceneManager.LoadScene("pauseMenu", LoadSceneMode.Additive);
			Time.timeScale = 0f;
			paused = true;
		}

		void ResumeGame()
		{
			SceneManager.UnloadSceneAsync("pauseMenu");
			Time.timeScale = 1f;
			paused = false;
		}

		void Update (){

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (paused) ResumeGame();
				else PauseGame();
			}

			
			float horiz = Input.GetAxis("Horizontal");
			float vert = Input.GetAxis("Vertical");

			Vector3 inputDir = new Vector3(horiz, 0f, vert);
			bool hasInput = inputDir.magnitude > 0.1f;

			if (hasInput)
			{
				targetAngle = Mathf.Atan2(horiz, vert) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
				float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime, maxRotationSpeed);
				transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
				moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * speed;
			} else if (controller.isGrounded)
			{
				moveDirection = Vector3.zero;
			}

			if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded) 
			{
				verticalVelocity = jumpStrength;
				anim.SetTrigger("Jump");
			}

			if (controller.isGrounded && verticalVelocity < 0f) verticalVelocity = -2f;

			anim.SetBool("Grounded", controller.isGrounded);

			if (controller.isGrounded) 
			{
				anim.SetInteger("AnimationPar", hasInput ? 1 : 0);
			}

			verticalVelocity -= gravity * Time.deltaTime;
			moveDirection.y = verticalVelocity;
			controller.Move(moveDirection * Time.deltaTime);
			anim.SetFloat("VerticalVelocity", verticalVelocity);
			

		}

	}
}
