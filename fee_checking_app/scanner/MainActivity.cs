using Android.App;
using Android.Widget;
using Android.OS;
using Android.Net;
using System.Net;
using System.Net.Http;
using Android.Content;
using System.Collections.Generic;
using System.Json;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System;
using SQLite;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views;

namespace scanner
{
    [Activity(Label = "scanner", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/MyTheme")]

    public class MainActivity : ActionBarActivity
    {

        private async void BtnScan_Click(object sender, EventArgs e)
        {
            //var options = new ZXing.Mobile.MobileBarcodeScanningOptions();
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();
			ApiFunctions apiGet = new ApiFunctions();
            //options.PossibleFormats = new List<ZXing.BarcodeFormat>() {
            //	ZXing.BarcodeFormat.CODE_93, ZXing.BarcodeFormat.CODE_39
            //};

            var options = new ZXing.Mobile.MobileBarcodeScanningOptions()
            {
                TryHarder = true,
                PossibleFormats = new List<ZXing.BarcodeFormat>
                {
                    ZXing.BarcodeFormat.CODE_39
                }
            };
            var result = await scanner.Scan(options);
            if (result != null)
            {
                string id;
                id = result.Text;
                id = id.Remove(0, 2);
                id = id.Remove(id.Length - 1);
                //Toast.MakeText(this, id, ToastLength.Long).Show();
				string url = secret.url +"students" + id.ToString();
                JsonValue pay = await apiGet.FetchStudentAsync(url);
                if (apiGet.GotPay(pay))
                {
                    Toast.MakeText(this, id + ",有繳學聯會費喔", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, id + ",沒有繳學聯會費！！", ToastLength.Long).Show();
                }
            }
        }
        private async void testBtn_Click(object sender, EventArgs e)
        {
            var sqlLiteFilePath = GetFileStreamPath("") + "/db_user.db";
			string TARGETURL = secret.url+"payOnly";
			string result = null;

			DatabaseFunctions dbFunc = new DatabaseFunctions();
			ApiFunctions apiGet = new ApiFunctions();

			if (File.Exists(sqlLiteFilePath))
            {
                File.Delete(sqlLiteFilePath);
            }
            dbFunc.createDB(sqlLiteFilePath);
			Console.WriteLine("Database created");

			result = await apiGet.parseData(TARGETURL); //must be used in threading pool otherwise add async to function

			ProgressDialog progress;
            progress = ProgressDialog.Show(this, "Loading", "Please Wait...", true);
            await Task.Factory.StartNew(
            () =>
            {

				if (result != null && result.Length >= 50)
               {
					dbFunc.buildDB(result,sqlLiteFilePath);
               }
            }).ContinueWith(
                t =>
                {
                    if (progress != null)
                    {
                        progress.Hide();
                    }

                }, TaskScheduler.FromCurrentSynchronizationContext()
            );
        }

        private mySetToolBar setToolBar;
        public static buttonEvent button; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //Set Toolbar
            SupportToolbar Toolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(Toolbar);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            
            setToolBar = new mySetToolBar(this, ref Toolbar, ApplicationContext);
            button=new buttonEvent(BtnScan_Click, testBtn_Click);

			EditText mainSearchBar = FindViewById<EditText>(Resource.Id.editTextMain);
			mainSearchBar.KeyPress += (object sender, View.KeyEventArgs e) =>
			{
				e.Handled = false;
				if (e.Event.Action == KeyEventActions.Down && e.KeyCode == Keycode.Enter)
				{
					Console.WriteLine("Enter detected\n");
					var passSearchInfo = new Intent(this, typeof(Search));
					passSearchInfo.PutExtra("search", mainSearchBar.Text);
					StartActivity(passSearchInfo);
					e.Handled = true;
				}
			};
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            setToolBar.DrawerToggleEvent(ref item);
            return base.OnOptionsItemSelected(item);
        }
    }
}