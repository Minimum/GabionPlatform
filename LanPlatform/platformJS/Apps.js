LPApps = {};

LPApps.Loaners = [];
LPApps.Apps = [];

LPApps.FlagAppEdit = "AppEdit";
LPApps.FlagLoanerEdit = "AppLoanerEdit";
LPApps.FlagLoanerCheckout = "AppLoanerCheckout";

LPApps.Initialize = function (data) {
    if (data.Data.LoanerAccounts != null) {
        var loanerCount = data.Data.LoanerAccounts.length;

        for (var x = 0; x < loanerCount; x++) {
            var loaner = new LPApps.LoanerAccount();

            loaner.LoadModel(data.Data.LoanerAccounts[x]);

            LPApps.AddLoanerAccount(loaner);
        }
    }
    
    if (data.Data.Apps != null) {
        var appCount = data.Data.Apps.length;

        for (var x = 0; x < appCount; x++) {
            var app = new LPApps.App();

            app.LoadModel(data.Data.Apps[x]);

            LPApps.AddApp(app);
        }
    }

    LPApps.OnSteamCodeReceived = new LPEvents.EventHandler();

    return;
}

LPApps.GetApp = function (id, callback) {
    var app = LPApps.Apps[id];

    if (app == null) {
        $.getJSON(LanPlatform.ApiPath + "apps/app/" + id, {}, callback);
    }

    return app;
}

LPApps.AddApp = function (app) {
    var result = LPApps.Apps[app.Id];

    if (result == null) {
        LPApps.Apps[app.Id] = app;

        result = app;
    }
    else if (LPApps.Apps[app.Id].VERSION < app.Version) {
        LPApps.Apps[app.Id].Update(app);
    }

    return result;
}

LPApps.GetLoanerAccounts = function (callback) {
    $.getJSON(LanPlatform.ApiPath + "apps/loaners", {}, callback);

    return;
}

LPApps.GetLoanerAccount = function (id, callback) {
    var account = LPApps.Loaners[id];

    if (account == null) {
        $.getJSON(LanPlatform.ApiPath + "apps/loaner/" + id, {}, callback);
    }

    return account;
}

LPApps.AddLoanerAccount = function (loaner) {
    var result = LPApps.Loaners[loaner.Id];

    if (result == null) {
        LPApps.Loaners[loaner.Id] = loaner;

        result = loaner;
    }
    else if (LPApps.Loaners[loaner.Id].Version < loaner.Version) {
        LPApps.Loaners[loaner.Id].Update(loaner);
    }

    return result;
}