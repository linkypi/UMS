cordova.define('cordova/plugin_list', function (require, exports, module) {
    module.exports = [
        {
            "file": "plugins/org.apache.cordova.dialogs/www/notification.js",
            "id": "org.apache.cordova.dialogs.notification",
            "merges": [
                "navigator.notification"
            ]
        },
        {
            "file": "plugins/org.apache.cordova.dialogs/www/android/notification.js",
            "id": "org.apache.cordova.dialogs.notification_android",
            "merges": [
                "navigator.notification"
            ]
        },
        {
            "file": "plugins/com.phonegap.plugins.barcodescanner/www/barcodescanner.js",
            "id": "com.phonegap.plugins.barcodescanner.BarcodeScanner",
            "clobbers": [
                "cordova.plugins.barcodeScanner"
            ]
        }
    ]
});