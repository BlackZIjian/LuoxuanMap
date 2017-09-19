using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFormId {
    private static string mPath = "Assets/Prefabs/UI/";
    private static string mLoginForm = "LoginForm";
    public static string LoginForm
    {
        get
        {
            return mPath + mLoginForm + ".prefab";
        }
    }
}
public class UIGroupId
{
    public static string LoginGroup = "LoginGroup";
}
