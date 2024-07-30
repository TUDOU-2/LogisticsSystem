using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Shared.Dtos
{
    public class SeaTransportDto : BaseDto
    {
        public SeaTransportDto()
        {
            SeaTransportDetailsList = new ObservableCollection<SeaTransportDetailDto>();
        }

        private ObservableCollection<SeaTransportDetailDto>? _seaTransportDetailsList;
        public ObservableCollection<SeaTransportDetailDto>? SeaTransportDetailsList
        {
            get => _seaTransportDetailsList;
            set
            {
                if (_seaTransportDetailsList != null)
                {
                    _seaTransportDetailsList.CollectionChanged -= OnCollectionChanged;

                    foreach (var item in _seaTransportDetailsList)
                    {
                        item.PropertyChanged -= OnDetailPropertyChanged;
                    }
                }

                _seaTransportDetailsList = value;

                if (_seaTransportDetailsList != null)
                {
                    _seaTransportDetailsList.CollectionChanged += OnCollectionChanged;

                    foreach (var item in _seaTransportDetailsList)
                    {
                        item.PropertyChanged += OnDetailPropertyChanged;
                    }
                }

                OnPropertyChanged(nameof(SeaTransportDetailsList));
            }
        }

        // 列表内的数据发生变化时，更新总数、总重、总体积
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (SeaTransportDetailDto item in e.NewItems)
                {
                    item.PropertyChanged += OnDetailPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (SeaTransportDetailDto item in e.OldItems)
                {
                    item.PropertyChanged -= OnDetailPropertyChanged;
                }
            }
            OnPropertyChanged(nameof(SumCount));
            OnPropertyChanged(nameof(SumWeight));
            OnPropertyChanged(nameof(SumVolume));
        }

        // 详情数据发生变化时，更新总数、总重、总体积
        private void OnDetailPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SeaTransportDetailDto.Count) ||
                e.PropertyName == nameof(SeaTransportDetailDto.Weight) ||
                e.PropertyName == nameof(SeaTransportDetailDto.Volume))
            {
                OnPropertyChanged(nameof(SumCount));
                OnPropertyChanged(nameof(SumWeight));
                OnPropertyChanged(nameof(SumVolume));
            }
        }

        [NotMapped]
        public int SumCount => SeaTransportDetailsList?.Sum(t => t.Count) ?? 0;  // 总数量
        [NotMapped]
        public double SumWeight => SeaTransportDetailsList?.Sum(t => t.Weight) ?? 0;  // 总重量
        [NotMapped]
        public double SumVolume => SeaTransportDetailsList?.Sum(t => t.Volume) ?? 0; // 总体积


        public int? UserId { get; set; }
        public int CustomerId { get; set; }
        public string? Tag { get; set; }
        public string Batch { get; set; } = "N/A";// 批次
        public string OtherDescription { get; set; } = "N/A"; // 其他费用描述
        public string PayDescription { get; set; } = "N/A"; // 实付金额描述
        public DateTime? PayDate { get; set; } // 付款日期


      
        private string note; // 备注
        public string Note
        {
            get { return note; }
            set { note = value;}
        }

        private string targetPlace; // 目的地
        public string TargetPlace
        {
            get { return targetPlace; }
            set { targetPlace = value; OnPropertyChanged(); }
        }

        private string sourcePlace; // 起运港
        public string SourcePlace
        {
            get { return sourcePlace; }
            set { sourcePlace = value; OnPropertyChanged(); }
        }

        private string boxModel; // 柜型
        public string BoxModel
        {
            get { return boxModel; }
            set { boxModel = value; OnPropertyChanged(); }
        }

        private string boxNumber; // 柜号
        public string BoxNumber
        {
            get { return boxNumber; }
            set { boxNumber = value; OnPropertyChanged(); }
        }

        private CustomerDto? customer; // 持有客户
        [NotMapped]
        public CustomerDto? Customer
        {
            get { return customer; }
            set { customer = value; OnPropertyChanged(); }
        }

        private double payMoney; // 已付金额
        public double PayMoney
        {
            get { return payMoney; }
            set { payMoney = value; OnPropertyChanged(); }
        }

        private double price; // 单价
        public double Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged(); CalcNeedPayMoney(); }
        }

        private double otherMoney; // 其他费用
        public double OtherMoney
        {
            get { return otherMoney; }
            set { otherMoney = value; OnPropertyChanged(); CalcNeedPayMoney(); }
        }

        private double needPayMoney; // 应付金额
        [NotMapped]
        public double NeedPayMoney
        {
            get { return needPayMoney; }
            set { needPayMoney = value; OnPropertyChanged(); }
        }

        private DateTime sendData = DateTime.Now; // 出货日期
        public DateTime SendData
        {
            get { return sendData; }
            set { sendData = value; }
        }

        // 计算应付金额
        public void CalcNeedPayMoney()
        {
            NeedPayMoney = Price + OtherMoney;
        }
    }
}
