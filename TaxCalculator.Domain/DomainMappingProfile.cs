using AutoMapper;
using TaxCalculator.Domain.Models;
using TaxCalculator.Infrastructure.Entities;

namespace TaxCalculator.Domain
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<TaxResult, TaxResultEntity>()
                .ForMember(dest => dest.TotalTax, opt => opt.MapFrom(c => c.TotalTax))
                .ForMember(dest => dest.NetMonthlySalary, opt => opt.MapFrom(c => c.NetMonthlySalary))
                .ForMember(dest => dest.GrossAnnualSalary, opt => opt.MapFrom(c => c.GrossAnnualSalary))
                .ForMember(dest => dest.NetAnnualSalary, opt => opt.MapFrom(c => c.NetAnnualSalary))
                .ForMember(dest => dest.GrossMonthlySalary, opt => opt.MapFrom(c => c.GrossMonthlySalary))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore());

            CreateMap<TaxResultEntity, TaxResult>()
                .ForMember(dest => dest.TotalTax, opt => opt.MapFrom(c => c.TotalTax))
                .ForMember(dest => dest.NetMonthlySalary, opt => opt.MapFrom(c => c.NetMonthlySalary))
                .ForMember(dest => dest.GrossAnnualSalary, opt => opt.MapFrom(c => c.GrossAnnualSalary))
                .ForMember(dest => dest.NetAnnualSalary, opt => opt.MapFrom(c => c.NetAnnualSalary))
                .ForMember(dest => dest.GrossMonthlySalary, opt => opt.MapFrom(c => c.GrossMonthlySalary));
        }
    }
}
