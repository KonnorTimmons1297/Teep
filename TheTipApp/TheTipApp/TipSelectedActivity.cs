using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;

namespace TheTipApp
{
    [Activity(Label = "TipSelectedActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class TipSelectedActivity : AppCompatActivity
    {
        private TextView waiterRatingCtrl;
        private TextView tipAmountCtrl;
        private TextView ticketTotalCtrl;
        private Android.Support.V7.Widget.Toolbar toolbarCtrl;

        private DataFile<List<Tip>> df;
        private List<Tip> tipList;
        private Tip selectedTip;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TipSelectedActivity);

            Setup();

            DisplayTipInformation();
        }

        public override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            
        }

        protected override void OnRestart() {
            Setup();
            base.OnRestart();
        }

        private void DisplayTipInformation()
        {
            waiterRatingCtrl.Text = selectedTip.WaiterRating.ToString();
            ticketTotalCtrl.Text = selectedTip.TicketTotal.ToString();
            tipAmountCtrl.Text = selectedTip.TipAmt.ToString();
        }

        private void DeleteSelectedTip()
        {
            tipList.Remove(selectedTip);
            df.Write(tipList);
        }

        #region Setup
        private void Setup()
        {
            GetUIControls();
            SetupDataFile();
            SetupTipList();
            GetSelectedTip();
            SetupActionBar();
        }

        private void GetUIControls()
        {
            waiterRatingCtrl = FindViewById<TextView>(Resource.Id.selectedTipWaiterRating);
            tipAmountCtrl = FindViewById<TextView>(Resource.Id.selectedTipTipAmount);
            ticketTotalCtrl = FindViewById<TextView>(Resource.Id.selectedTipTicketTotal);
            toolbarCtrl = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.selectedTipToolbar);
        }

        private void GetSelectedTip()
        {
            int selectedTipPosition = Intent.GetIntExtra("selectedTipPosition", 0);
            selectedTip = tipList[selectedTipPosition];
        }

        private void SetupDataFile()
        {
            df = new DataFile<List<Tip>>();
        }

        private void SetupTipList()
        {
            tipList = df.Read();
        }

        private void SetupActionBar()
        {
            SetSupportActionBar(toolbarCtrl);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            TextView restTitle = (TextView)toolbarCtrl.FindViewById(Resource.Id.toolbar_title);
            restTitle.Text = selectedTip.RestaurantName;
        }
        #endregion Setup

        #region Action Bar
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.TipSelectedActivityToolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            switch (id)
            {
                case Resource.Id.DeleteTipEntry:
                    DeleteSelectedTip();
                    StartActivity(new Intent(this, typeof(MainActivity)));
                    break;
                case Resource.Id.EditTipEntry:
                    Intent nextActivity = new Intent(this, typeof(EditTipActivity));
                    nextActivity.PutExtra("tipIndex", Intent.GetIntExtra("selectedTipPosition", 0));
                    StartActivity(nextActivity);
                    break;
                case Android.Resource.Id.Home:
                    StartActivity(new Intent(this, typeof(MainActivity)));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }
        #endregion Action Bar
    }
}