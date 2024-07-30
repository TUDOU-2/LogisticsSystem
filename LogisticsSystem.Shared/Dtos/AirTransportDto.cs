using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogisticsSystem.Shared.Dtos
{
    public class AirTransportDto : BaseDto
    {
        public AirTransportDto()
        {
            AirTransportDetailsList = new ObservableCollection<AirTransportDetailDto>();
        }

        private ObservableCollection<AirTransportDetailDto>? _airTransportDetailsList;
        public ObservableCollection<AirTransportDetailDto>? AirTransportDetailsList
        {
            get => _airTransportDetailsList;
            set
            {
                if (_airTransportDetailsList != null)
                {
                    _airTransportDetailsList.CollectionChanged -= OnCollectionChanged;

                    foreach (var item in _airTransportDetailsList)
                    {
                        item.PropertyChanged -= OnDetailPropertyChanged;
                    }
                }

                _airTransportDetailsList = value;

                if (_airTransportDetailsList != null)
                {
                    _airTransportDetailsList.CollectionChanged += OnCollectionChanged;

                    foreach (var item in _airTransportDetailsList)
                    {
                        item.PropertyChanged += OnDetailPropertyChanged;
                    }
                }

                OnPropertyChanged(nameof(AirTransportDetailsList));
                UpdateTag();
            }
        }

        // 列表内的数据发生变化时，更新总数、总重、总体积、总密度
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (AirTransportDetailDto item in e.NewItems)
                {
                    item.PropertyChanged += OnDetailPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (AirTransportDetailDto item in e.OldItems)
                {
                    item.PropertyChanged -= OnDetailPropertyChanged;
                }
            }
            OnPropertyChanged(nameof(SumCount));
            OnPropertyChanged(nameof(SumWeight));
            OnPropertyChanged(nameof(SumVolume));
            OnPropertyChanged(nameof(SumDensity));
            OnPropertyChanged(nameof(Tag));
            UpdateTag();
        }

        // 详情数据发生变化时，更新总数、总重、总体积、总密度
        private void OnDetailPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AirTransportDetailDto.Count) ||
                e.PropertyName == nameof(AirTransportDetailDto.Weight) ||
                e.PropertyName == nameof(AirTransportDetailDto.Volume))
            {
                OnPropertyChanged(nameof(SumCount));
                OnPropertyChanged(nameof(SumWeight));
                OnPropertyChanged(nameof(SumVolume));
                OnPropertyChanged(nameof(SumDensity));
                OnPropertyChanged(nameof(Tag));
                UpdateTag();
            }
        }


        [NotMapped]
        public int SumCount => AirTransportDetailsList?.Sum(t => t.Count) ?? 0;  // 总数量
        [NotMapped]
        public double SumWeight => AirTransportDetailsList?.Sum(t => t.Weight) ?? 0;  // 总重量
        [NotMapped]
        public double SumVolume => AirTransportDetailsList?.Sum(t => t.Volume) ?? 0; // 总体积
        [NotMapped]
        public double SumDensity => SumWeight > 0 && SumVolume > 0 ? SumWeight / SumVolume : 0; // 总密度


        public int? UserId { get; set; }
        public int CustomerId { get; set; }
        public string? Batch { get; set; } = "N/A";// 批次
        public string? Note { get; set; }
        public string OtherDescription { get; set; } = "N/A"; // 其他费用描述
        public string PayDescription { get; set; } = "N/A"; // 实付金额描述
        public DateTime? PayDate { get; set; } // 付款日期


        private string sourcePlace; // 起运港
        public string SourcePlace
        {
            get { return sourcePlace; }
            set { sourcePlace = value; OnPropertyChanged(); }
        }

        private string targetPlace; // 目的地
        public string TargetPlace
        {
            get { return targetPlace; }
            set { targetPlace = value; OnPropertyChanged(); }
        }

        private DateTime? sendData = DateTime.Now; // 出货日期
        public DateTime? SendData
        {
            get { return sendData; }
            set { sendData = value; OnPropertyChanged(); }
        }

        private CustomerDto? customer; // 持有客户
        [NotMapped]
        public CustomerDto? Customer
        {
            get { return customer; }
            set { customer = value; OnPropertyChanged(); }
        }

        private string orderNumber; // 订单号
        public string OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; OnPropertyChanged(); }
        }

        private string? tag;
        public string? Tag
        {
            get { return tag; }
            set { tag = value; OnPropertyChanged(); }
        }

        private double price; // 单价
        public double Price
        {
            get { return price; }
            set { price = value; CalcNeedPayMoney(); OnPropertyChanged(); }
        }

        private double calcWeight; // 计费重量
        public double CalcWeight
        {
            get { return calcWeight; }
            set { calcWeight = value; CalcNeedPayMoney(); OnPropertyChanged(); }
        }

        private double needPayMoney; // 应付总金额
        [NotMapped]
        public double NeedPayMoney
        {
            get { return needPayMoney; }
            set { needPayMoney = value; OnPropertyChanged(); }
        }

        private double payMoney; // 已付金额
        public double PayMoney
        {
            get { return payMoney; }
            set { payMoney = value; OnPropertyChanged(); }
        }

        private double otherMoney; // 其他费用
        public double OtherMoney
        {
            get { return otherMoney; }
            set { otherMoney = value; OnPropertyChanged(); CalcNeedPayMoney(); }
        }

        // 计算应付总金额
        public void CalcNeedPayMoney()
        {
            NeedPayMoney = CalcWeight * Price + OtherMoney;
        }

        // 自动生成标签
        public void UpdateTag()
        {
            StringBuilder tagBuilder = new StringBuilder();
            if (AirTransportDetailsList != null)
            {
                foreach (var item in AirTransportDetailsList)
                {
                    tagBuilder.Append($"{item.Length}*{item.Width}*{item.Height}/{item.Count}\r");
                }
            }
            Tag = tagBuilder.ToString();
        }
    }
}
