using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI_TASK_5.ViewModel
{
    public partial class MainViewModel : INotifyPropertyChanged
    {
        private string screenVal;
        private string calculationVal;
        private readonly List<string> availableOperations = new() { "+", "-", "/", "*" };
        private readonly DataTable dataTable = new();
        private bool isLastSignAnOperation;
        public event PropertyChangedEventHandler PropertyChanged;
        static readonly string pattern = "[+-/*]";
        readonly Regex rgx = new Regex(pattern);

        public ICommand AddNumberCommand { get; private set; }
        public ICommand AddOperationCommand { get; private set; }
        public ICommand ClearScreenCommand { get; private set; }
        public ICommand GetResultCommand { get; private set; }
        public ICommand OnNegativeCommand { get; private set; }
        public ICommand OnPercentageCommand { get; private set; }

        public MainViewModel()
        {
            CalculationDisplay = "";
            ResultDisplay = "0";
            AddNumberCommand = new RelayCommand(AddNumber);
            AddOperationCommand = new RelayCommand(AddOperation, CanAddOperation);
            ClearScreenCommand = new RelayCommand(ClearScreen);
            GetResultCommand = new RelayCommand(GetResult, CanGetResult);
            OnNegativeCommand = new RelayCommand(OnNegative);
            OnPercentageCommand = new RelayCommand(OnPercentage);
        }


        private bool CanGetResult(object obj) => !isLastSignAnOperation;
        private bool CanAddOperation(object obj) => !isLastSignAnOperation;


        private void OnNegative(object obj)
        {
            if (ResultDisplay.Contains("-") || ResultDisplay == "0")
            {
                ResultDisplay = ResultDisplay.Remove(ResultDisplay.IndexOf("-"), 1);
            }
            else ResultDisplay = "-" + ResultDisplay;
        }

        private void OnPercentage(object obj)
        {
            CalculationDisplay = ResultDisplay + "* 0,01";
            ResultDisplay = (Convert.ToDouble(ResultDisplay) * 0.01).ToString();
        }


        private void GetResult(object obj)
        {
            try
            {
                var result = Math.Round(Convert.ToDouble(dataTable.Compute(ResultDisplay.Replace(",", "."), "")), 2);
                ResultDisplay = result.ToString();
                CalculationDisplay = calculationVal;
            }
            catch (Exception)
            {
                var result = rgx.Replace(ResultDisplay, "");
                ResultDisplay = result.ToString();
            }
        }

        private void ClearScreen(object obj)
        {
            ResultDisplay = "0";
            CalculationDisplay = "";
            isLastSignAnOperation = false;
        }

        private void AddOperation(object obj)
        {
            var operation = obj as string;

            ResultDisplay += operation;

            calculationVal += operation;

            isLastSignAnOperation = true;
        }

        private void AddNumber(object obj)
        {
            var number = obj as string;

            if (ResultDisplay == "0" && number != ",")
                ResultDisplay = string.Empty;
            else if (number == "," && availableOperations.Contains(ResultDisplay.Substring(ResultDisplay.Length - 1)))
                number = "0,";

            ResultDisplay += number;

            calculationVal += number;

            isLastSignAnOperation = false;
        }

        public string CalculationDisplay
        {
            get { return calculationVal; }
            set
            {
                calculationVal = value;
                OnPropertyChanged();
            }
        }

        public string ResultDisplay
        {
            get { return screenVal; }
            set
            {
                screenVal = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
