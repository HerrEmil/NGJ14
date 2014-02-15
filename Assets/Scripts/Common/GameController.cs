using UnityEngine;
using System.Collections.Generic;
using ModestTree.Zenject;

public class GameController : IEntryPoint, ITickable
{
	public IList<GameObject> collectedPickups;

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
