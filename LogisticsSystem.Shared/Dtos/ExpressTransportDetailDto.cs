using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Shared.Dtos
{
    public class ExpressTransportDetailDto : BaseDto
    {
        public bool? IsNew { get; set; }
        public bool IsModified { get; set; }


        public int UserId { get; set; }
        public int ExpressTransportId { get; set; }
        public string Productor { get; set; }
        public DateTime? ReceiveDate { get; set; }
        public string? Tag { get; set; }
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

        private double volume;
        public double Volume
        {
            get { return volume; }
            set { volume = value; OnPropertyChanged(); }
        }
    }
}
