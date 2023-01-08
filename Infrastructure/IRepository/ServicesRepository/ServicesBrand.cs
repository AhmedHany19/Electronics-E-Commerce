using Domain.Entity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.IRepository.ServicesRepository
{
    public class ServicesBrand : IServicesRepository<Brand>
    {
        private readonly ApplicationDbContext _context;
        public ServicesBrand(ApplicationDbContext context)
        {
            _context = context;
        }


        public bool Delete(Guid Id)
        {
            try
            {
                var result = FindBy(Id);
                result.CurrentState = (int)Helper.eCurrentState.Delete;
                _context.Brands.Update(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Brand FindBy(Guid Id)
        {
            try
            {
                return _context.Brands.FirstOrDefault(x => x.Id == Id && x.CurrentState > 0);

            }
            catch (Exception)
            {

                return null;
            }
        }

        public Brand FindBy(string? Name)
        {
            try
            {
                return _context.Brands.FirstOrDefault(x => x.Name.Equals(Name.Trim()) && x.CurrentState > 0);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<Brand> GetAll()
        {
            try
            {
                return _context.Brands.Include(x => x.Category).OrderBy(a => a.Name).Where(x => x.CurrentState > 0).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Brand? model)
        {
            try
            {
                var result = FindBy(model.Id);
                if (result == null) //Create
                {

                    model.Id = Guid.NewGuid();
                    model.CurrentState = (int)Helper.eCurrentState.Active;
                    _context.Brands.Add(model);
                }
                else // Update
                {
                    result.Name = model.Name;
                    result.Description = model.Description;
                    result.CurrentState = (int)Helper.eCurrentState.Active;
                    
                    _context.Brands.Update(result);
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
