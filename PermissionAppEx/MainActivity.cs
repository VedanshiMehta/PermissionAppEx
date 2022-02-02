using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using System;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;

namespace PermissionAppEx
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button mybutton;
        private ISharedPreferences _sharedPrefrence;
        private ISharedPreferencesEditor _sharedPreferenceEditor;
        private AlertDialog.Builder builder;
        private const int REQUEST_STORAGE = 2;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            UIReference();
            UIClickEvents();
            InitializedSharedPreference();
        }

        [Obsolete]
        private void InitializedSharedPreference()
        {
            _sharedPrefrence = PreferenceManager.GetDefaultSharedPreferences(this);
            _sharedPreferenceEditor = _sharedPrefrence.Edit();
        }

        private void UIClickEvents()
        {
            mybutton.Click += Mybutton_Click;
        }

        private void Mybutton_Click(object sender, EventArgs e)
        {
            if (Build.VERSION.SdkInt > BuildVersionCodes.M)

            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Android.Content.PM.Permission.Granted)
                {
                    Toast.MakeText(this, "Camera Permisson is already granted", ToastLength.Short).Show();

                }
                else
                {

                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage))
                    {
                        ShowRAtionaleDialouge(REQUEST_STORAGE);
                    }

                    else if (_sharedPrefrence.GetBoolean(Manifest.Permission.ReadExternalStorage, false))
                    {
                        builder = new AlertDialog.Builder(this);
                        builder.SetTitle(Resource.String.storage_permission_title);
                        builder.SetMessage(Resource.String.storage_permission_message);
                        builder.SetPositiveButton(Resource.String.grant,

                            (s, e) =>
                             {
                                 Intent intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
                                 Android.Net.Uri uri = Android.Net.Uri.FromParts(scheme: "package",PackageName, null);
                                 intent.SetData(uri);
                                 StartActivityForResult(intent, requestCode: 12);


                             });
                        builder.SetNegativeButton(Resource.String.cancel, (s, e) => { builder.Dispose(); });
                        builder.Show();


                    }
                    else
                    {
                        RequestPermissions(REQUEST_STORAGE);
                    }

                    _sharedPreferenceEditor.PutBoolean(Manifest.Permission.ReadExternalStorage, true);
                    _sharedPreferenceEditor.Commit();
                }

            }

            else {

                Toast.MakeText(this, "Camera Permisson is already granted", ToastLength.Short).Show();
            }
        }


        private void RequestPermissions(int rEQUEST_STORAGE)
        {
            RequestPermissions(new string[] { Manifest.Permission.WriteExternalStorage, Manifest.Permission.ReadExternalStorage }, rEQUEST_STORAGE);
        }

        private void ShowRAtionaleDialouge(int rEQUEST_STORAGE)
        {
            builder = new AlertDialog.Builder(this);
            builder.SetTitle(Resource.String.storage_permission_title);
            builder.SetMessage(Resource.String.storage_permission_message);
            builder.SetPositiveButton(Resource.String.ok,(s, e) =>{ RequestPermissions(rEQUEST_STORAGE); });
            builder.SetNegativeButton(Resource.String.cancel, (s, e) => { builder.Dispose(); });
            builder.Show();

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if(requestCode == REQUEST_STORAGE)
            {

                if (grantResults.Length > 0 && (grantResults[0] == Permission.Granted))
                {

                    Toast.MakeText(this, "Camera Permisson is already granted", ToastLength.Short).Show();

                }
                else {

                    Toast.MakeText(this, "Permission Denied", ToastLength.Short).Show();

                }
            }
        }

        private void UIReference()
        {
            mybutton = FindViewById<Button>(Resource.Id.button1);
        }
    }
}