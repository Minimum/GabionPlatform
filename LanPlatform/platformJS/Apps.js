LPApps = {};

LPApps.Loaners = [];
LPApps.Apps = [];

LPApps.Initialize = function (data) {
    if (data.Data.LoanerAccounts != null) {
        LPApps.Loaners = data.Data.LoanerAccounts;
    }
    
    if (data.Data.Apps != null) {
        LPApps.Apps = data.Data.Apps;
    }

    LPApps.OnSteamCodeReceived = new LPEvents.EventHandler();

    return;
}

LPApps.GetLoanerAccounts = function (callback) {
    var loaners = null;

    if (LPApps.Loaners.length > 0) {
        loaners = LPApps.Loaners;
    } else {
        $.getJSON(LanPlatform.ApiPath + "loaners/all", {}, callback);
    }

    return loaners;
}

LPApps.AddLoanerAccount = function (loaner) {
    if (LPApps.Loaners[loaner.Id] == null) {
        LPApps.Loaners[loaner.Id] = loaner;
    }
    else if (LPApps.Loaners[loaner.Id].Version != loaner.Version) {
        LPApps.Loaners[loaner.Id].Update(loaner);
    }

    return;
}

