using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Helper
    {
        public const string PathImageuser = "/Images/Users/";
        public const string PathSaveImageuser = "Images/Users";

        public const string PathImageProduct = "/Images/Products/";
        public const string PathSaveImageProduct = "Images/Products";


        public const string PathImageCategory = "/Images/Category/";
        public const string PathSaveImageCategory = "Images/Category";

		public const string PathImageSlider = "/Images/Sliders/";
		public const string PathSaveImageSlider = "Images/Sliders";

		public const string Success = "success";
        public const string Error = "error";

        public const string MsgType = "msgType";
        public const string Title = "title";
        public const string Msg = "msg";

        public const string Save = "Save";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string Admin = "Admin";
		public const string SuperAdmin = "SuperAdmin";
		public const string Basic = "Basic";


		// Data Defaul User "SuperAdmin"
		public const string UserName = "superadmin@test.com";
        public const string Email = "superadmin@test.com";
        public const string Name = "SuperAdmin";
        public const string Password = "Pa$$w0rd";
        public const string ImageUser = "vector.jpg";

        // Data Defaul User "Basic"
        public const string UserNameBasic = "basic@test.com";
        public const string EmailBasic = "basic@test.com";
        public const string NameBasic = "BasicUser";
        public const string PasswordBasic = "Pa$$w0rd";
        public const string ImageUserBasic = "vector.jpg";



        public const string Permission = "Permission";
        public const string AdminNumberPhone = "+201090504001";
        public const string AdminEmail = "AhmedWilzy19@gmail.com";



        public enum eCurrentState
        {
            Active = 1,
            Delete = 0
        }

        public enum Roles
        {
            SuperAdmin,
            Admin,
            Basic
        }

		

		public const string ShoppingCartCount = "ShoppingCartCount";

        public const string StatusSubmitted = "Submitted";
        public const string StatusInProcess = "Being Prepared";
        public const string StatusReady = "Ready for Pickup";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusRejected = "Rejected";



        // List Of Controller And Some Method From AccountsController
        public enum PermissionsModuleName
        {
            Home,
            Accounts,
            Registers,
            Roles,
            Brands,
            Products,
            Categories,
            Sliders

        };



		public static string ConvertToRawHtml(string source)
		{
			char[] array = new char[source.Length];
			int arrayIndex = 0;
			bool inside = false;

			for (int i = 0; i < source.Length; i++)
			{
				char let = source[i];
				if (let == '<')
				{
					inside = true;
					continue;
				}
				if (let == '>')
				{
					inside = false;
					continue;
				}
				if (!inside)
				{
					array[arrayIndex] = let;
					arrayIndex++;
				}
			}
			return new string(array, 0, arrayIndex);
		}
	}
}
