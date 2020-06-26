using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GongSolutions.Wpf.DragDrop;
using ProduceFeeder.UI.Models;
using ProduceFeeder.UI.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProduceFeeder.UI.ViewModel
{
    public class RCLTLWHViewModel : ViewModelBase, IDropTarget
    {

        public RCLTLWHViewModel()
        {
            DataLoad();
        }

        private void DataLoad()
        {
            mpsTLItemsList = new Repository.TLScheduleRepository().GetALL()
                .Where(w => w.RunningStatus != TLRunningStatus.完成).OrderByDescending(b => b.XH).ToList();

            for (int i = 0; i < mpsTLItemsList.Count; i++)
            {
                mpsTLItemsList[i].XH = i + 1;
            }
            MPSTLItemsView = new ObservableCollection<MPSTLItem>(mpsTLItemsList);
        }

        public string Title => "热处理投料维护";

        private List<MPSTLItem> mpsTLItemsList = new List<MPSTLItem>();
        private ObservableCollection<MPSTLItem> mPSTLItemsView;

        public ObservableCollection<MPSTLItem> MPSTLItemsView
        { get => mPSTLItemsView; set
            {
                mPSTLItemsView = value;
                RaisePropertyChanged("MPSTLItemsView");
            }
        }

        public MPSTLItem SelectedMPSTLItem { get; set; } = new MPSTLItem();

        public Array StatusSource => System.Enum.GetValues(typeof(FeedingType));
        public List<string> QueryWorkShopView { get; set; }

        public void DragOver(IDropInfo dropInfo)
        {
            MPSTLItem sourceItem = dropInfo.Data as MPSTLItem;
            MPSTLItem targetItem = dropInfo.TargetItem as MPSTLItem;

            if (sourceItem != null && targetItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Move;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            MPSTLItem sourceItem = dropInfo.Data as MPSTLItem;
            MPSTLItem targetItem = dropInfo.TargetItem as MPSTLItem;
            var targetindex = MPSTLItemsView.IndexOf(targetItem);
            var sourcetindex = MPSTLItemsView.IndexOf(sourceItem);
            MPSTLItemsView.RemoveAt(sourcetindex);
            MPSTLItemsView.Insert(targetindex, sourceItem);
            for (int i = 0; i < mPSTLItemsView.Count; i++)
            {
                mPSTLItemsView[i].XH = i + 1;
            }

            cansave = true;
        }
        //public FeedingType SelectedFeedingType { get => selectedFeedingType; set { selectedFeedingType = value; OnQueryChanged(); } }



        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(SaveCommandExec, CanSave);
                }
                return saveCommand;
            }
        }
        private bool cansave = false;
        private bool CanSave()
        {
            return cansave;
        }

        private void SaveCommandExec()
        {
            foreach (var item in MPSTLItemsView)
            {
                new Repository.TLScheduleRepository().Update(item);
            }
            OnUpdate();
        }

        private string iconPath = "image/flag_red.png";

        public string IconPath { get => iconPath; private set { iconPath = value; RaisePropertyChanged("IconPath"); } }



        private RelayCommand redFlagCommand;
        public RelayCommand RedFlagCommand
        {
            get
            {
                if (redFlagCommand == null)
                {
                    redFlagCommand = new RelayCommand(RedFlagCommandExec);
                }
                return redFlagCommand;
            }
        }
        private void RedFlagCommandExec()
        {
            SelectedMPSTLItem.HandleStatus = TLhandleStatus.Urgent;
            OnUpdate();
        }

        private RelayCommand blueFlagCommand;
        public RelayCommand BlueFlagCommand
        {
            get
            {
                if (blueFlagCommand == null)
                {
                    blueFlagCommand = new RelayCommand(BlueFlagCommandExec);
                }
                return blueFlagCommand;
            }
        }
        private void BlueFlagCommandExec()
        {
            SelectedMPSTLItem.HandleStatus = TLhandleStatus.Normal;
            OnUpdate();
        }

        private RelayCommand yellowFlagCommand;
        public RelayCommand YellowFlagCommand
        {
            get
            {
                if (yellowFlagCommand == null)
                {
                    yellowFlagCommand = new RelayCommand(YellowFlagCommandExec);
                }
                return yellowFlagCommand;
            }
        }
        private void YellowFlagCommandExec()
        {
            SelectedMPSTLItem.HandleStatus = TLhandleStatus.Delay;
            OnUpdate();
        }

        private void OnUpdate()
        {
            cansave = true;
        }


        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                if (deleteCommand == null)
                {
                    deleteCommand = new RelayCommand(DeleteCommandExec, CanDelete);
                }
                return deleteCommand;
            }
        }

        private bool CanDelete()
        {
            return SelectedMPSTLItem != null;
        }

        private void DeleteCommandExec()
        {
            NotificationMessageAction<MessageBoxResult> msg = new NotificationMessageAction<MessageBoxResult>("", (ret) =>
            {
                if (ret == MessageBoxResult.OK)
                {
                    if (new TLScheduleRepository().UserDelete(SelectedMPSTLItem) > 0)
                    {
                        MPSTLItemsView.Remove(SelectedMPSTLItem);
                    }
                }
            }
           );

            Messenger.Default.Send<NotificationMessageAction<MessageBoxResult>>(msg);
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
            DataLoad();
        }

    }
}
