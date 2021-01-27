using AutoMapper;
using TrelloClone.Core.DTOs;
using TrelloClone.Core.Models;

namespace TrelloClone.WebUI.AutoMapperProfiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
        }
    }
}
