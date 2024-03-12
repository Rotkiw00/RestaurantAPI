using RestaurantAPI.Models;
using AutoMapper;

namespace RestaurantAPI
{
	public class RestaurantMappingProfile : Profile
	{
        public RestaurantMappingProfile()
        {
			#region AutoMapper profile for GET			
			CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.City, s => s.MapFrom(r => r.Address.City))
                .ForMember(d => d.Street, s => s.MapFrom(r => r.Address.Street))
                .ForMember(d => d.PostalCode, s => s.MapFrom(r => r.Address.PostalCode));

            CreateMap<Dish, DishDto>();
			#endregion

			#region AutoMapper profile for POST
			CreateMap<CreateRestaurantDto, Restaurant>()
				.ForMember(d => d.Address, s => s.MapFrom(dto => new Address()
					{ City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }));
			#endregion
		}
	}
}
