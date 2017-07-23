using System.ComponentModel;

namespace DeviceSample.Models
{
    public interface ISpeechRecognize : INotifyPropertyChanged
    {
        bool IsRecognizing { get; }
        string RecognizedText { get; }

        void Start();
    }
}
