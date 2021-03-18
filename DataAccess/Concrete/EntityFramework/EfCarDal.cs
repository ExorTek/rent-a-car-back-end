﻿using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, RentACarDbContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            using (RentACarDbContext context = new RentACarDbContext())
            {
                var result = (from p in filter == null ? context.Cars : context.Cars.Where(filter)
                    join c in context.Colors on p.ColorId equals c.ColorId
                    join d in context.Brands on p.BrandId equals d.BrandId
                    join im in context.CarImages on p.Id equals im.CarId
                    select new CarDetailDto
                    {
                        BrandName = d.BrandName,
                        ColorName = c.ColorName,
                        DailyPrice = p.DailyPrice,
                        Description = p.Description,
                        ModelYear = p.ModelYear,
                        Id = p.Id,
                        Date = im.Date,
                        ImagePath = im.ImagePath,
                        ImageId = im.Id
                    }).ToList();
                return result.GroupBy(p => p.Id)
                    .Select(p => p.FirstOrDefault()).ToList();
            }
        }

        public List<CarDetailDto> GetCarDetailById(int carId)
        {
            using (RentACarDbContext context = new RentACarDbContext())
            {
                var result = from p in context.Cars 
                    join c in context.Colors on p.ColorId equals c.ColorId
                    join d in context.Brands on p.BrandId equals d.BrandId
                    join im in context.CarImages on p.Id equals im.CarId
                             where p.Id == carId
                    select new CarDetailDto
                    {
                        BrandName = d.BrandName,
                        ColorName = c.ColorName,
                        DailyPrice = p.DailyPrice,
                        Description = p.Description,
                        ModelYear = p.ModelYear,
                        Id = p.Id,
                        Date = im.Date,
                        ImagePath = im.ImagePath,
                        ImageId = im.Id
                    };
                return result.ToList();
            }
        }
    }
}