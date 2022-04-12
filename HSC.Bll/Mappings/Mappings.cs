﻿using AutoMapper;
using HSC.Transfer.Match;

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
        }
    }
}
