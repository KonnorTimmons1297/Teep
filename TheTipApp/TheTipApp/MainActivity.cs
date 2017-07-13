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

            SetContentView (Resource.Layout.TipHistoryActivity); //Set our view from the "TipHistoryActivity" layout resource
            
            Task newTask = TryGetStoragePermissions(); //Getting Storage Permissions from User.

            Setup();

            TipListViewCtrl.ItemClick += GetSelectedTip;
        }

        private void GetSelectedTip(object sender, ItemClickEventArgs e)
        {
            Tip SelectedTip = TipList[e.Position];

            StartNextActivity(SelectedTip, e.Position);
        }

        private void StartNextActivity(Tip selectedTip, int tipPosition)
        {
            Intent nextActivity = new Intent(this, typeof(TipSelectedActivity));
            nextActivity.PutExtra("selectedTipPosition", tipPosition);
            StartActivity(nextActivity);
            OverridePendingTransition(Resource.Animation.slide_right, Resource.Animation.slide_left);
        }

        #region Setup

        private void Setup()
        {
            GetUIControls();
            SetupActionBar();
            SetupData();
            SetupTipAdapter();
        }

        private void GetUIControls()
        {
            TipListViewCtrl = FindViewById<ListView>(Resource.Id.tipHistoryListView);
            toolbarCtrl = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.TipHistoryToolbar);
        }

        private void SetupActionBar()
        {
            SetSupportActionBar(toolbarCtrl);
        }

        private void SetupTipAdapter()
        {
            ArrayAdapter<Tip> tipAdapter = new ArrayAdapter<Tip>(this, Android.Resource.Layout.SimpleListItem1, TipList); //Populate the list view
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
            //OverridePendingTransition(Resource.Animation.slide_up, Resource.Animation.slide_down);

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