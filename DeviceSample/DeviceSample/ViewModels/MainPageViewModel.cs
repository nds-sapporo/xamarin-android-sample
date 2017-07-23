using System.ComponentModel;
using System.Windows.Input;
using DeviceSample.Commands;
using DeviceSample.Models;
using Xamarin.Forms;

namespace DeviceSample
{
    class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string recognizedText = string.Empty;
        private bool isRecognizing = false;
        private ImageSource imageUrl = null;

        private ISpeechRecognize speechRecognize;
        private IImagePickup imagePickup;

        public ICommand SpeechRecognizeCommand { get; }
        public ICommand PickupImageCommand { get; }

        public string RecognizedText
        {
            get { return recognizedText; }
            protected set
            {
                recognizedText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("RecognizedText"));
            }
        }

        public bool IsRecognizing
        {
            get { return isRecognizing; }
            protected set { isRecognizing = value; }
        }

        public ImageSource ImageUrl
        {
            get { return imageUrl; }
            protected set
            {
                imageUrl = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImageUrl"));
            }
        }

        public MainPageViewModel()
        {
            speechRecognize = DependencyService.Get<ISpeechRecognize>();
            imagePickup = DependencyService.Get<IImagePickup>();

            SpeechRecognizeCommand = new SpeechRecognizeCommand(speechRecognize);
            PickupImageCommand = new PickupImageCommand(imagePickup);

            speechRecognize.PropertyChanged += speechRecognizePropertyChanged;
            imagePickup.PropertyChanged += imagePickupPropertyChanged;
        }

        private void speechRecognizePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "RecognizedText")
            {
                RecognizedText = speechRecognize.RecognizedText;
            }
            if (args.PropertyName == "IsRecognizing")
            {
                IsRecognizing = speechRecognize.IsRecognizing;
            }

        }

        private void imagePickupPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "ImageUrl")
            {
                ImageUrl = ImageSource.FromFile(imagePickup.ImageUrl);
            }
        }
    }
}
