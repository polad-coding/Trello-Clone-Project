using AutoMapper;
using TrelloClone.Core.DTOs;
using TrelloClone.Core.Models;

namespace TrelloClone.WebUI.AutoMapperProfiles
{
    public class ApplicationUserDTOProfile : Profile
    {
        public ApplicationUserDTOProfile()
        {
            CreateMap<ApplicationUserDTO, ApplicationUser>().ForMember(au => au.Id, ce => ce.Ignore());
        }
    }
}
