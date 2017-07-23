using DeviceSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeviceSample.Commands
{
    class PickupImageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private IImagePickup imagePickup;

        public PickupImageCommand(IImagePickup imagePickup)
        {
            this.imagePickup = imagePickup;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            imagePickup.PickUp();
        }
    }
}
