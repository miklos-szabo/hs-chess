using AutoMapper;
using HSC.Dal.Entities;
using HSC.Transfer.Friends;
using HSC.Transfer.Groups;
using HSC.Transfer.Match;
using HSC.Transfer.User;

namespace HSC.Bll.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Dal.Entities.Match, MatchStartDto>()
                .ForMember(x => x.WhiteUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Common.Enums.Color.White).UserName))
                .ForMember(x => x.WhiteRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Common.Enums.Color.White).Rating))
                .ForMember(x => x.BlackUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Common.Enums.Color.Black).UserName))
                .ForMember(x => x.BlackRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Common.Enums.Color.Black).Rating));

            CreateMap<Group, GroupDto>()
                .ForMember(x => x.UserCount, o => o.MapFrom(m => m.Users.Count()));

            CreateMap<Group, GroupDetailsDto>()
                .ForMember(x => x.UserCount, o => o.MapFrom(m => m.Users.Count()))
                .ForMember(x => x.Members, o => o.MapFrom(m => m.Users.Select(u => u.UserName).ToList()));

            CreateMap<FriendRequest, FriendRequestDto>();

            CreateMap<User, FriendDto>();
            CreateMap<User, UserMenuDto>();
            CreateMap<User, UserFullDetailsDto>();
        }
    }
}
