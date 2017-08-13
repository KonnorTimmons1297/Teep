using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Widget;

namespace TheTipApp
{
    //WindowSoftInputMode = Android.Views.SoftInput.AdjustResize
    [Activity(Label = "RestaurantTotalActivity", Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class RestaurantTotalActivity : AppCompatActivity
    {
        private EditText ticketTotalCtrl;
        private EditText restaurantNameCtrl;
        private Button calcButtonCtrl;
        private Android.Support.V7.Widget.Toolbar toolbarCtrl;

        private string waiterRating;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.RestaruantTotalActivity);

            Setup();

            ticketTotalCtrl.AfterTextChanged += EditText_TextChanged;
            restaurantNameCtrl.AfterTextChanged += EditText_TextChanged;

            calcButtonCtrl.Click += CalcButton_Click;
        }

        private void CalcButton_Click(object sender, System.EventArgs e)
        {
            restaurantNameCtrl.Text = ParseName(restaurantNameCtrl.Text);
            StartNextAcitivty();
        }

        private void EditText_TextChanged(object sender, AfterTextChangedEventArgs e)
        {
            if (BoxesFilled(ticketTotalCtrl, restaurantNameCtrl) == true)
            {
                calcButtonCtrl.Enabled = true;
            }
        }

        private void StartNextAcitivty()
        {
            Intent nextActivity = new Intent(this, typeof(TipTotalActivity));

            nextActivity.PutExtra("waiterRating", waiterRating);
            nextActivity.PutExtra("ticketTotalStr", ticketTotalCtrl.Text);
            nextActivity.PutExtra("restaurantNameStr", restaurantNameCtrl.Text);

            StartActivity(nextActivity);
        }

        #region Setup
        private void Setup()
        {
            GetUIControls();
            calcButtonCtrl.Enabled = false;
            SetupActionBar();
            RetrieveWaiterRating();
        }

        private void GetUIControls()
        {
            ticketTotalCtrl = FindViewById<EditText>(Resource.Id.ticketTotal);
            restaurantNameCtrl = FindViewById<EditText>(Resource.Id.restaurantName);
            calcButtonCtrl = FindViewById<Button>(Resource.Id.calcButton);
            toolbarCtrl = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.RestaurantTotalToolbar);
        }

        private void SetupActionBar()
        {
            SetSupportActionBar(toolbarCtrl);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
        }

        private void RetrieveWaiterRating()
        {
            waiterRating = Intent.GetStringExtra("waiterRating") ?? "";
        }

        #endregion Setup

        #region Utils
        private string ParseName(string name)
        {
            string[] s = name.Split(' ');

            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsLetter(s[i][0]))
                    s[i] = ToTitleCase(s[i]);
                else
                    continue;
            }

            return string.Join(" ", s);
        }
        private string ToTitleCase(string s)
        {
            return char.ToUpper(s[0]) + s.Substring(1);
        }
        private bool BoxesFilled(EditText txt1, EditText txt2)
        {
            if(txt1.Text.Length > 1 && txt2.Text.Length > 1)
                return true;

            return false;
        }
        #endregion Utils
    }
}