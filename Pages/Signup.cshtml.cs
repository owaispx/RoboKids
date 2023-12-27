using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using BCrypt.Net;
using System.Collections.ObjectModel;

namespace new_preject_ASP.net.Pages
{
    public class SignupModel : PageModel
    {
        private readonly string connectionstring;
        public SignupModel(IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");
        }


        public user userinfo = new();
        public string errorMessage = "";
        public string successMessage = "";

        public void Onpost()
        {
            userinfo.firstname = Request.Form["Firstname"];
            userinfo.lastname = Request.Form["Lastname"];
            userinfo.email = Request.Form["Email"];
            userinfo.password = Request.Form["Password"];


            if (userinfo.firstname.Length == 0 || userinfo.lastname.Length == 0 || userinfo.email.Length == 0 || userinfo.password.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                string hashedpassword = BCrypt.Net.BCrypt.HashPassword(userinfo.password);


                using SqlConnection connection = new(connectionstring);

                    connection.Open();

                string sql = "Insert into users" +
                    "(firstname,lastname,email,password)" +
                    "Values(@firstname,@lastname,@email,@hashedpassword)";





                using SqlCommand command = new(sql, connection);
				
					// passing the data from userinfo into sql query

					command.Parameters.AddWithValue("@firstname", userinfo.firstname);
					command.Parameters.AddWithValue("@lastname", userinfo.lastname);
					command.Parameters.AddWithValue("@email", userinfo.email);
					command.Parameters.AddWithValue("@hashedpassword", hashedpassword);


					// execute the sql query 
					command.ExecuteNonQuery();

					successMessage = "Registered";
				

			}
            catch (Exception ex)
            {
                errorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(ex.ToString());

            }
            userinfo.firstname = "";
			userinfo.lastname = "";
			userinfo.email = "";
			userinfo.password = "";
            Response.Redirect("/log-in");

		}

    }

}



   