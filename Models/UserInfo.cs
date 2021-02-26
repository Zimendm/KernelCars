using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KernelCars.Models
{
    public class UserInfo
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public UserInfo(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> GetUserName()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user?.Identity?.Name ?? "Car man"; ;
        }

        //public string GetUserName()
        //{
        //    var authState = authenticationStateProvider.GetAuthenticationState();
        //    var user = authState.User;
        //    user
        //    return user?.Identity?.Name ?? "Pitza man";
        //}
    }
}
