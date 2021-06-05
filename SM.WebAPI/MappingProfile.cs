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



			CreateMap<DTOAddRealEstate, RealEstate>()
				.ForMember(s => s.images, c => c.MapFrom(m => m.ImgURL));
			
			CreateMap<DTOAddRealEstate, RealEstate>().ReverseMap()
				.ForMember(s => s.ImgURL, c => c.MapFrom(m => m.images));

			CreateMap<DTORentProperty, RentContract>();
			CreateMap<DTORentProperty, RentContract>().ReverseMap();

			CreateMap<DTORentProperty, RealEstate>();
			CreateMap<DTORentProperty, RealEstate>().ReverseMap();


			CreateMap<DTOImages, ImagesRealEstate>();
			CreateMap<DTOImages, ImagesRealEstate>().ReverseMap();

			CreateMap<DTOProvincia, Provincia>();
			CreateMap<DTOProvincia, Provincia>().ReverseMap();

			CreateMap<DTOContract, RentContract>();
			CreateMap<DTOContract, RentContract>().ReverseMap();


			CreateMap<DTOCuponPago, CuponDePago>()
				.ForPath(dest => dest.rentContract.ID, opts => opts.MapFrom(src => src.RentContractID))
				.ForPath(dest => dest.rentContract.RealEstate.RentFee, opts => opts.MapFrom(src => src.Monto))
				.ForPath(dest => dest.rentContract.RealEstate.User.WalletAddress, opts => opts.MapFrom(src => src.RentContractID));

			CreateMap<DTOCuponPago, CuponDePago>().ReverseMap()
				.ForPath(dest => dest.Monto, opts => opts.MapFrom(src => src.rentContract.RealEstate.RentFee))
				.ForPath(dest => dest.RentContractID, opts => opts.MapFrom(src => src.rentContract.ID))
				.ForPath(dest => dest.OwnerWallet, opts => opts.MapFrom(src =>src.rentContract.RealEstate.User.WalletAddress));



			CreateMap<DTOShowRealEstate, RealEstate>()
				.ForMember(s => s.images, c => c.MapFrom(m => m.ImgURL))
				.ForMember(s => s.Provincia, c => c.MapFrom(m => m.Provincia));

			CreateMap<DTOShowRealEstate, RealEstate>().ReverseMap()
				.ForMember(s => s.ImgURL, c => c.MapFrom(m => m.images))
				.ForMember(s => s.Provincia, c => c.MapFrom(m => m.Provincia));


			

			CreateMap<DTOShowFullDetail, RealEstate>()
				.ForMember(s => s.images, c => c.MapFrom(m => m.ImgURL))
				.ForMember(s => s.User, c => c.MapFrom(m => m.User));

			CreateMap<DTOShowFullDetail, RealEstate>().ReverseMap()
				.ForMember(s => s.ImgURL, c => c.MapFrom(m => m.images))
				.ForMember(s => s.User, c => c.MapFrom(m => m.User))
				.ForMember(s => s.Provincia, c => c.MapFrom(m => m.Provincia));

		
			
			CreateMap<DTOContractWithCupons, RentContract>().ReverseMap()
				.ForMember(r => r.ContractId, c => c.MapFrom(m => m.ID))
				.ForPath(dest => dest.Address, opts => opts.MapFrom(src => src.RealEstate.Address))
				.ForPath(dest => dest.Isvalid, opts => opts.MapFrom(src => src.Isvalid))
				.ForMember(r => r.CuponesDePago, c => c.MapFrom(m => m.cupones));





			CreateMap<DTOContractWithCupons, RentContract>()
				.ForMember(r => r.ID, c => c.MapFrom(m => m.ContractId))
				.ForPath(dest => dest.RealEstate.Address, opts => opts.MapFrom(src => src.Address))

				.ForMember(r => r.cupones, c => c.MapFrom(m => m.CuponesDePago));



		}
	}
}

