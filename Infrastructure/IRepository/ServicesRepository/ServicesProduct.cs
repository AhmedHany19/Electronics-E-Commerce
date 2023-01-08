using Domain.Entity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IRepository.ServicesRepository
{
    public class ServicesProduct : IServicesRepository<Product>
    {
        private readonly ApplicationDbContext _context;
        public ServicesProduct(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Delete(Guid Id)
        {
            try
            {
                var result = FindBy(Id);
                result.CurrentState = (int)Helper.eCurrentState.Delete;
                _context.Products.Update(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Product FindBy(Guid Id)
        {
            try
            {
                return _context.Products.Include(x=>x.Brand).Include(x=>x.Category).FirstOrDefault(x => x.Id == Id && x.CurrentState > 0);

            }
            catch (Exception)
            {

                return null;
            }
        }

        public Product FindBy(string? Name)
        {
            try
            {
                return _context.Products.FirstOrDefault(x => x.Name.Equals(Name.Trim()) && x.CurrentState > 0);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<Product> GetAll()
        {
            try
            {
                return _context.Products.Include(x => x.Category).Include(x=>x.Brand).OrderBy(a => a.Name).Where(x => x.CurrentState > 0).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Product? model)
        {
            try
            {
                var result = FindBy(model.Id);
                if (result == null) //Create
                {

                    model.Id = Guid.NewGuid();
                    model.CurrentState = (int)Helper.eCurrentState.Active;
                    model.UpdateDate= DateTime.Now;
                    _context.Products.Add(model);
                }
                else // Update
                {
                    result.Name = model.Name;
                    result.Description = model.Description;
                    result.CurrentState = (int)Helper.eCurrentState.Active;
                    result.Price = model.Price;
                    result.ProductColor = model.ProductColor;
                    result.IsAvailable = model.IsAvailable;
                    result.Discount= model.Discount;
                    result.ImageUrl= model.ImageUrl;
                    result.Brand = model.Brand;
                    result.Category = model.Category;
                    result.UpdateDate = DateTime.Now;

                    _context.Products.Update(result);
                }
                _context.SaveChanges();


                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
