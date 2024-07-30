using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Shared.Dtos
{
    public class BaseDto : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;


        private int? indexNumber; // 下标编号
        [NotMapped]
        public int? IndexNumber
        {
            get { return indexNumber; }
            set { indexNumber = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        // 实现通知更新
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
