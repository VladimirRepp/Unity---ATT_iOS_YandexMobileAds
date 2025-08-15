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

        // 1) Добавляем описание в Info.plist
        string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        plist.root.SetString("NSUserTrackingUsageDescription",
            "Ваши данные будут использоваться для персонализации рекламы.");

        plist.WriteToFile(plistPath);

        // 2) Путь к UnityAppController.mm
        string appControllerPath = Path.Combine(pathToBuiltProject, "Classes/UnityAppController.mm");
        string fileContent = File.ReadAllText(appControllerPath);

        // Добавляем import, если нет
        if (!fileContent.Contains("<AppTrackingTransparency/AppTrackingTransparency.h>"))
        {
            fileContent = fileContent.Replace(
                "#import \"UnityAppController.h\"",
                "#import \"UnityAppController.h\"\n#import <AppTrackingTransparency/AppTrackingTransparency.h>\n#import <AdSupport/AdSupport.h>"
            );
        }

        // Код ATT
        string attCode = @"
    if (@available(iOS 14, *)) {
        [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
            NSLog(@""Tracking status: %ld"", (long)status);
            UnitySendMessage(""ATTManager"", ""OnTrackingAuthorizationComplete"", [[NSString stringWithFormat:@""%ld"", (long)status] UTF8String]);
        }];
    } else {
        UnitySendMessage(""ATTManager"", ""OnTrackingAuthorizationComplete"", ""-1"");
    }";

        string methodSignature = "- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions";
        if (fileContent.Contains(methodSignature))
        {
            int methodStartIndex = fileContent.IndexOf(methodSignature);

            // Ищем return YES; только после начала этого метода
            int returnIndex = fileContent.IndexOf("return YES;", methodStartIndex);

            if (returnIndex != -1 && !fileContent.Contains("requestTrackingAuthorizationWithCompletionHandler"))
            {
                fileContent = fileContent.Insert(returnIndex, attCode + "\n");
            }
        }

        File.WriteAllText(appControllerPath, fileContent);

        UnityEngine.Debug.Log("--> iOS PostBuild: ATT код добавлен (авто), Info.plist обновлён, UnitySendMessage настроен");
    }
}
