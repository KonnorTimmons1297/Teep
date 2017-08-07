﻿using System.Collections.Generic;
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
        private TextView waiterNameCtrl;
        private TextView tipDateCtrl;
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
            SupportActionBar.SetDisplayShowTitleEnabled(false);
        }

        private void DisplayTipInformation()
        {
            waiterNameCtrl.Text = selectedTip.WaiterName;
            waiterRatingCtrl.Text = selectedTip.WaiterRating.ToString();
            ticketTotalCtrl.Text = selectedTip.TicketTotal.ToString();
            tipAmountCtrl.Text = selectedTip.TipAmt.ToString();

            tipDateCtrl.Text = selectedTip.Date;
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
            SetupActionBar();
            SetupDataFile();
            SetupTipList();
            GetSelectedTip();
        }

        private void GetUIControls()
        {
            waiterRatingCtrl = FindViewById<TextView>(Resource.Id.selectedTipWaiterRating);
            tipAmountCtrl = FindViewById<TextView>(Resource.Id.selectedTipTipAmount);
            ticketTotalCtrl = FindViewById<TextView>(Resource.Id.selectedTipTicketTotal);
            waiterNameCtrl = FindViewById<TextView>(Resource.Id.selectedTipWaiterName);
            tipDateCtrl = FindViewById<TextView>(Resource.Id.selectedTipDate);
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
                    Toast.MakeText(this, "Edit this tip...", ToastLength.Short).Show();
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