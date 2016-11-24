using System;
using SQLite;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace scanner
{
	public class DatabaseFunctions
	{
		public string createDB(string dbPath)
		{
			try
			{
				var db = new SQLiteConnection(dbPath);
				db.CreateTable<Student>();
				db.Close();
				return "db create success";
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}
		public void buildDB(string result, string dbPath)
		{
			byte[] nameUTF8bytes;
			byte[] courseUTF8bytes;
			string name, course;
			if (result != null && result.Length >= 50)
			{
				try
				{
					var db = new SQLiteConnection(dbPath);

					var students = Newtonsoft.Json.Linq.JObject.Parse(result);
					foreach (var student in students["students"])
					{
					 	nameUTF8bytes = Encoding.UTF8.GetBytes((string)student["name"]);
						courseUTF8bytes = Encoding.UTF8.GetBytes((string)student["course"]);
						name = Encoding.UTF8.GetString(nameUTF8bytes, 0, nameUTF8bytes.Length);
						course = Encoding.UTF8.GetString(courseUTF8bytes, 0, courseUTF8bytes.Length);
					 	var stu = new Student { name = name, stuID = (string)student["stuID"], pay = (string)student["pay"], sex = (string)student["sex"],course = course };
					 	db.Insert(stu);
					}
					db.Close();
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Sqlite exploded");
					Console.WriteLine(ex.Message);
				}
			}
		}
		public string insertDB(string name, string id, string pay, string sex, string dbPath)
		{
			try
			{
				string course = "none";
				var db = new SQLiteConnection(dbPath);
				var stu = new Student { name = name, stuID = id, course = course,pay = pay, sex = sex };
				db.Insert(stu);
				db.Close();

				return "success";
			}
			catch (SQLiteException ex)
			{
				return ex.Message;
			}
		}
	}
}
