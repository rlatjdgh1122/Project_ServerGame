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
            // �ڲ� �������ð� �Ǵ� ������ �߻��Ͽ� ������ �����÷� �ٲ���
            // ������ �ڼ����� �𸣰����� �����찡 ���� ��θ� ó���� �� ������ ��ȯ�����ִ� ������ ����
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
