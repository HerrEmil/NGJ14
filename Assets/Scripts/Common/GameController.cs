using UnityEngine;
using System.Collections;
using ModestTree.Zenject;

public class GameController : IEntryPoint, ITickable
{

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
	    Screen.showCursor = false;
	    Debug.Log("Started Game");
	}

	public void Tick()
	{
	}

	void StartGame()
	{
	}
}
