using AutoMapper;
using HSC.Common.Enums;
using HSC.Dal.Entities;
using HSC.Transfer.Friends;
using HSC.Transfer.Groups;
using HSC.Transfer.History;
using HSC.Transfer.Match;
using HSC.Transfer.Searching;
using HSC.Transfer.SignalR;
using HSC.Transfer.Tournament;
using HSC.Transfer.User;

namespace HSC.Bll.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            var currentUserName = string.Empty;

            CreateMap<Dal.Entities.Match, MatchStartDto>()
                .ForMember(x => x.WhiteUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.White).UserName))
                .ForMember(x => x.WhiteRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.White).Rating))
                .ForMember(x => x.BlackUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.Black).UserName))
                .ForMember(x => x.BlackRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.Black).Rating));

            CreateMap<Dal.Entities.Match, MatchFullDataDto>()
                .ForMember(x => x.WhiteUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.White).UserName))
                .ForMember(x => x.WhiteRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.White).Rating))
                .ForMember(x => x.BlackUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.Black).UserName))
                .ForMember(x => x.BlackRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.Black).Rating))
                .ForMember(x => x.FinalPot, o => o.MapFrom(m => m.MatchPlayers.Max(mp => mp.CurrentBet)))
                .ForMember(x => x.IsHistoryMode, o => o.MapFrom(m => m.Result != Result.Ongoing));

            CreateMap<Group, GroupDto>()
                .ForMember(x => x.UserCount, o => o.MapFrom(m => m.Users.Count()));

            CreateMap<Group, GroupDetailsDto>()
                .ForMember(x => x.UserCount, o => o.MapFrom(m => m.Users.Count()));

            CreateMap<FriendRequest, FriendRequestDto>();

            CreateMap<User, FriendDto>();
            CreateMap<User, UserMenuDto>();
            CreateMap<User, UserFullDetailsDto>();

            CreateMap<Dal.Entities.Match, PastGameDto>()
                .ForMember(x => x.WhiteUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.White).UserName))
                .ForMember(x => x.WhiteRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.White).Rating))
                .ForMember(x => x.BlackUserName, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.Black).UserName))
                .ForMember(x => x.BlackRating, o => o.MapFrom(m => m.MatchPlayers.Single(mp => mp.Color == Color.Black).Rating))
                .ForMember(x => x.BetAmount, o => o.MapFrom(m => m.MatchPlayers.Max(mp => mp.CurrentBet)));

            CreateMap<ChatMessage, ChatMessageDto>();
            CreateMap<User, UserContextMenuDto>();

            CreateMap<Challenge, CustomGameDto>();

            CreateMap<TournamentPlayer, TournamentPlayerDto>();
            CreateMap<Tournament, TournamentDetailsDto>()
                .ForMember(x => x.HasJoined, o => o.MapFrom(m => m.Players.Any(p => p.UserName == currentUserName)));
            CreateMap<Tournament, TournamentListDto>()
                .ForMember(x => x.PlayerCount, o => o.MapFrom(m => m.Players.Count));
            CreateMap<TournamentMessage, TournamentMessageDto>();


        }
    }
}
