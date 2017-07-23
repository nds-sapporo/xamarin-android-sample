using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using DeviceSample.Models;
using DeviceSample.Droid.Models;
using Xamarin.Forms;
using Android.Preferences;
using Android.Database;
using Android.Provider;

[assembly: Dependency(typeof(PickupImage))]
namespace DeviceSample.Droid.Models
{
    class PickupImage : INotifyPropertyChanged, IImagePickup
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string imageUrl;

        private MainActivity mainActivity;

        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImageUrl"));
            }
        }

        public PickupImage()
        {
            // 音声認識のアクティビティで取得した結果をハンドルする処理をMainActivityに付ける。
            mainActivity = Forms.Context as MainActivity;
            mainActivity.ActivityResult += HandleActivityResult;
        }

        // 画像選択のアクティビティで取得した結果をハンドルする処理の本体
        private void HandleActivityResult(object sender, PreferenceManager.ActivityResultEventArgs args)
        {
            if (args.RequestCode == MainActivity.IMAGE_REQUEST_CODE)
            {
                if (args.ResultCode == Result.Ok)
                {
                    ICursor cursor = mainActivity.ContentResolver.Query(args.Data.Data, null, null, null, null);
                    cursor.MoveToFirst();
                    string document_id = cursor.GetString(0);
                    if (document_id.Contains(":"))
                        document_id = document_id.Split(':')[1];
                    cursor.Close();

                    cursor = mainActivity.ContentResolver.Query(
                    Android.Provider.MediaStore.Images.Media.ExternalContentUri,
                    null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new String[] { document_id }, null);
                    cursor.MoveToFirst();
                    string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
                    cursor.Close();

                    ImageUrl = path;
                }
            }
        }

        public void PickUp()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            mainActivity.StartActivityForResult(Intent.CreateChooser(intent, "画像を選択"), MainActivity.IMAGE_REQUEST_CODE);
        }
    }
}