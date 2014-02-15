using UnityEngine;
using System.Collections.Generic;
using ModestTree.Zenject;

public class GameController : IEntryPoint, ITickable
{
	public IList<GameObject> collectedPickups;
	public bool isFinished;
	public bool gameover;

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
		Screen.showCursor = false;
	    Debug.Log("Started Game");
	}

	public void Tick()
	{
		if (gameover)
		{
			Application.LoadLevel("Lose");
		}
		else if (isFinished)
		{
			Application.LoadLevel("Win");
		}
	}

	void StartGame()
	{
	}



}
