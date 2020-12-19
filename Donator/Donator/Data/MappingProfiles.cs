using AutoMapper;
using Donator.Dtos.NPO;
using Donator.Dtos.NPOType;
using Donator.Dtos.OrgUser;
using Donator.Dtos.User;
using Donator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region User maps
            CreateMap<User, UserAuthDto>();
            CreateMap<User, UserDetailDto>();
            CreateMap<UserToLoginDto, User>();
            CreateMap<UserToRegisterDto, User>();
            CreateMap<UserToCreateOrgDto, User>();
            #endregion

            #region NPO maps
            CreateMap<NPOtoCreateDto, NPO>();
            CreateMap<NPO, NPODetailDto>()
                    .ForMember(x => x.Type, opts =>
                    {
                        opts.MapFrom(src => src.Type.Name);
                    });
            #endregion

            #region NPO Types
            CreateMap<NPOTypeToCreateDto, NPOType>();
            CreateMap<NPOTypeToUpdateDto, NPOType>();
            #endregion

            #region OrgUsers
            CreateMap<OrgUser, OrgUserForListDto>()
                .ForMember(x => x.Id, opts =>
                {
                    opts.MapFrom(src => src.User.Id);
                })
                .ForMember(x => x.UserName, opts =>
                {
                    opts.MapFrom(src => src.User.UserName);
                });
            CreateMap<OrgUser, ListOfNPOsForOrgUser>()
                .ForMember(x => x.Id, opts =>
                {
                    opts.MapFrom(src => src.NonProfitOrg.Id);
                })
                .ForMember(x => x.Name, opts =>
                {
                    opts.MapFrom(src => src.NonProfitOrg.Name);
                })
                .ForMember(x => x.Type, opts =>
                {
                    opts.MapFrom(src => src.NonProfitOrg.Type.Name);
                });
            #endregion
        }
    }
}
