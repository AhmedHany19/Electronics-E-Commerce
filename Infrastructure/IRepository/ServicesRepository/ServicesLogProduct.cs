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
    public class ServicesLogProduct : IServicesRepositoryLog<LogProduct>
    {
        private readonly ApplicationDbContext _context;
        public ServicesLogProduct(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Delete(Guid Id, Guid UserId)
        {
            try
            {
                var logProducts = new LogProduct
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Delete,
                    Date = DateTime.Now,
                    UserId = UserId,
                    ProductId = Id,
                };
                _context.LogProducts.Add(logProducts);
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
                    _context.LogProducts.Remove(result);
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

        public LogProduct FindBy(Guid Id)
        {
            try
            {
                return _context.LogProducts.FirstOrDefault(x => x.Id == Id);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<LogProduct> GetAll()
        {
            try
            {
                return _context.LogProducts.Include(x => x.Product).OrderByDescending(a => a.Date).ToList();
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
                var logProducts = new LogProduct
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Save,
                    Date = DateTime.Now,
                    UserId = UserId,
                    ProductId = Id,
                };
                _context.LogProducts.Add(logProducts);
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
                var logProducts = new LogProduct
                {
                    Id = Guid.NewGuid(),
                    Action = Helper.Update,
                    Date = DateTime.Now,
                    UserId = UserId,
                    ProductId = Id,
                };
                _context.LogProducts.Add(logProducts);
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
