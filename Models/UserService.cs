namespace CV_v2.Models
{
	public class UserService : List<User>
	{
		public UserService()
		{
			Add(new User { UserId = 1, Firstname = "Sofi", Lastname = "Carlsson", Email = "carlssonsofi99@gmail.com", Password = "123" });
			Add(new User { UserId = 2, Firstname = "Clara", Lastname = "Lunak", Email = "ClaraLunak@gmail.com", Password = "456" });
		}
	}
}
