//using Domain.Entity;
//using Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.IRepository.ServicesRepository
//{
//	public class ServicesSlider : IServicesRepository<Slider>
//	{
//		private readonly ApplicationDbContext _context;
//		public ServicesSlider(ApplicationDbContext context)
//		{
//			_context = context;
//		}

//		public bool Delete(Guid Id)
//		{
//			try
//			{
//				var result = FindBy(Id);
//				_context.sliders.Update(result);
//				_context.SaveChanges();
//				return true;
//			}
//			catch (Exception)
//			{

//				return false;
//			}
//		}

//		public Slider FindBy(Guid Id)
//		{
//			try
//			{
//				return _context.sliders.FirstOrDefault(x => x.Id == Id);

//			}
//			catch (Exception)
//			{

//				return null;
//			}
//		}

//		public Slider FindBy(string? Name)
//		{
//            try
//            {
//                return _context.sliders.FirstOrDefault(x => x.Name.Equals(Name.Trim()));
//            }
//            catch (Exception)
//            {

//                return null;
//            }
//        }

//		public List<Slider> GetAll()
//		{
//			try
//			{
//				return _context.sliders.OrderBy(a => a.Id).ToList();
//			}
//			catch (Exception)
//			{

//				return null;
//			}
//		}

//		public bool Save(Slider? model)
//		{
//			try
//			{
//				var result = FindBy(model.Id);
//				if (result == null) //Create
//				{

//					model.Id = Guid.NewGuid();
//					_context.sliders.Add(model);
//				}
//				else // Update
//				{
//					result.Name= model.Name;
//					result.ImageUrl = model.ImageUrl;					
//					_context.sliders.Update(result);
//				}
//				_context.SaveChanges();


//				return true;

//			}
//			catch (Exception)
//			{

//				return false;
//			}
//		}
//	}
//}

