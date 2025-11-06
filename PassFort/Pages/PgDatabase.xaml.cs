using FZCore;

using PassFort.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace PassFort.Pages;

/// <summary>
/// Representing the page with info about the loaded password database.
/// This page also contains functionality to adding and removing password entries (<see cref="PasswordEntry"/>.
/// </summary>
public partial class PgDatabase : Page
{
    /// <summary>
    /// Creates a new instance of the <see cref="PgDatabase"/> class.
    /// /// </summary>
    public PgDatabase()
    {
        InitializeComponent();
        this.Loaded += async (s, e) =>
        {
            await RefreshEntriesAsync();
        };
    }

    private Task<List<PasswordEntry>> GetEntriesAsync()
    {
        return Task<PasswordEntry>.Run(() =>
        {
            if (PasswordDatabase.Current == null)
            {
                Log.Error(Messages.NO_DB_OPENED, nameof(GetEntriesAsync));
                return [];
            }

            // create a list of items
            List<PasswordEntry> items = [];

            // iterate through all password entries
            foreach (PasswordEntry entry in PasswordDatabase.Current.Entries)
            {
                items.Add(entry);
            }

            return items;
        });
    }

    private void OpenEntryDetails(Guid? entryId)
    {
        // access check
        if (entryId.HasValue == false) return;
        if (PasswordDatabase.Current == null) return;

        // get entry
        PasswordEntry? entry = PasswordDatabase.Current.Entries.Where(x => x.Id == entryId).FirstOrDefault();
        if (entry == null) return;

        // manipulate entry
        // display entry info
        _ = MessageBox.Show($"Title: {entry.Title}\nPassword: {entry.Password}", "View Entry", MessageBoxButton.OK, MessageBoxImage.Information);

        return;
    }

    private async Task<bool> RefreshEntriesAsync()
    {
        if (PasswordDatabase.Current == null)
        {
            Log.Error(Messages.NO_DB_OPENED, nameof(RefreshEntriesAsync));
            return false;
        }

        List<PasswordEntry> items = await GetEntriesAsync();

        lbEntries.Items.Clear();
        foreach (PasswordEntry item in items)
        {
            ListBoxItem lbi = new ListBoxItem
            {
                Tag = item.Id,
                Content = new Label
                {
                    Content = new TextBlock
                    {
                        Inlines =
                        {
                            new Run(item.Title)
                            {
                                FontSize = 18
                            },

                            new LineBreak(),

                            new Run($"{item.CreationTime.ToShortDateString()} {item.CreationTime.ToShortTimeString()}")
                            {

                            }
                        }
                    }
                }
            };

            lbi.MouseDoubleClick += (s, e) => OpenEntryDetails(item.Id);
            lbEntries.Items.Add(lbi);
        }

        return true;
    }

    private async void miRefresh_Click(object sender, RoutedEventArgs e)
    {
        await RefreshEntriesAsync();
    }
}
