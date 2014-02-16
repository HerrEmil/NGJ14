using UnityEngine;
using System.Collections.Generic;

public static class GameResult
{

	public static GameController result
	{
		get
		{
			if (_result == null)
			{
				_result = new GameController();
				_result.collectedPickups = new List<GameObject>();
				_result.extrasTotalCount = 0;
			}
			return _result;
		}
		set { _result = value; }
	}

	private static GameController _result;

}
