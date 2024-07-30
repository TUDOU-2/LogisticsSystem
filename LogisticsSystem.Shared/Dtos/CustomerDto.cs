using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Shared.Dtos
{
    public class CustomerDto : BaseDto
    {
        private int? userId { get; set; }
        public int? UserId
        {
            get { return userId; }
            set { userId = value; OnPropertyChanged(); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private string telephone;
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; OnPropertyChanged(); }
        }

        private string nation;
        public string Nation
        {
            get { return nation; }
            set { nation = value; OnPropertyChanged(); }
        }

        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; OnPropertyChanged(); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }

        private string? tag;
        public string? Tag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged(); }
        }
    }
}
