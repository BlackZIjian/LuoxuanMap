using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo {
    private static string mPath = "Assets/Prefabs/Entity/Character/";
    private static string mLocalPlayer = "LocalPlayer";
    public static string LocalPlayer
    {
        get { return mPath + mLocalPlayer + ".prefab"; }
    }
}
public class EntityGroupInfo
{
    public static string Player = "Player";
}
