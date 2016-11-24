using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using SQLite;
using System.Collections.Generic;
using Xamarin.Android;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views;

namespace scanner
{
   [Activity(Label = "scanner", MainLauncher = false, Theme = "@style/MyTheme")]
    public class Search : ActionBarActivity
    {
        private mySetToolBar setToolBar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Search);
			var find = FindViewById<EditText>(Resource.Id.searchBar);
            var Main = FindViewById<TextView>(Resource.Id.text_toolbar_title);
			//get intent input

            //set toolbar
            SupportToolbar Toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(Toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            setToolBar = new mySetToolBar(this, ref Toolbar,ApplicationContext);

            // back to main page

			string mainAcitivityText = Intent.GetStringExtra("search") ?? "no data";
			if (mainAcitivityText != "no data")
			{
				Console.WriteLine("get data from main!\n");
				find.Text = mainAcitivityText;
				searchInit(mainAcitivityText);
			}
			//setup list view 
			Main.Click += delegate
            {
                var main = new Intent(this, typeof(MainActivity));
                StartActivity(main);
            };
            
            find.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
				searchInit(e.Text.ToString());
            };
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            setToolBar.DrawerToggleEvent(ref item);
            return base.OnOptionsItemSelected(item);
        }
		private List<queryResult> createResultList(TableQuery <Student> query)			
		{
			List<queryResult> results = new List<queryResult>();
			string buffer;
			int num = 0;
			if (query.Count() == 0)
			{
				results.Add(new queryResult { Title = "查無資料!", StuInfo = " " });
			}
			else {
				foreach (var stu in query)
				{
					if (num >= 40)
						break;
					if (stu.sex == "male")
						buffer = "男";
					else if (stu.sex == "female")
						buffer = "女";
					else
						buffer = "  無資料";
					if (stu.course == "none")
						buffer += "  無系上資料";
					else
						buffer += "  " + stu.course;
					results.Add(new queryResult { Title = stu.stuID + " " + stu.name, StuInfo = buffer });
					num++;
				}
			}
			return results;
		}
		private void searchInit(string input)
		{
			string queryID = input;
			var sqlLiteFilePath = GetFileStreamPath("") + "/db_user.db";
			var db = new SQLiteConnection(sqlLiteFilePath);
			ListView listView = FindViewById<ListView>(Resource.Id.listViewMain);
			List<queryResult> results;
			try
			{
				var query = db.Table<Student>().Where(v => v.stuID.Contains(queryID));
				results = createResultList(query);
				listView.Adapter = new studentListAdapter(this, results);
			}
			catch (Exception ex)
			{
				Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
			}

		}
    }
}

