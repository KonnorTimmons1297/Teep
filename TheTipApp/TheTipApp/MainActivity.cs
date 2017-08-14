using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android;
using Android.Content.PM;
using System.Collections.Generic;
using Android.Views;
using static Android.Widget.AdapterView;
using System.Threading.Tasks;
using Android.Support.V7.App;
using Android.Graphics;

namespace TheTipApp
{
    [Activity(Label = "TheTipApp", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class MainActivity : AppCompatActivity
    {
        #region Needed Permissions
        //Needed for setting ReadFileData and Write Permissions
        readonly string[] PermissionsReadWrite =
        {
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.ReadExternalStorage
        };

        const int RequestReadWriteId = 0;
        #endregion Needed Permissions

        private List<Tip> TipList;
        private DataFile<object> dataFile;
        private Android.Support.V7.Widget.Toolbar toolbarCtrl;

        ListView TipListViewCtrl;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.TipHistoryActivity);
            
            Task newTask = TryGetStoragePermissions();

            Setup();
        }

        private void ViewSelectedTip(object sender, ItemClickEventArgs e)
        {
            Tip SelectedTip = TipList[e.Position];

            ExamineSelectedTip(SelectedTip, e.Position);
        }

        private void ExamineSelectedTip(Tip selectedTip, int tipPosition)
        {
            Intent nextActivity = new Intent(this, typeof(TipSelectedActivity));
            nextActivity.PutExtra("selectedTipPosition", tipPosition);
            StartActivity(nextActivity);
            //OverridePendingTransition(Resource.Animation.slide_right, Resource.Animation.slide_left);
        }

        #region Setup

        private void Setup()
        {
            GetUIControls();
            SetupActionBar();
            SetupData();
            SetupTipAdapter();
            TipListViewCtrl.ItemClick += ViewSelectedTip;
        }

        private void GetUIControls()
        {
            TipListViewCtrl = FindViewById<ListView>(Resource.Id.tipHistoryListView);
            toolbarCtrl = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.TipHistoryToolbar);
        }

        private void SetupActionBar()
        {
            SetSupportActionBar(toolbarCtrl);
            SupportActionBar.SetDisplayShowTitleEnabled(false);

            var title = (TextView)toolbarCtrl.FindViewById(Resource.Id.tipHistoryTitle);
            Typeface tf = Typeface.CreateFromAsset(Assets, "Pacifico.ttf");
            title.SetTypeface(tf, TypefaceStyle.Normal);
        }

        private void SetupTipAdapter()
        {
            var items = new string[TipList.Count];

            for(var i = 0; i < TipList.Count; i++)
            {
                items[i] = TipList[i].RestaurantName + "(" + TipList[i].Date + ")";
            }

            ArrayAdapter<string> tipAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items); //Populate the list view
            try
            {
                TipListViewCtrl.Adapter = tipAdapter;
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        private void SetupData()
        {
            dataFile = new DataFile<object>();
            TipList = dataFile.Read();
        }
        
        #endregion Setup

        #region Action Bar

        //This sets the action bar menu items up
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MainActivityToolbar, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        //When an item is selected, this function is triggered.
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Intent nextActivity = new Intent(this, typeof(RateWaiterActivity));

            StartActivity(nextActivity);
            //OverridePendingTransition(Resource.Animation.slide_right, Resource.Animation.slide_left);

            return base.OnContextItemSelected(item);
        }

        #endregion Action Bar

        #region Getting Permissions

        private async Task TryGetStoragePermissions()
        {
            await GetStoragePermissionsAsync();
        }

        private async Task GetStoragePermissionsAsync()
        {
            while(CheckStoragePermissions() != Permission.Granted) {
                Toast.MakeText(this, "Storage Permissions needed", ToastLength.Long);
                RequestPermissions(PermissionsReadWrite, RequestReadWriteId);
            }
        }

        private Permission CheckStoragePermissions()
        {
            if (CheckWritePermission() && CheckReadPermission())
                return Permission.Granted;

            return Permission.Denied;
        }

        private bool CheckReadPermission()
        {
            const string permissionRead = Manifest.Permission.ReadExternalStorage;

            if (CheckSelfPermission(permissionRead) == (int)Permission.Granted)
                return true;

            return false;
        }

        private bool CheckWritePermission()
        {
            const string permissionWrite = Manifest.Permission.WriteExternalStorage;

            if (CheckSelfPermission(permissionWrite) == (int)Permission.Granted)
                return true;

            return false;
        }

        #endregion Getting Permissions
    }
}