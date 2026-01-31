using Exchange.Data;

using FZCore;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Exchange.Controls;

/// <summary>
/// Representing a currency exchange calculator control.
/// </summary>
public partial class CtlExchangeCalculator : UserControl
{
    /// <summary>
    /// Creates a new <see cref="CtlExchangeCalculator"/> instance.
    /// </summary>
    public CtlExchangeCalculator()
    {
        InitializeComponent();
    }

    private static Key[] AllowedKeys => [Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9,
                                         Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
                                         Key.Left, Key.Right, Key.Back, Key.OemPeriod, Key.OemComma, Key.Decimal];

    private ExchangeReport? _report = null;

    private async Task ClearFields()
    {
        await ReloadUI();
    }

    private async Task ReloadUI()
    {
        if (_report.HasValue == false)
        {
            // fetch only if the report is null
            _report = await ExchangeReport.FetchAsync();
            if (_report.HasValue == false)
            {
                Log.Error("Unable to fetch the currency report.", nameof(ReloadUI));
                FZCore.Core.ErrorBox("Unable to fetch the currency report.");
                return;
            }
        }

        cbxInput.Items.Clear();
        cbxOutput.Items.Clear();
        txtResult.Clear();

        foreach (CurrencyInfo currency in _report.Value.Currencies)
        {
            string header = $"{currency.Currency} ({currency.Country})";
            ComboBoxItem cbiIn = new ComboBoxItem
            {
                Tag = currency,
                Uid = currency.Code,
                Content = header
            };

            ComboBoxItem cbiOut = new ComboBoxItem
            {
                Tag = currency,
                Uid = currency.Code,
                Content = header
            };

            cbxInput.Items.Add(cbiIn);
            cbxOutput.Items.Add(cbiOut);
        }

        if (cbxInput.Items.Count > 0) cbxInput.SelectedIndex = 0;
        if (cbxOutput.Items.Count > 0) cbxOutput.SelectedIndex = 0;
    }

    private async Task<bool> TryConvert()
    {
        try
        {
            if (cbxInput.SelectedItem is ComboBoxItem cbiIn && cbiIn.Tag is CurrencyInfo cIn &&
                cbxOutput.SelectedItem is ComboBoxItem cbiOut && cbiOut.Tag is CurrencyInfo cOut)
            {
                // 1. get the amount
                if (decimal.TryParse(txtAmount.Text, out decimal inputAmount))
                {
                    // 2. calculate the rate
                    decimal unitRateIn = cIn.Rate / cIn.Amount;
                    decimal unitRateOut = cOut.Rate / cOut.Amount;

                    // 3. convert
                    decimal result = (inputAmount * unitRateIn) / unitRateOut;

                    // 4. display
                    txtAmount.Text = inputAmount.ToString("N2");
                    txtResult.Text = result.ToString("N2");
                }
                else
                {
                    FZCore.Core.InfoBox("Please enter a valid number.");
                    return false;
                }
            }

            return true;
        }

        catch (Exception ex)
        {
            Log.Error(ex, nameof(TryConvert));
            return false;
        }
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await ReloadUI();
    }

    private async void btnConvert_Click(object sender, RoutedEventArgs e)
    {
        if (await TryConvert() == false)
        {
            App.MainWindow.SetStatusMessage("Unable to perform this conversion.");
        }
    }

    private async void btnClear_Click(object sender, RoutedEventArgs e)
    {
        await ClearFields();
    }

    private void txtAmount_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // ignore all non numeric characters

        if (AllowedKeys.Contains(e.Key) == false)
        {
            e.Handled = true;
            return;
        }
    }
}
