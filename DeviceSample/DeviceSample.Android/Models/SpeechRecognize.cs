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
using Android.Speech;

using DeviceSample.Models;
using DeviceSample.Droid.Models;
using Xamarin.Forms;

using Android.Preferences;

[assembly: Dependency(typeof(SpeechRecognize))]
namespace DeviceSample.Droid.Models
{
    public class SpeechRecognize : INotifyPropertyChanged, ISpeechRecognize
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #region メンバ変数

        private bool isRecognizing;

        private string recognizedText = string.Empty;

        private MainActivity mainActivity;

        public bool IsRecognizing
        {
            get { return isRecognizing; }
            protected set
            {
                isRecognizing = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsRecognizing"));
            }
        }

        public string RecognizedText
        {
            get { return recognizedText; }
            protected set
            {
                recognizedText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecognizedText"));
            }
        }

        #endregion

        #region 定数
        
        private readonly int INTERVAL_MILLISEC = 1500;

        #endregion

        public SpeechRecognize()
        {
            // 音声認識のアクティビティで取得した結果をハンドルする処理をMainActivityに付ける。
            mainActivity = Forms.Context as MainActivity;
            mainActivity.ActivityResult += HandleActivityResult;
        }

        // 音声認識のアクティビティで取得した結果をハンドルする処理の本体
        private void HandleActivityResult(object sender, PreferenceManager.ActivityResultEventArgs args)
        {
            if (args.RequestCode == MainActivity.SPEECH_REQUEST_CODE)
            {
                IsRecognizing = false;
                if (args.ResultCode == Result.Ok)
                {
                    // 認識が成功した場合、認識結果の文字列を引き出し、RecognizedTextに入れる。
                    var matches = args.Data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        System.Diagnostics.Debug.WriteLine("[音声認識成功]:" + matches[0]);
                        RecognizedText = matches[0];
                    }
                }
            }
        }

        public void Start()
        {
            RecognizedText = string.Empty;
            IsRecognizing = true;

            try
            {
                // 音声認識のアクティビティを呼び出すためのインテントを用意する。
                var intent = new Intent(RecognizerIntent.ActionRecognizeSpeech);

                // 諸々のプロパティを設定する。
                intent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                intent.PutExtra(RecognizerIntent.ExtraPrompt, "何か話しかけてみてください。");
                intent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, INTERVAL_MILLISEC);
                intent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, INTERVAL_MILLISEC);
                intent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, INTERVAL_MILLISEC);
                intent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                // 認識言語の指定。端末の設定言語(Java.Util.Locale.Default)で音声認識を行う。
                intent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

                // 音声認識のアクティビティを開始する。
                mainActivity.StartActivityForResult(intent, MainActivity.SPEECH_REQUEST_CODE);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        
    }
}