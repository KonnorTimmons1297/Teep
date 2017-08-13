using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;

namespace TheTipApp
{
    [Activity(Label = "TipTotalActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class TipTotalActivity : AppCompatActivity
    {
        private Tip newTip;
        private TextView tipAmtCtrl;
        private TextView ticketTotalAmtCtrl;
        private Button doneButton;
        private Android.Support.V7.Widget.Toolbar toolbarCtrl;

        private float TicketTotal;
        private int WaiterRating;
        private string RestaurantName;
        private float TipAmt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TipTotalActivity);

            Setup();

            DisplayTipAndTotal();

            doneButton.Click += DoneButton_Click;
        }
        
        private void DisplayTipAndTotal()
        {
            //Setting Variables for this Activity
            TipAmt = CalcTip(WaiterRating, TicketTotal);
            float ticketTotalAmount = TipAmt + TicketTotal;

            tipAmtCtrl.Text = String.Format("{0:c}", TipAmt);
            ticketTotalAmtCtrl.Text = String.Format("{0:c}", ticketTotalAmount);
        }

        private void DoneButton_Click(object sender, System.EventArgs e)
        {
            DataFile<Tip> dataFile = new DataFile<Tip>();

            newTip = new Tip() {
                WaiterRating = this.WaiterRating,
                RestaurantName = this.RestaurantName,
                TipAmt = this.TipAmt,
                TicketTotal = this.TicketTotal,
                Date = ParseDate(DateTime.Today)
            };

            dataFile.Write(newTip);

            StartNextActivity();
        }

        private void StartNextActivity()
        {
            Intent nextActivity = new Intent(this, typeof(MainActivity));

            Finish();

            StartActivity(nextActivity);
        }

        private string ParseDate(DateTime date)
        {
            string[] dateString = date.ToString().Split(' ');
            return dateString[0];
        }

        #region Setup
        private void Setup()
        {
            GetUIControls();
            SetupActionBar();
            GetData();
        }

        private void GetData()
        {
            TicketTotal = float.Parse(Intent.GetStringExtra("ticketTotalStr"));
            WaiterRating = Int32.Parse(Intent.GetStringExtra("waiterRating"));
            RestaurantName = Intent.GetStringExtra("restaurantNameStr");
        }

        private void GetUIControls()
        {
            tipAmtCtrl = FindViewById<TextView>(Resource.Id.tipAmt);
            ticketTotalAmtCtrl = FindViewById<TextView>(Resource.Id.ticketTotalAmt);
            doneButton = FindViewById<Button>(Resource.Id.doneButton);
            toolbarCtrl = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.TipTotalToolbar);
        }

        private void SetupActionBar()
        {
            SetSupportActionBar(toolbarCtrl);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
        }
        #endregion Setup

        #region Tip Calculation

        private float CalcTip(int waiterRating, float ticketTotal) {
            float tipRate = GetTipRate(waiterRating, ticketTotal);
            return (ticketTotal * (tipRate / 100.0f));
        }
        private float GetTipRate(int waiterRating, float ticketTotal)
        {
            if (waiterRating == 1) { return 10f; }
            else if (waiterRating == 2) { return 12.5f; }
            else if (waiterRating == 3) { return 15f; }
            else if (waiterRating == 4) { return 17.5f; }
            else { return 20f; }
        }

        #endregion Tip Calculation
    }
}