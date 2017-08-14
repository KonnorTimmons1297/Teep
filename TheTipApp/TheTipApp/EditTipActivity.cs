using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using System;
using System.Collections.Generic;

namespace TheTipApp
{
    [Activity(Label = "EditTipActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class EditTipActivity : AppCompatActivity
    {
        private Android.Support.V7.Widget.Toolbar toolbarCtrl;
        private EditText editTipNameCtrl;
        private EditText editTipAmountCtrl;
        private EditText editTipTicketTotalCtrl;
        private Button saveButtonCtrl;

        private Tip editTip;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.EditTipActivity);

            Setup();

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            //Set Tip information
            //Get Tip Index
            //Set edited tip in tip list
            //Write to file.
            //Finish

            DataFile<List<Tip>> dataFile = new DataFile<List<Tip>>();
            var tipIndex = Intent.GetIntExtra("tipIndex", 0);
            var tipList = dataFile.Read();

            editTip.RestaurantName = editTipNameCtrl.Text;
            editTip.TipAmt = (float)Convert.ToDecimal(editTipAmountCtrl.Text);
            editTip.TicketTotal = (float)Convert.ToDecimal(editTipTicketTotalCtrl.Text);

            tipList[tipIndex] = editTip;

            dataFile.Write(tipList);

            Finish();
        }

        #region Setup

        private void Setup()
        {
            GetUIControls();
            saveButtonCtrl.Click += SaveButton_Click;
            SetupActionBar();
            LoadTipData();
        }

        private void GetUIControls()
        {
            toolbarCtrl = FindViewById <Android.Support.V7.Widget.Toolbar>(Resource.Id.EditActivityToolbar);
            editTipNameCtrl = FindViewById<EditText>(Resource.Id.editTipName);
            editTipAmountCtrl = FindViewById<EditText>(Resource.Id.editTipTipAmount);
            editTipTicketTotalCtrl = FindViewById<EditText>(Resource.Id.editTipTicketTotal);
            saveButtonCtrl = FindViewById<Button>(Resource.Id.editTipSaveButton);
        }

        private void SetupActionBar()
        {
            SetSupportActionBar(toolbarCtrl);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
        }

        private void LoadTipData()
        {
            DataFile<Tip> df = new DataFile<Tip>();
            editTip = df.Read()[Intent.GetIntExtra("tipIndex", 0)];

            editTipNameCtrl.Text = editTip.RestaurantName;
            editTipAmountCtrl.Text = editTip.TipAmt.ToString();
            editTipTicketTotalCtrl.Text = editTip.TicketTotal.ToString();
        }

        #endregion Setup
    }
}