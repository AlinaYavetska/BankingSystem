using AutoMapper;
using CustomerService.Dtos;
using CustomerService.Models;

namespace CustomerService.Profiles
{
    public class CustomersProfile : Profile
    {
        public CustomersProfile() 
        {
            CreateMap<Customer, CustomerReadDto>()
                .ReverseMap();
            CreateMap<CustomerCreateDto, Customer>()
                .ReverseMap();
            CreateMap<CustomerReadDto, CustomerPublishedDto>()
                .ReverseMap();
            CreateMap<Customer, GrpcCustomerModel>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

        }
    }
}
