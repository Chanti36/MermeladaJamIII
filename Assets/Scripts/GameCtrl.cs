using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
	int gameScene;

	private TextCtrl textCtrl;

	[SerializeField] private GameObject titleScene;
	[SerializeField] private GameObject textScene;
	[SerializeField] private GameObject endingScene;
	[SerializeField] private GameObject creditsScene;


	[SerializeField] private PlayerCtrl player;
	[SerializeField] private EnemyCtrl enemy;
	[SerializeField] private Animator adviceAnim;
	private bool b_adviced;

	string[] introTxtESP =
	{
		"Parece que todo se ha acabado",
		"Mis pobres vacas, esparcidas por el espacio",
		"Debería intentar recogerlas"
	};

	string[] introTxtENG =
	{
		"It looks like it's all over",
		"My poor cows, scattered through space",
		"I should try to collect them"
	};

	string[] plotTwistTxtESP =
	{
		"Ese maldito lunático",
		"Nadie pensó que realmente compraría el mundo",
		"¿Y para qué?, Para echarnos a todos y exhibirlo en su maldia colección",
		"Espera un momento",
		"¿Que está haciendo ahí?",
		"Se va a enterar ese desgraciado"
	};

	string[] plotTwistTxtENG =
	{
		"That damn lunatic",
		"No one thought it would actually buy the world",
		"And for what, to throw us all out and display it in his damn collection?",
		"Wait a moment",
		"What is he doing there?",
		"That bastard is going to find out"
	};

	//intro fade
	public Image introFade;
	private float introLerp = 0;
	private float outroLerp = 0;


	float playerlerptimer = 0;
	float playerstartangle;

	public Sprite winSpr, loseSpr;

	public bool b_languajeESP;
	public Text txt_start;

	private void Awake()
	{
		b_languajeESP = false;
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 30;

		textCtrl = GetComponent<TextCtrl>();

		gameScene = 0;

		titleScene.SetActive(true);
		textScene.SetActive(false);
		endingScene.SetActive(false);
		creditsScene.SetActive(false);

		b_adviced = false;
	}

	public void LanguajeESP()
	{
		b_languajeESP = true;
		txt_start.text = "Elige el idioma y \n pulsa espacio para empezar \n Controles: flechas para moverse";
	}

	public void LanguajeENG()
	{
		b_languajeESP = false;
		txt_start.text = "Select Languaje\r\nAnd press space to start \n Controls: arrows to move";
	}

	private void Update()
	{
		if (gameScene == 0) //TITLE
		{
			if (Input.GetKeyUp(KeyCode.Space))
			{
				titleScene.SetActive(false);
				textScene.SetActive(true);
				gameScene = 1;
				if (b_languajeESP)
					textCtrl.StartText(introTxtESP, false);
				else
					textCtrl.StartText(introTxtENG, false);
			}
		}
		if (gameScene == 1) //INTRO
		{
			if( introLerp < 4)
			{
				introLerp += Time.deltaTime;
				introFade.color = new Color(introFade.color.r, introFade.color.g, introFade.color.b, Mathf.Lerp(1, 0, introLerp / 4));
			}

			if (textCtrl.b_onText == false && Input.anyKey)
			{
				textScene.SetActive(false);
				gameScene = 2;
				player.b_playing = true;
				GetComponent<GameplayCtrl>().i_gameState = 1;
			}
		}
		if (gameScene == 2) //GAME PART 1
		{
			if (GetComponent<GameplayCtrl>().i_cowCount > 5 && GetComponent<GameplayCtrl>().ScenaryEmpty())
			{
				if (Input.anyKeyDown)
				{
					gameScene = 3;
					player.StopMove();
					player.b_playing = false;

					textScene.SetActive(true);
					if (b_languajeESP)
						textCtrl.StartText(plotTwistTxtESP, false);
					else
						textCtrl.StartText(plotTwistTxtENG, false);
					playerstartangle = player.spriteTr.localEulerAngles.z;
				}
			}
		}
		if (gameScene == 3) //PLOT TWIST
		{
			if (textCtrl.b_onText && textCtrl.i_phraseIndex == 2)
				enemy.Show();
			if (textCtrl.b_onText == false && !b_adviced)
			{
				b_adviced = true;
				adviceAnim.SetTrigger("advice");
			}
			if (textCtrl.b_onText == false )
			{
				textScene.SetActive(false);
				gameScene = 4;
				player.b_playing = true;
				GetComponent<GameplayCtrl>().i_gameState = 2;
				player.i_gameState = 2;
				enemy.StartFight();
			}
			if (playerlerptimer < 1)
			{
				playerlerptimer += Time.deltaTime;
				player.spriteTr.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(playerstartangle, 0, playerlerptimer));
			}
			else
				player.spriteTr.localEulerAngles = new Vector3(0, 0, 0);
		}
		if (gameScene == 4) //GAME PART 2
		{
			if (enemy.Hits() > 5)
				GetComponent<GameplayCtrl>().i_ending = 2;

			if (GetComponent<GameplayCtrl>().i_ending != 0)
			{
				if (GetComponent<GameplayCtrl>().i_ending == 1)
					endingScene.GetComponent<Image>().sprite = loseSpr;
				else
					endingScene.GetComponent<Image>().sprite = winSpr;
				gameScene = 5;
				enemy.GetComponent<BoxCollider2D>().enabled = false;	
			}
		}
		if (gameScene == 5) //ENDING
		{
			if (outroLerp < 2)
			{
				outroLerp += Time.deltaTime * 2;
				introFade.color = new Color(introFade.color.r, introFade.color.g, introFade.color.b, Mathf.Lerp(0, 1, outroLerp / 2));
			}
			else
			{
				if (outroLerp < 4)
				{
					endingScene.SetActive(true);
					introFade.gameObject.SetActive(false);

					if (Input.anyKeyDown)
						outroLerp = 20;
				}
				else
				{
					gameScene = 6;
					creditsScene.SetActive(true);
					endingScene.SetActive(false);
				}
			}
		}
		if (gameScene == 6) //CREDITS
		{
			if (Input.GetKeyUp(KeyCode.R))
				SceneManager.LoadScene(0);
		}
	}
}

//TODO LIST
/*txts
 * 2021.3.16
 * ajustar la aparicion del enemigo a su frase AQUI conel txt index

 */