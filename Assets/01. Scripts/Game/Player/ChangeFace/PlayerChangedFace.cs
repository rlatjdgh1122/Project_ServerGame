using ExtensionMethod.Dictionary;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerChangedFace : ExpansionMonoBehaviour, ISetupHandler, IPlayerStopHandler, IGameFlowHandler
{
	public string SkinName = "";

	private Dictionary<FaceType, Sprite> _typeToSpriteDic = new();
	private ISpriteRenderer2DHandler _sr = null;

	public void Setup(ComponentList list)
	{
		_sr = list.Find<ISpriteRenderer2DHandler>();
	}

	void IGameFlowHandler.OnGameStart()
	{
		List<Sprite> spriteList = ResourceManager.Instance.GetAssetsByLabelName<Sprite>(SkinName);

		foreach (Sprite sprite in spriteList)
		{
			if (Enum.TryParse(sprite.name, out FaceType type))
			{
				_typeToSpriteDic.TryAdd(type, sprite);
			}

		}//end foreach

		Debug_S.Log("¼¼ÆÃ¿Ï");
	}

	void IGameFlowHandler.OnGameEnd()
	{

	}

	public void OnPlayerStart()
	{
		_sr.SetSprite(_typeToSpriteDic.GetValue(FaceType.Default));
	}

	public void OnPlayerStop()
	{
		_sr.SetSprite(_typeToSpriteDic.GetValue(FaceType.Stop));
	}


}
