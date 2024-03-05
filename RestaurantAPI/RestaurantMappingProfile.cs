using RestaurantAPI.Models;
using AutoMapper;

namespace RestaurantAPI
{
	public class RestaurantMappingProfile : Profile
	{
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.City, s => s.MapFrom(r => r.Address.City))
                .ForMember(d => d.Street, s => s.MapFrom(r => r.Address.Street))
                .ForMember(d => d.PostalCode, s => s.MapFrom(r => r.Address.PostalCode));

            CreateMap<Dish, DishDto>();
		}
    }
}
