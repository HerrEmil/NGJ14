using UnityEngine;
using System.Collections.Generic;
using ModestTree.Zenject;
using UnityEngine.SceneManagement;

public class GameController : IEntryPoint, ITickable
{
	public IList<GameObject> collectedPickups;
	public bool isFinished;
	public bool gameover;
	public int extrasTotalCount;
	public readonly bool isMobile =
	#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
		true;
	#else
		false;
	#endif
	
	public int TickPriority
	{
	    get { return 0; }
	}

	public int InitPriority
	{
			get { return 0; }
	}

	public GameController()
	{
	}

	public void Initialize()
	{
		collectedPickups = new List<GameObject>();
		isFinished = false;
		Cursor.visible = false;
		GameResult.result = this;
		extrasTotalCount = 5;
	}

	public void Tick()
	{
		if (gameover)
		{
			SceneManager.LoadScene("Lose");
		}
		else if (isFinished)
		{
			SceneManager.LoadScene("Win");
		}
	}

	void StartGame()
	{
	}



}
