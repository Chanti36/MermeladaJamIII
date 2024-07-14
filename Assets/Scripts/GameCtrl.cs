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

	string[] introTxt = 
	{
		"intro",
		"parece que todo se ha acabado",
		"o no?"
	};

	string[] plotTwistTxt =
	{
		"plot twist",
		"un millonario habia comprado el mundo" ,
		"os ha hechado a todos",
		"que hace ahí?¿?¿¿¿?"
	};

	//intro fade
	public Image introFade;
	private float introLerp = 0;
	private float outroLerp = 0;

	private void Awake()
	{
		textCtrl = GetComponent<TextCtrl>();

		gameScene = 0;

		titleScene.SetActive(true);
		textScene.SetActive(false);
		endingScene.SetActive(false);
		creditsScene.SetActive(false);

		b_adviced = false;
	}

	private void Update()
	{
		if (gameScene == 0) //TITLE
		{
			if (Input.anyKeyDown)
			{
				titleScene.SetActive(false);
				textScene.SetActive(true);
				gameScene = 1;
				textCtrl.StartText(introTxt, false);
			}
		}
		if (gameScene == 1) //INTRO
		{
			if( introLerp < 3)
			{
				introLerp += Time.deltaTime;
				introFade.color = new Color(introFade.color.r, introFade.color.g, introFade.color.b, Mathf.Lerp(.6f, 0, introLerp / 3));
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
					textCtrl.StartText(plotTwistTxt, false);
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
		}
		if (gameScene == 4) //GAME PART 2
		{
			if (enemy.Hits() > 5)
				GetComponent<GameplayCtrl>().i_ending = 2;

			if (GetComponent<GameplayCtrl>().i_ending != 0)
			{
				gameScene = 5;
				//kek
			}
		}
		if (gameScene == 5) //ENDING
		{
			outroLerp += Time.deltaTime;
			if (outroLerp < 2)
			{
				introFade.color = new Color(introFade.color.r, introFade.color.g, introFade.color.b, Mathf.Lerp(0, 1, outroLerp / 2));
			}
			else
			{
				endingScene.SetActive(true);
				introFade.gameObject.SetActive(false);

				if (outroLerp > 3 )
				{
					gameScene = 6;
					creditsScene.SetActive(true);
					endingScene.SetActive(false);
				}
			}
			print("endinggggg");
		}
		if (gameScene == 6) //CREDITS
		{
			if (Input.GetKeyUp(KeyCode.R))
				SceneManager.LoadScene(0);
		}
	}
}


//TODO LIST
/*
 * 2021.3.16
 * ajustar la aparicion del enemigo a su frase AQUI conel txt index
 * back to recoger 6 vacas en gamepart1 AQUI Y EN GAMEPLAY DONDE DEEJEN DE APARECER
 * cowsprite when spawning both in gameplay and player
 * enemy pivot has the script so do the hitbox
 */