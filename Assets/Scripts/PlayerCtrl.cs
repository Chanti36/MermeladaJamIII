using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public int i_gameState = 0;

	[SerializeField] private Animator consequences;
	[SerializeField] private GameplayCtrl gameplayCtrl;
	public bool b_playing;

	private float ySpeed;
	private float xSpeed;
	public int i_spd;
	Rigidbody2D rb;
	private float f_worldHeight, f_worldWidth;

	int cowCount;

	[SerializeField] private GameObject cow;
	private GameObject[] cowPool;
	[SerializeField] private Sprite[] cowSprites;

	private float shootDelay;
	private float shootTimer;

	private float startAngle = 0;
	private float endAngle = 0;
	private float angleLerpTime = 0;
	private float angleLerpTimer = 0;

	public Transform spriteTr;

	public AudioClip[] cowSounds;
	public AudioClip[] hitSounds;

	public AudioSource cowSound;
	public AudioSource hitSound;


	void Start()
    {
		f_worldHeight = Camera.main.orthographicSize;
		f_worldWidth = f_worldHeight * ((float)Screen.width / Screen.height);

		i_gameState = 1;
		rb = GetComponent<Rigidbody2D>();
		b_playing = false;
		cowCount = 0;

		cowPool = new GameObject[30];
		for (int i = 0; i < 30; i++)
		{
			GameObject prefab = Instantiate(cow);
			cowPool[i] = prefab;
			prefab.GetComponent<Cow>().b_shoot = true;
			prefab.GetComponent<SpriteRenderer>().flipX = true;
			prefab.SetActive(false);

		}
		
		shootDelay = 1;
		shootTimer = 0;
	}

	GameObject GetCow()
	{
		for (int i = 0; i < 30; i++)
		{
			if (!cowPool[i].activeInHierarchy)
				return (cowPool[i]);
		}
		return (null);
	}

    void Update()
    {
		if (i_gameState == 1)
			Mode1();
		else
			Mode2();

		if (angleLerpTimer < angleLerpTime)
		{
			angleLerpTimer += Time.deltaTime;
			spriteTr.eulerAngles = new Vector3(0, 0, Mathf.Lerp(startAngle, endAngle, angleLerpTimer/ angleLerpTime));
//			print("works	" + spriteTr.eulerAngles + " " + Mathf.Lerp(startAngle, endAngle, angleLerpTimer));
		}


		if (transform.position.y > f_worldHeight - GetComponent<SpriteRenderer>().size.y)
			transform.position = new Vector3(transform.position.x, f_worldHeight - GetComponent<SpriteRenderer>().size.y, 0);
		else if (transform.position.y < -f_worldHeight + GetComponent<SpriteRenderer>().size.y)
			transform.position = new Vector3(transform.position.x, -f_worldHeight + GetComponent<SpriteRenderer>().size.y, 0);
	}

	void Mode1()
	{
		if (!b_playing)
			return;
		
		if (Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow) ||
			Input.GetKeyDown(KeyCode.LeftArrow) && !Input.GetKeyDown(KeyCode.RightArrow) ||
			Input.GetKeyDown(KeyCode.UpArrow) && !Input.GetKeyDown(KeyCode.DownArrow) ||
			Input.GetKeyDown(KeyCode.DownArrow) && !Input.GetKeyDown(KeyCode.UpArrow))
		{
			startAngle = spriteTr.eulerAngles.z;
			endAngle = Random.Range(0, 360);
			angleLerpTimer = 0;
			angleLerpTime = Random.Range(0.5f, 2.0f);
		}


			if (Input.GetKey(KeyCode.RightArrow) &&
			!Input.GetKey(KeyCode.LeftArrow))
		{
			if (xSpeed < 1)
				xSpeed += Time.deltaTime;
			else
				xSpeed = 1;
		}
		else
		{
			if (xSpeed > -.01f && xSpeed < .01f)
				xSpeed = 0;
			else if (xSpeed > 0)
				xSpeed -= Time.deltaTime;
			else if (xSpeed < 0)
				xSpeed += Time.deltaTime;
		}

		if (!Input.GetKey(KeyCode.UpArrow) &&
			Input.GetKey(KeyCode.DownArrow))
		{
			ySpeed = -1;
		}
		else if (Input.GetKey(KeyCode.UpArrow) &&
				!Input.GetKey(KeyCode.DownArrow))
		{
			ySpeed = 1;
		}
		else
		{
			if (ySpeed > -.01f && ySpeed < .01f)
				ySpeed = 0;
			else if (ySpeed > 0)
				ySpeed -= Time.deltaTime;
			else if (ySpeed < 0)
				ySpeed += Time.deltaTime;
		}
		rb.velocity = new Vector2(0, ySpeed) * i_spd;
		//print(rb.velocity);
	}

	void Mode2()
	{
		if (!b_playing)
			return;

		shootTimer += Time.deltaTime;

		if (Input.GetKey(KeyCode.RightArrow) &&
			!Input.GetKey(KeyCode.LeftArrow))
		{
			if (shootTimer >= shootDelay)
			{
				GameObject newCow = GetCow();
				if (newCow)
				{
					newCow.GetComponent<SpriteRenderer>().sprite = cowSprites[Random.Range(0, cowSprites.Length)];
					newCow.transform.position = transform.position + new Vector3(GetComponent<SpriteRenderer>().size.x, 0, 0);
					newCow.SetActive(true);
				}
				shootTimer = 0;			
			}
		}
		else if (!Input.GetKey(KeyCode.RightArrow) &&
			Input.GetKey(KeyCode.LeftArrow))
		{
			if (xSpeed > -0.3f)
				xSpeed -= Time.deltaTime;
			else
				xSpeed = -0.3f;
		}
		else
		{
			if (xSpeed > -.01f && xSpeed < .01f)
				xSpeed = 0;
			else if (xSpeed > 0)
				xSpeed -= Time.deltaTime * 2;
			else if (xSpeed < 0)
				xSpeed += Time.deltaTime * 2;
		}

		if (!Input.GetKey(KeyCode.UpArrow) &&
			Input.GetKey(KeyCode.DownArrow))
		{
			ySpeed = -1;
		}
		else if (Input.GetKey(KeyCode.UpArrow) &&
				!Input.GetKey(KeyCode.DownArrow))
		{
			ySpeed = 1;
		}
		else
		{
			if (ySpeed > -.01f && ySpeed < .01f)
				ySpeed = 0;
			else if (ySpeed > 0)
				ySpeed -= Time.deltaTime * 3;
			else if (ySpeed < 0)
				ySpeed += Time.deltaTime * 3;
		}
		rb.velocity = new Vector2(xSpeed, ySpeed) * i_spd;

		//print(rb.velocity);
	}

	public float GetXSpeed()
	{
		return (xSpeed + (Mathf.Abs(ySpeed)*.5f));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "cow")
		{
			cowSound.clip = cowSounds[Random.Range(0, cowSounds.Length)];
			cowSound.Play();

			collision.gameObject.SetActive(false);
			gameplayCtrl.i_cowCount++;
			cowCount++;
		}
		else if (collision.gameObject.tag == "asteroid")
		{
			hitSound.clip = hitSounds[Random.Range(0, hitSounds.Length)];
			hitSound.Play();

			collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
			Camera.main.GetComponent<CameraCtrl>().DoShake(.2f, 1);
			xSpeed = 0;
			ySpeed = 0;
			
			startAngle = spriteTr.eulerAngles.z;
			endAngle = Random.Range(0, 360);
			angleLerpTimer = 0;
			angleLerpTime = Random.Range(1.0f, 2.0f);		
		}
	}

	public void StopMove()
	{
		rb.velocity = Vector2.zero;
		xSpeed = 0;
		ySpeed = 0;
	}
}
