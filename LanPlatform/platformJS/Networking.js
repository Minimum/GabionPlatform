/*
*   Networking
*/
var LPNet = {};

LPNet.MessageTypes = [];

//LPNet.RESPONSE_OUTOFDATE = -2;
//LPNet.RESPONSE_INVALID = -1;
LPNet.RESPONSE_HANDLED = 0;
LPNet.RESPONSE_ERROR = 1;
LPNet.RESPONSE_NOT_INSTALLED = 2;
LPNet.RESPONSE_DISABLED = 3;
LPNet.RESPONSE_APPERROR = 4;

LPNet.Initialize = function () {

}

LPNet.RegisterMessage = function (name, callback) {
    if (LPNet.MessageTypes[name] == null) {
        LPNet.MessageTypes[name] = new LPEvents.EventHandler();
    }

    return LPNet.MessageTypes[name].AddHook(callback);
}

LPNet.AppResponse = function (data) {
    var response;

    if (data != null && data.AppName != null && data.AppName == "LanPlatform") {
        if (data.AppBuild != null && data.AppBuild == LanPlatform.AppBuild) {
            response = data.AppResponse;
        }
        else {
            response = LPNet.RESPONSE_OUTOFDATE;
        }
    }
    else {
        response = LPNet.RESPONSE_INVALID;
    }

    return response;
}

LPNet.ReadMessages = function () {

}