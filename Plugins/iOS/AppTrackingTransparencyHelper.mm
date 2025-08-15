#import <AppTrackingTransparency/AppTrackingTransparency.h>
#import <AdSupport/AdSupport.h>

extern "C" {
    void RequestTrackingAuthorization() {
        if (@available(iOS 14, *)) {
            [ATTrackingManager requestTrackingAuthorizationWithCompletionHandler:^(ATTrackingManagerAuthorizationStatus status) {
                // Преобразуем статус в строку и передаем в Unity
                NSString *statusStr;
                switch (status) {
                    case ATTrackingManagerAuthorizationStatusAuthorized:
                        statusStr = @"authorized";
                        break;
                    case ATTrackingManagerAuthorizationStatusDenied:
                        statusStr = @"denied";
                        break;
                    case ATTrackingManagerAuthorizationStatusNotDetermined:
                        statusStr = @"not_determined";
                        break;
                    case ATTrackingManagerAuthorizationStatusRestricted:
                        statusStr = @"restricted";
                        break;
                }
                // Отправляем статус в Unity
                UnitySendMessage("ATTManager", "OnTrackingAuthorizationComplete", [statusStr UTF8String]);
            }];
        } else {
            // Для iOS < 14 считаем, что трекинг разрешен (ATT не требуется)
            UnitySendMessage("ATTManager", "OnTrackingAuthorizationComplete", "iOS_less_14");
        }
    }
}
