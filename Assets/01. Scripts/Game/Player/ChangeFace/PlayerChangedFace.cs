using ExtensionMethod.Dictionary;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerChangedFace : ExpansionMonoBehaviour, ISetupHandler, IPlayerStopHandler
{
	public string SkinName = "";

	private Dictionary<FaceType, Sprite> _typeToSpriteDic = new();
	private ISpriteRenderer2DHandler _sr = null;

	public void Setup(ComponentList list)
	{

		_sr = list.Find<ISpriteRenderer2DHandler>();


	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			List<Sprite> spriteList = ResourceManager.Instance.GetAssetsByLabelName<Sprite>(SkinName);
			List<Texture2D> test = ResourceManager.Instance.GetAssetsByLabelName<Texture2D>(SkinName);

			for (int i = 0; i < spriteList.Count; i++)
			{
				_typeToSpriteDic.TryAdd((FaceType)i, spriteList[i]);

			} //end for
		}
		
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
