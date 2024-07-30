using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Shared.Dtos
{
    public class AirTransportDetailDto : BaseDto
    {
        public bool? IsNew { get; set; }
        public bool IsModified { get; set; }


        public int AirTransportId { get; set; }
        public int? UserId { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? Note { get; set; }



        private double weight;
        public double Weight
        {
            get { return weight; }
            set { weight = value; OnPropertyChanged(); IsModified = true; }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; OnPropertyChanged(); IsModified = true; }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set { height = value; OnPropertyChanged(); CalcVolume(); IsModified = true; }
        }

        private double width;
        public double Width
        {
            get { return width; }
            set { width = value; OnPropertyChanged(); CalcVolume(); IsModified = true; }
        }

        private double length;
        public double Length
        {
            get { return length; }
            set { length = value; OnPropertyChanged(); CalcVolume(); IsModified = true; }
        }

        private double volume;
        public double Volume
        {
            get { return volume; }
            set { volume = value; OnPropertyChanged(); }
        }

        private string? tag;
        public string? Tag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged(); IsModified = true; }
        }

        public void CalcVolume()
        {
            Volume = Height * Width * Length / 1000000;

        }
    }
}
