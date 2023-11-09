using AutoMapper;
using TransactionsService.Dtos;
using TransactionsService.Models;
using CustomerService;

namespace TransactionsService.Profiles
{
    public class TransactionsProfile : Profile
    {
        public TransactionsProfile()
        {
            CreateMap<Customer, CustomerReadDto>()
                .ReverseMap();
            CreateMap<TransactionCreateDto, Transaction>()
                .ReverseMap();
            CreateMap<Transaction, TransactionReadDto>()
                .ReverseMap();
            CreateMap<CustomerPublishedDto, Customer>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcCustomerModel, Customer>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Transactions, opt => opt.Ignore());
        }
    }
}
