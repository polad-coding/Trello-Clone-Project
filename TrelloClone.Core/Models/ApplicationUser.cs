using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TrelloClone.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Bio { get; set; }
        public string FullName { get; set; }
        public string AvatarColorCode { get; set; }
        public List<TeamUserModel> TeamUserModels { get; set; }

    }
}
