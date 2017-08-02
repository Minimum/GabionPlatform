LPApps.LoanerAccount = function () {
    this.Id = 0;
    this.Version = 0;

    this.Username = "";
    this.Password = "";
    this.SteamCode = "";
    this.CheckoutUser = 0;
    
    this.Apps = [];
    this.AppCount = 0;

    this.CheckoutUserName = "N/A";

    this.OnUpdate = new LPEvents.EventHandler();
}

LPApps.LoanerAccount.prototype.LoadModel = function (model) {
    this.Id = model.Id;
    this.Version = model.Version;

    this.Username = model.Username;
    this.Password = model.Password;
    this.SteamCode = model.SteamCode;
    this.CheckoutUser = model.CheckoutUser;

    this.Apps = model.Apps;
    this.AppCount = model.Apps.length;
}

LPApps.LoanerAccount.prototype.Update = function (loaner) {
    this.Version = loaner.Version;

    this.Username = model.Username;
    this.Password = model.Password;
    this.SteamCode = model.SteamCode;
    this.CheckoutUser = model.CheckoutUser;

    for (var x = 0; x < loaner.AppCount; x++) {
        this.AddApp(loaner.Apps[x]);
    }

    return;
}

LPApps.LoanerAccount.prototype.AddApp = function (app) {
    if (this.GetAppIndex(app) == -1) {
        this.Apps[this.AppCount] = app;

        this.AppCount++;
    }
    
    return;
}

LPApps.LoanerAccount.prototype.GetAppIndex = function (app) {
    var appId = -1;

    for (var x = 0; x < this.AppCount; x++) {
        if (this.Apps[x].Id == app.Id) {
            appId = x;

            break;
        }
    }

    return appId;
}

LPApps.LoanerAccount.prototype.ShowPassword = function () {
    return LPAccounts.LocalAccount != null && LPAccounts.LocalAccount.Id == this.CheckoutUser;
}

LPApps.LoanerAccount.prototype.GetCheckoutUserName = function () {
    var self = this;

    var user = LPAccounts.GetAccount(this.CheckoutUser, function(data) {
        if (data.Data != null) {
            var account = LPAccounts.InitializeAccount(data.Data);

            LPAccounts.AddAccount(account);

            self.CheckoutUserName = account.DisplayName;

            self.OnUpdate.Invoke(this, null);
        }
    });

    if (user != null) {
        this.CheckoutUserName = user.DisplayName;
    }

    return;
}
