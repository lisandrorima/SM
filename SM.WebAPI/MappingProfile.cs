using AutoMapper;
using SM.DAL.Models;
using SM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SM.WebAPI
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<DTOUser, User>();
			CreateMap<DTOUser, User>().ReverseMap();
			CreateMap<DTOUserReDetail, User>();
			CreateMap<DTOUserReDetail, User>().ReverseMap();

			

			CreateMap<DTOAddRealEstate, RealEstate>();

			CreateMap<DTOImages, ImagesRealEstate>();
			CreateMap<DTOImages, ImagesRealEstate>().ReverseMap();

			CreateMap<DTOShowRealEstate, RealEstate>()
				.ForMember(s => s.images, c => c.MapFrom(m => m.ImgURL));

			CreateMap<DTOShowRealEstate, RealEstate>().ReverseMap()
				.ForMember(s => s.ImgURL, c => c.MapFrom(m => m.images));


			CreateMap<DTOShowFullDetail, RealEstate>()
				.ForMember(s => s.images, c => c.MapFrom(m => m.ImgURL))
				.ForMember(s => s.User, c => c.MapFrom(m => m.User));

			CreateMap<DTOShowFullDetail, RealEstate>().ReverseMap()
				.ForMember(s => s.ImgURL, c => c.MapFrom(m => m.images))
				.ForMember(s => s.User, c => c.MapFrom(m => m.User)); ;



		}
	}
}

