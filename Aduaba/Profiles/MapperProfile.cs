using Aduaba.Data.Models;
using Aduaba.Dtos;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Category, AddCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Product, AddProductDto>().ForMember(c => c.FeaturedProduct, opt => opt.MapFrom(scr => scr.IsFeaturedProduct))
                .ForMember(c => c.BestSelling, opt => opt.MapFrom(scr => scr.IsBestSelling))
                .ReverseMap();
            CreateMap<Product, GetProductDto>().ForMember(c => c.FeaturedProduct, opt => opt.MapFrom(scr => scr.IsFeaturedProduct))
                .ForMember(c => c.BestSelling, opt => opt.MapFrom(scr => scr.IsBestSelling)).ReverseMap();
            CreateMap<Category, GetCategoryDto>().ReverseMap();

            CreateMap<Product, UpdateProductDto>().ReverseMap();

            CreateMap<AddCustomerDto, Customer>().ReverseMap();
            CreateMap<AddToCartDto, Cart>().ReverseMap();
            CreateMap<Customer, GetCustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerDto>().ReverseMap();



            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<AddCustomerDto, Customer>().ReverseMap();
            CreateMap<AddToCartDto, Cart>().ReverseMap();


            CreateMap<GetShippingAddressDto, ShippingAddress>().ReverseMap();
            CreateMap<AddShippingAddressDto, ShippingAddress>().ReverseMap();
            CreateMap<UpdateShippingAddressDto, ShippingAddress>().ReverseMap();
            CreateMap<AddToWishList, WishList>().ReverseMap();
        }
    }
}
