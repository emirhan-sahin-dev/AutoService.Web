using AutoService.Dto.BrandDtos;
using AutoService.Dto.SparePartDtos;
using AutoService.Dto.ServiceDetailDtos;
using AutoService.Dto.MechanicDtos;
using AutoMapper;
using AutoService.Dto.CustomerDtos;
using AutoService.Dto.ModelDtos;
using AutoService.Dto.ServiceRecordDtos;
using AutoService.Dto.VehicleDtos;
using AutoService.Entity.Entities;

namespace AutoService.Business.Mappings;

public class GeneralMapping : Profile
{
    public GeneralMapping()
    {
        
        // CUSTOMER
        

        CreateMap<Customer, CreateCustomerDto>().ReverseMap();

        CreateMap<Customer, UpdateCustomerDto>().ReverseMap();

        CreateMap<Customer, ResultCustomerDto>().ReverseMap();

        CreateMap<Customer, CustomerDetailDto>()
            .ForMember(dest => dest.VehicleCount,
                opt => opt.MapFrom(src => src.Vehicles.Count));


        
        // VEHICLE
        

        CreateMap<CreateVehicleDto, Vehicle>()
            .ForMember(dest => dest.Plate,
                opt => opt.MapFrom(src => src.Plate))
            .ForMember(dest => dest.VinNumber,
                opt => opt.MapFrom(src => src.VinNumber))
            .ForMember(dest => dest.Mileage,
                opt => opt.MapFrom(src => src.Mileage))
            .ReverseMap();

        CreateMap<UpdateVehicleDto, Vehicle>().ReverseMap();

        CreateMap<Vehicle, ResultVehicleDto>()
            .ForMember(dest => dest.Plate,
                opt => opt.MapFrom(src => src.Plate))
            .ForMember(dest => dest.BrandName,
                opt => opt.MapFrom(src => src.Model.Brand.BrandName))
            .ForMember(dest => dest.ModelName,
                opt => opt.MapFrom(src => src.Model.ModelName))
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer.FullName));

        CreateMap<Vehicle, VehicleDetailDto>()
            .ForMember(dest => dest.Plate,
                opt => opt.MapFrom(src => src.Plate))
            .ForMember(dest => dest.VinNumber,
                opt => opt.MapFrom(src => src.VinNumber))
            .ForMember(dest => dest.Mileage,
                opt => opt.MapFrom(src => src.Mileage))
            .ForMember(dest => dest.BrandName,
                opt => opt.MapFrom(src => src.Model.Brand.BrandName))
            .ForMember(dest => dest.ModelName,
                opt => opt.MapFrom(src => src.Model.ModelName))
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer.FullName));


        // ServiceRecord

        CreateMap<CreateServiceRecordDto, ServiceRecord>().ReverseMap();

        CreateMap<UpdateServiceRecordDto, ServiceRecord>().ReverseMap();

        CreateMap<ServiceRecord, ResultServiceRecordDto>()
        .ForMember(dest => dest.Plate,
         opt => opt.MapFrom(src => src.Vehicle.Plate))
        .ForMember(dest => dest.CustomerName,
        opt => opt.MapFrom(src => src.Vehicle.Customer.FullName));

        CreateMap<ServiceRecord, ServiceRecordDetailDto>()
         .ForMember(dest => dest.VehicleId,
         opt => opt.MapFrom(src => src.VehicleId))
         .ForMember(dest => dest.Plate,
         opt => opt.MapFrom(src => src.Vehicle.Plate))
         .ForMember(dest => dest.CustomerName,
        opt => opt.MapFrom(src => src.Vehicle.Customer.FullName));

        // ServiceDetail

        CreateMap<CreateServiceDetailDto, ServiceDetail>().ReverseMap();

        CreateMap<UpdateServiceDetailDto, ServiceDetail>().ReverseMap();

        CreateMap<ServiceDetail, ResultServiceDetailDto>()
            .ForMember(dest => dest.Plate,
                opt => opt.MapFrom(src => src.ServiceRecord.Vehicle.Plate))
            .ForMember(dest => dest.SparePartName,
                opt => opt.MapFrom(src => src.SparePart.PartName));

        CreateMap<ServiceDetail, ServiceDetailDetailDto>()
            .ForMember(dest => dest.Plate,
                opt => opt.MapFrom(src => src.ServiceRecord.Vehicle.Plate))
            .ForMember(dest => dest.SparePartName,
                opt => opt.MapFrom(src => src.SparePart.PartName));

        // MODEL


        CreateMap<Model, ResultModelDto>().ReverseMap();

        CreateMap<Model, CreateModelDto>().ReverseMap();

        CreateMap<Model, UpdateModelDto>().ReverseMap();

        CreateMap<Model, GetByIdModelDto>().ReverseMap();

        // Mechanic

        CreateMap<CreateMechanicDto, Mechanic>().ReverseMap();

        CreateMap<UpdateMechanicDto, Mechanic>().ReverseMap();

        CreateMap<Mechanic, ResultMechanicDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));

        CreateMap<Mechanic, MechanicDetailDto>().ReverseMap();

        // SparePart

        CreateMap<CreateSparePartDto, SparePart>().ReverseMap();

        CreateMap<UpdateSparePartDto, SparePart>().ReverseMap();

        CreateMap<SparePart, ResultSparePartDto>().ReverseMap();

        CreateMap<SparePart, SparePartDetailDto>().ReverseMap();

        //Brand

        CreateMap<Brand, ResultBrandDto>().ReverseMap();
        CreateMap<Brand, GetByIdBrandDto>().ReverseMap();
        CreateMap<Brand, CreateBrandDto>().ReverseMap();
        CreateMap<Brand, UpdateBrandDto>().ReverseMap();
    }
}