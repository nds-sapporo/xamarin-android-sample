using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceSample.Models
{
    public interface IImagePickup : INotifyPropertyChanged
    {
        string ImageUrl { get; }

        void PickUp();
    }
}
