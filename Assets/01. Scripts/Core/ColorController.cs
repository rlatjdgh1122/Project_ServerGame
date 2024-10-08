using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorController
{
	private static Dictionary<TurnType, Color> _typeToColorDic = new();

	static ColorController()
	{
		_typeToColorDic.Add(TurnType.None, Color.white);
		_typeToColorDic.Add(TurnType.Blue, Color.blue);
		_typeToColorDic.Add(TurnType.Yellow, Color.yellow);
		_typeToColorDic.Add(TurnType.Red, Color.red);
		_typeToColorDic.Add(TurnType.Green, Color.green);
	}

	public static Color GetColorByTurnType(TurnType type)
	{
		return _typeToColorDic[type];
	}
}
