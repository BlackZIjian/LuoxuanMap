
public class SceneInfo {

    private static string mPath = "Assets/Scenes/";
    public static string GetScene(int x,int y)
    {
        return mPath + "Scene" + x.ToString() + y.ToString() + ".unity";
    }
    private static string mVoxelandScene = "VoxelandScene";
    public static string VoxelandScene
    {
        get
        {
            return mPath + mVoxelandScene + ".unity";
        }
    }
}
