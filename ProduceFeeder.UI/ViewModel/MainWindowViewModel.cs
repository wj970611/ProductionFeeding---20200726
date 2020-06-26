using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProduceFeeder.UI.ViewModel
{
   public class MainWindowViewModel : ViewModelBase
    {

        public string WinTitle   => "ERP的某某某 ";





		private RelayCommand mpsSelectedWindowShow;

		public RelayCommand MpsSelectedWindowShow
		{
			get {
				if (mpsSelectedWindowShow == null)
				{
					return mpsSelectedWindowShow = new RelayCommand(mpsSelectedWindowShowExec);
				}
				return mpsSelectedWindowShow; } 
		}

		private void mpsSelectedWindowShowExec()
		{
			Messenger.Default.Send<string>("null", "MpsSelectedWindowShow");
		}


		private RelayCommand mpsYPWindowShow;

		public RelayCommand MpsYPWindowShow
		{
			get {
				if (mpsYPWindowShow == null)
				{
					return mpsYPWindowShow = new RelayCommand(mpsYPWindowShowExec);
				}
				return mpsYPWindowShow; } 
		}

		private void mpsYPWindowShowExec()
		{
			Messenger.Default.Send<string>("null", "MpsYPWindowShow");
		}


		private RelayCommand mpsTLWindowShow;

		public RelayCommand MpsTLWindowShow
		{
			get {
				if (mpsTLWindowShow ==null)
				{
					return mpsTLWindowShow = new RelayCommand(mpsTLWindowShowExec);
				}
				return mpsTLWindowShow; } 
		}

		private RelayCommand erKeWindowShow;

		public RelayCommand ERKeWindowShow
		{
			get {
				if (erKeWindowShow == null)
				{
					return erKeWindowShow = new RelayCommand(ERKeWindowShowExec);
				}
				return erKeWindowShow; } 
		}

		private void ERKeWindowShowExec()
		{
			 
			Messenger.Default.Send<string>("null", "ERKeWindowShow");
		}

		private void mpsTLWindowShowExec()
		{
			Messenger.Default.Send<string>("null", "MpsTLWindowShow");
		}


		private RelayCommand rclTLWHWindowShow;

		public RelayCommand RCLTLWHWindowShow
		{
			get {
				if (rclTLWHWindowShow == null)
				{
					return rclTLWHWindowShow = new RelayCommand(RCLTLWHWWindowShowExec);
				}
				return rclTLWHWindowShow; } 
		}

		private void RCLTLWHWWindowShowExec()
		{
			Messenger.Default.Send<string>("null", "RCLTLWHWindowShow");
		}


		private RelayCommand mpsQueryWindowShow;

		public RelayCommand MPSQueryWindowShow
		{
			get {
				if (mpsQueryWindowShow == null)
				{
					return mpsQueryWindowShow = new RelayCommand(MPSQueryWindowShowExec);
				}
				return mpsQueryWindowShow; } 
		}

		private void MPSQueryWindowShowExec()
		{
			Messenger.Default.Send<string>("null", "MPSQueryWindowShow");
		}

		private RelayCommand tlQueryWindowShow;

		public RelayCommand TLQueryWindowShow
		{
			get {
				if (tlQueryWindowShow == null)
				{
					return tlQueryWindowShow = new RelayCommand(TLQueryWindowShowExec);
				}
				return tlQueryWindowShow; } 
		}

		private void TLQueryWindowShowExec()
		{
			Messenger.Default.Send<string>("null", "TLQueryWindowShow");
		}
	}
}
