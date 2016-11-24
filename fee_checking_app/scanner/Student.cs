using SQLite;

namespace scanner
{
	public class Student
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(20)]
		public string name { get; set; }
		public string sex { get; set; }
		public string pay { get; set; }
		public string course { get; set;}
		[MaxLength(7)]
		public string stuID { get; set; }
	}
	public class queryResult
	{
		public string Title { get; set; }
		public string StuInfo{ get; set; }
	}

}