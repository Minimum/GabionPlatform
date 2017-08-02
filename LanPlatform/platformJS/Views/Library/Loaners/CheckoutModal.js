LPAngular.controller('ModalLoanerCheckout', function ($uibModalInstance, $scope, account) {
    $scope.RequestStatus = 0;
    $scope.Account = account;
    

    $.post(LanPlatform.ApiPath + "loaners/checkout/" + account.Id, {}, null, "json");

    $scope.FinishLogin = function (data) {
        if (LPAccounts.Auth(data)) {
            $uibModalInstance.dismiss('finish');
        }
        else {
            $scope.RequestStatus = 2;
        }
    }

    $scope.Ok = function () {
        $uibModalInstance.dismiss('accept');
    };

    $scope.Cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
});