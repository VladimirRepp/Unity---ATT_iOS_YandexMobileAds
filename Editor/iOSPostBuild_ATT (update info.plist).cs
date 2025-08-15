using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class iOSPostBuild_ATT
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target != BuildTarget.iOS)
            return;

        // Добавляем описание в Info.plist
        string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        plist.root.SetString("NSUserTrackingUsageDescription",
            "Ваши данные будут использоваться для персонализации рекламы.\nYour data will be used to personalize your ads.");

        plist.WriteToFile(plistPath);
        UnityEngine.Debug.Log("--> Info.plist обновлён");

        return;
    }
}
