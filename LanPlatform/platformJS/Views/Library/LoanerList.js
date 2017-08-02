LPAngular.controller("RouteLibraryLoaners", function ($scope) {
    LPInterface.NavSelect("library");

    $scope.Accounts = [];
    $scope.AccountHooks = [];

    $scope.LoadAccounts = function (data) {
        if (data.Data != null) {
            var accountCount = data.Data.length;

            for (var x = 0; x < accountCount; x++) {
                $scope.Accounts[x] = new LPApps.LoanerAccount();

                // Load model into object
                $scope.Accounts[x].LoadModel(data.Data[x]);

                // Hook onto accounts
                $scope.AccountHooks[x] = $scope.Accounts[x].OnUpdate.AddHook($scope.UpdateAccountDetails);

                // Update username
                $scope.Accounts[x].GetCheckoutUserName();
            }

            $scope.$apply();
        }
    }

    $scope.UpdateAccountDetails = function(sender, args) {
        $scope.$apply();

        return;
    }

    $scope.$on("$destroy", function () {
        var accountCount = $scope.AccountHooks.length;
        
        for (var x = 0; x < accountCount; x++) {
            $scope.AccountHooks[x].RemoveHook();
        }
    });

    $scope.Checkout = function(account)
    {
        $.post(LanPlatform.ApiPath + "loaners/checkout/" + account.Id, {}, null, "json");

        return;
    }

    $scope.Checkin = function() {
        
    }

    $scope.ShowCheckin = function(account) {
        var show = false;
        
        if (LPAccounts.LocalAccount != null) {
            show = account.CheckoutUser == LPAccounts.LocalAccount.Id;
        }

        return show;
    }

    LPApps.GetLoanerAccounts($scope.LoadAccounts);
});