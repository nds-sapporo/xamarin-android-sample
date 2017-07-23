using System;
using System.Windows.Input;
using DeviceSample.Models;

namespace DeviceSample.Commands
{
    class SpeechRecognizeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ISpeechRecognize speechRecognize;

        public SpeechRecognizeCommand(ISpeechRecognize speechRecognize)
        {
            this.speechRecognize = speechRecognize;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            speechRecognize.Start();
        }
    }
}
