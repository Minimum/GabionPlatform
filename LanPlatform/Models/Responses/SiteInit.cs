using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GabionPlatform.Accounts;
using GabionPlatform.Apps;
using GabionPlatform.DTO.Accounts;
using GabionPlatform.Engine;
using Newtonsoft.Json;

namespace GabionPlatform.Models.Responses
{
    public class SiteInit
    {
        [JsonIgnore]
        protected AppInstance Instance;

        // Local Account
        public UserAccountDto LocalUserAccount { get; set; }
        public List<UserPermission> LocalPermissions { get; set; }

        // Accounts
        public List<UserAccountDto> ActiveUsers { get; set; } 

        // Loaners
        public List<LoanerAccount> LoanerAccounts { get; set; }

        // Apps
        public List<App> Apps { get; set; }

        public SiteInit(AppInstance instance)
        {
            Instance = instance;
        }

        public void LoadData()
        {
            // Local Account
            LocalUserAccount = Instance.LocalAccount != null ? new UserAccountDto(Instance.LocalAccount) : null;
            LocalPermissions = LocalUserAccount != null ? Instance.Accounts.GetAllAccountFlags(Instance.LocalAccount) : new List<UserPermission>();

            ActiveUsers = UserAccountDto.ConvertList(Instance.Accounts.GetActiveAccounts(EngineUtil.CurrentTime - 1800, 100, 0));

            AppManager apps = new AppManager(Instance);

            LoanerAccounts = apps.GetLoanerAccounts();

            Apps = apps.GetApps();

            return;
        }
    }
}