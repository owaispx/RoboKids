using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace new_preject_ASP.net.Pages
{
    public class Log_inModel : PageModel

    { 
		public LoginInfo logininfo = new();
        public string errorMessage = "";

        private readonly string connectionstring;
        public Log_inModel (IConfiguration configuration)
        {
            connectionstring = configuration.GetConnectionString("DefaultConnection");


        }
        public void Onget()
        { 
        }

        public void OnPost()
        {
	       logininfo.Email = Request.Form["Email"];

		   logininfo.Password = Request.Form["Password"];
			if (logininfo.Email.Length == 0 || logininfo.Password.Length == 0)
			{
				errorMessage = "All feilds are required";
				return;
			}
			try
			{


				//creating a  sql connection by paasing the connection string 
				using (SqlConnection connection = new SqlConnection(connectionstring))
				{
					connection.Open();
					String sql = "SELECT Email, Password FROM Users WHERE Email = @Email";



					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						// passing the data from userinfo into sql query


						command.Parameters.AddWithValue("@Email", logininfo.Email);




						//Executing Reader on data base 

						using (SqlDataReader reader = command.ExecuteReader())
						{

							//reading hashed Passowrd from data base 
							if (reader.Read())
							{
								String storedHash = reader["Password"].ToString();



								// comapring storedhash with password from login form 

								if (BCrypt.Net.BCrypt.Verify(logininfo.Password, storedHash))

								//if Succesful Login then redirect to the secured page 
								{
									Response.Redirect("/List");
								}
								else
								{
									errorMessage = "Database Error";
								}
							}
						}
					}
				}

				errorMessage = "Login failed";
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			logininfo.Email = "";
			logininfo.Password = "";
			Response.Redirect("/Administration");
		}

	}
    
}
