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
    public class ServicesLogBrand : IServicesRepositoryLog<LogBrand>
    {
        private readonly ApplicationDbContext _context;
        public ServicesLogBrand(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Delete(Guid Id, Guid UserId)
        {
            try
            {
                var logBrands = new LogBrand
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Delete,
                    Date = DateTime.Now,
                    UserId = UserId,
                    BrandId = Id,
                };
                _context.LogBrands.Add(logBrands);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteLog(Guid Id)
        {
            try
            {
                var result = FindBy(Id);
                if (result != null)
                {
                    _context.LogBrands.Remove(result);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public LogBrand FindBy(Guid Id)
        {
            try
            {
                return _context.LogBrands.FirstOrDefault(x => x.Id == Id);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<LogBrand> GetAll()
        {
            try
            {
                return _context.LogBrands.Include(x => x.Brand).OrderByDescending(a => a.Date).ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Guid Id, Guid UserId)
        {
            try
            {
                var logBrand = new LogBrand
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Save,
                    Date = DateTime.Now,
                    UserId = UserId,
                    BrandId = Id,
                };
                _context.LogBrands.Add(logBrand);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Update(Guid Id, Guid UserId)
        {
            try
            {
                var logBrand = new LogBrand
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Update,
                    Date = DateTime.Now,
                    UserId = UserId,
                    BrandId = Id,
                };
                _context.LogBrands.Add(logBrand);
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
