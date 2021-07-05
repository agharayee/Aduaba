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
            CreateMap<Product, AddProductDto>().ReverseMap();
            CreateMap<Product, GetProductDto>().ReverseMap();
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
