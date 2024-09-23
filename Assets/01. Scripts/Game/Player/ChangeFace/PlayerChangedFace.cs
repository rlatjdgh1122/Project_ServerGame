using ExtensionMethod.Dictionary;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerChangedFace : ExpansionMonoBehaviour, ISetupHandler, IPlayerStopHandler
{
    public string SkinName = "";

    private readonly string _path = "D:/GitHub/Project_ServerGame/Assets/02. Sprites/Game/Player/Face";
    private Dictionary<FaceType, Sprite> _typeToSpriteDic = new();
    private ISpriteRenderer2DHandler _sr = null;

    public void Setup(ComponentList list)
    {
        _sr = list.Find<ISpriteRenderer2DHandler>();

        string[] paths = System.IO.Directory.GetFiles($"{_path}/{SkinName}", "*.png");

        for (int i = 0; i < paths.Length; ++i)
        {
            // 자꾸 역슬래시가 되는 문제가 발생하여 강제로 슬래시로 바꿔줌
            // 원인은 자세히는 모르겠으나 윈도우가 파일 경로를 처리할 때 강제로 변환시켜주는 것으로 보임
            string correctedPath = paths[i].Replace(Application.dataPath, "Assets").Replace("\\", "/");
            Sprite img = (Sprite)AssetDatabase.LoadAssetAtPath(correctedPath, typeof(Sprite));

            _typeToSpriteDic.TryAdd((FaceType)i, img);

        } //end for

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
