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
    public class ExpressTransportDto : BaseDto
    {
        public List<string> Channels
        {
            get
            {
                return new List<string> { "DHL", "FeDex", "TNT", "UPS", "专线", "其他" };
            }
        }

        public ExpressTransportDto()
        {
            ExpressTransportDetailsList = new ObservableCollection<ExpressTransportDetailDto>();
        }

        private ObservableCollection<ExpressTransportDetailDto>? _expressTransportDetailsList;
        public ObservableCollection<ExpressTransportDetailDto>? ExpressTransportDetailsList
        {
            get => _expressTransportDetailsList;
            set
            {
                if (_expressTransportDetailsList != null)
                {
                    _expressTransportDetailsList.CollectionChanged -= OnCollectionChanged;

                    foreach (var item in _expressTransportDetailsList)
                    {
                        item.PropertyChanged -= OnDetailPropertyChanged;
                    }
                }

                _expressTransportDetailsList = value;

                if (_expressTransportDetailsList != null)
                {
                    _expressTransportDetailsList.CollectionChanged += OnCollectionChanged;

                    foreach (var item in _expressTransportDetailsList)
                    {
                        item.PropertyChanged += OnDetailPropertyChanged;
                    }
                }

                OnPropertyChanged(nameof(ExpressTransportDetailsList));
            }
        }

        // 列表内的数据发生变化时，更新总数、总重、总体积
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ExpressTransportDetailDto item in e.NewItems)
                {
                    item.PropertyChanged += OnDetailPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (ExpressTransportDetailDto item in e.OldItems)
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
            if (e.PropertyName == nameof(ExpressTransportDetailDto.Count) ||
                e.PropertyName == nameof(ExpressTransportDetailDto.Weight) ||
                e.PropertyName == nameof(ExpressTransportDetailDto.Volume))
            {
                OnPropertyChanged(nameof(SumCount));
                OnPropertyChanged(nameof(SumWeight));
                OnPropertyChanged(nameof(SumVolume));
            }
        }

        [NotMapped]
        public int SumCount => ExpressTransportDetailsList?.Sum(t => t.Count) ?? 0;  // 总数量
        [NotMapped]
        public double SumWeight => ExpressTransportDetailsList?.Sum(t => t.Weight) ?? 0;  // 总重量
        [NotMapped]
        public double SumVolume => ExpressTransportDetailsList?.Sum(t => t.Volume) ?? 0; // 总体积

        public int? UserId { get; set; }
        public int CustomerId { get; set; }
        public string Channel { get; set; } = "其他"; // 渠道
        public string? Tag { get; set; }
        public string OtherDescription { get; set; } = "N/A"; // 其他费用描述
        public string PayDescription { get; set; } = "N/A"; // 实付金额描述
        public DateTime? PayDate { get; set; } // 付款日期


        private string orderNumber; // 订单号
        public string OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; OnPropertyChanged(); }
        }

        private string note; // 备注
        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        private DateTime sendData = DateTime.Now; // 出货日期
        public DateTime SendData
        {
            get { return sendData; }
            set { sendData = value; }
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

        private CustomerDto? customer; // 持有客户
        [NotMapped]
        public CustomerDto? Customer
        {
            get { return customer; }
            set { customer = value; OnPropertyChanged(); }
        }

        private double needPayMoney = 0; // 应付总金额
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

        private double otherMoney; // 其他费用
        public double OtherMoney
        {
            get { return otherMoney; }
            set { otherMoney = value; OnPropertyChanged(); CalcNeedPayMoney(); }
        }

        public void CalcNeedPayMoney()
        {
            this.NeedPayMoney = CalcWeight * Price + OtherMoney;
        }
    }
}
