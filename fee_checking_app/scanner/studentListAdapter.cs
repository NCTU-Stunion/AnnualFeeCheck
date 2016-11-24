using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace scanner
{
	public class studentListAdapter : BaseAdapter<queryResult> //using queryResult as obj template
	{
		List<queryResult> items;

		Activity context;

		public studentListAdapter(Activity context, List<queryResult> items)
            : base()
        {
			this.context = context;
			this.items = items;
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override queryResult this[int position]
		{
			get { return items[position]; }
		}
		public override int Count
		{
			get { return items.Count; }
		}

		/// <summary>
		/// 系統會呼叫 並且render.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="convertView"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			var view = convertView;
			if (view == null)
			{
				//使用自訂的UserListItemLayout
				view = context.LayoutInflater.Inflate(Resource.Layout.UserListLayout, null);
			}

			view.FindViewById<TextView>(Resource.Id.tvName).Text = item.Title;
			view.FindViewById<TextView>(Resource.Id.tvDesc).Text = item.StuInfo;

			return view;
		}
	}
}

