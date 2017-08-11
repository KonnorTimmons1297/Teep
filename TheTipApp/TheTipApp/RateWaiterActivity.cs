using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using static Android.Widget.SeekBar;

namespace TheTipApp
{
    [Activity(Label = "RateWaiterActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    class RateWaiterActivity : AppCompatActivity 
    {
        private SeekBar rateWaiterScaleCtrl;
        private TextView currentRatingTextCtrl;
        private Button nextButtonCtrl;
        private Android.Support.V7.Widget.Toolbar toolbarCtrl;

        public int WaiterRating { get; set; }

        protected override void OnCreate(Bundle saveBundleInstance)
        {
            base.OnCreate(saveBundleInstance);

            SetContentView(Resource.Layout.RateWaiterActivity);

            Setup();

            nextButtonCtrl.Enabled = false;
            
            rateWaiterScaleCtrl.ProgressChanged += GetWaiterRating;
            
            nextButtonCtrl.Click += NextButton_Click;
        }

        private void NextButton_Click(object sender, System.EventArgs e)
        {
            Intent nextActivity = new Intent(this, typeof(RestaurantTotalActivity));
            nextActivity.PutExtra("waiterRating", WaiterRating.ToString());
            StartActivity(nextActivity);
        }

        private void GetWaiterRating(object sender, ProgressChangedEventArgs e)
        {
            currentRatingTextCtrl.Text = string.Format("{0}", e.Progress);
            WaiterRating = e.Progress;
            nextButtonCtrl.Enabled = true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;

            switch (id)
            {
                case Android.Resource.Id.Home:
                    StartActivity(new Intent(this, typeof(MainActivity)));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        #region Setup

        private void Setup()
        {
            GetUIControls();
            SetupActionBar();
        }

        private void GetUIControls()
        {
            rateWaiterScaleCtrl = FindViewById<SeekBar>(Resource.Id.rateWaiterBar);
            currentRatingTextCtrl = FindViewById<TextView>(Resource.Id.currentWaiterRating);
            nextButtonCtrl = FindViewById<Button>(Resource.Id.nextButton);
            toolbarCtrl = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.RaitWaiterToolbar);
        }

        private void SetupActionBar()
        {
            SetSupportActionBar(toolbarCtrl);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
        }

        #endregion Setup
    }
}