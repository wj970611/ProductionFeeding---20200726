using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Models.ItemsContainer;
using ProduceFeeder.UI.Models.K3Items;
using ProduceFeeder.UI.Models.YuPai;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProduceFeeder.UI.ViewModel
{
    public class MPSTLViewModel : ViewModelBase
    {

        public MPSTLViewModel()
        {
            OnDataLoad();
            selectedTLType = TLRunningStatus.等待;
        }

        private RelayCommand feedingCommand;
        private string filterItemString; 
        private string selectedWorkShop;

        public List<MPSTLItem> MPSTLItemsView { get; set; }


        public MPSTLItem SelectedMPSTLItem { get; set; } = new MPSTLItem();

        //public Array StatusSource => System.Enum.GetValues(typeof(FeedingType));    

        public string FilterItemString { get => filterItemString; set { filterItemString = value; OnQueryChanged(); } }

        public string SelectedRclLinev
        {
            get => selectedRclLine;
            set
            {
                selectedRclLine = value; OnQueryChanged();
                selectedWorkShop = string.Empty;
                RaisePropertyChanged("QueryWorkShopView");
                RaisePropertyChanged("SelectedOrderDate");

            }
        }
        private string selectedRclLine;


        private TLRunningStatus selectedTLType;

        public TLRunningStatus SelectedTLType
        {
            get =>selectedTLType;
            set { selectedTLType = value;
                OnQueryChanged();
            }
        }


        private void OnQueryChanged()
        {

            using (MyKingdeeDBContext _context = new MyKingdeeDBContext())
            {
                MPSTLItemsView = new TLScheduleRepository().GetALL().Where(x => x.RunningStatus ==selectedTLType)
                    .Where(x => string.IsNullOrEmpty(filterItemString) || x.SubFNumber.Contains(filterItemString) || x.CPItem.K3FNumber.Contains(filterItemString)) 
                    .OrderByDescending(b => b.XH).ThenBy(b => b.HandleStatus)
                    .ToList();
            }
            RaisePropertyChanged("MPSTLItemsView");
        }

        private void OnDataLoad()
        {
             
                MPSTLItemsView =new TLScheduleRepository().GetALL().Where(x => x.RunningStatus != TLRunningStatus.完成).OrderBy(b => b.HandleStatus).ThenBy(b => b.XH)
                    .ToList();
            
            RaisePropertyChanged("MPSTLItemsView");
        }

        private RelayCommand outFeedingCommand;

        public RelayCommand OutFeedingCommand
        {
            get
            {
                if (outFeedingCommand == null)
                {
                    outFeedingCommand = new RelayCommand(() =>
                    {
                          OutFeedingCommandExec();
                    }, CanOutFeeding);
                }
                return outFeedingCommand;
            }
        }

        private bool CanOutFeeding()
        {

            return SelectedMPSTLItem == null || (SelectedMPSTLItem.FeedingDate.HasValue && !SelectedMPSTLItem.OutFeedingDate.HasValue);
        }

        /// <summary>
        /// 热处理完成 出料
        /// </summary>
        /// <returns></returns>
        private  void  OutFeedingCommandExec()
        {
            MPSTLContainer.Update(SelectedMPSTLItem);
        }

        public RelayCommand FeedingCommand
        {
            get
            {
                if (feedingCommand == null)
                {
                    feedingCommand = new RelayCommand(FeedingCommandExec, CanFeeding);
                }
                return feedingCommand;
            }
        }

        private bool CanFeeding() => SelectedMPSTLItem == null || !SelectedMPSTLItem.FeedingDate.HasValue;

        private void FeedingCommandExec()
        {
            SelectedMPSTLItem.FeedingDate = DateTime.Now;
        }



        private RelayCommand uCommand;
        public RelayCommand UCommand
        {
            get
            {
                if (uCommand == null)
                {
                    uCommand = new RelayCommand(uCommandExec, CanuFeeding);
                }
                return uCommand;
            }
        }

        private bool CanuFeeding()
        {
            return true;
        }

        private void uCommandExec()
        {
            //new TLScheduleRepository().UpdateQty(SelectedMPSTLItem);
        }




        private RelayCommand refreshCommand;

        public RelayCommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new RelayCommand(RefreshCommandExec);
                }
                return refreshCommand;
            }
        }
        private void RefreshCommandExec()
        {
            OnQueryChanged();
        }



        private RelayCommand exportExcelCommand;

        public RelayCommand ExportExcelCommand
        {
            get
            {
                if (exportExcelCommand == null)
                {
                    exportExcelCommand = new RelayCommand(ExportExcelCommandExec);
                }
                return exportExcelCommand;
            }
        }

        private void ExportExcelCommandExec()
        {
            DataTable _dt = new DataTable();
            _dt.Columns.Add("目标型号");
            _dt.Columns.Add("制造车间");
            _dt.Columns.Add("投料编码");
            _dt.Columns.Add("投料名称");
            _dt.Columns.Add("大小圈");
            _dt.Columns.Add("计划数量");
            _dt.Columns.Add("投料数量"); 
            _dt.Columns.Add("要求投料时间");
            _dt.Columns.Add("炉线"); 
            _dt.Columns.Add("投料时间"); 
            _dt.Columns.Add("出料时间"); 
            _dt.Columns.Add("投料人"); 
            _dt.Columns.Add("备注"); 
            _dt.Columns.Add("运行状态"); 
            _dt.Columns.Add("投料状态");
            foreach (var item in MPSTLItemsView)
            { 
                DataRow _dr = _dt.NewRow();
                _dr[0] = item.CPItem.K3FNumber;
                _dr[1] = item.CPItem.DepFName;
                _dr[2] = item.SubFNumber ;
                _dr[3] = item.SubFName ;
                _dr[4] = item.DXQ ;
                _dr[5] = item.OnePlanQTY ;
                _dr[6] = item.Qty ;
                _dr[7] = item. OrderDate.ToString();
                _dr[8] = item.RclLine;
                _dr[9] = item.FeedingDate.ToString();
                _dr[10] = item.OutFeedingDate.ToString();
                _dr[11] = item.Feeder;
                _dr[12] = item.Remark;
                _dr[13] = item.RunningStatus;
                _dr[14] = item.FeedingType;
                _dt.Rows.Add(_dr);
            }
            SaveFileDialog _dialog = new SaveFileDialog();
            _dialog.DefaultExt = ".xlsx";
            _dialog.Filter = "Excel 文档 (.xlsx)| *.xlsx";
            var _result = _dialog.ShowDialog();
            if (_result == true)
            {

                var _xls = new WJJ.PF.Infranstructure.Data.Tools.ExcelHelper(_dialog.FileName);
                _xls.DataTableToExcel(_dt, "Sheet1", true);

                MessageBox.Show("导出成功!", "", MessageBoxButton.OK, MessageBoxImage.Information);


            }
        }

    }
}
