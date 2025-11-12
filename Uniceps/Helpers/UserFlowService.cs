using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Stores;

namespace Uniceps.Helpers
{
    public class UserFlowService
    {
        private readonly UserContextValidator _validator;
        private readonly AccountStore _accountStore;

        public UserFlowService(UserContextValidator validator, AccountStore accountStore)
        {
            _validator = validator;
            _accountStore = accountStore;
        }

        public async Task RefreshUserContextAsync()
        {
            var result = await _validator.EvaluateStageAsync();
            _accountStore.UserContext = result;
            _accountStore.BusinessId=_validator.GetId();
        }
    }
}
