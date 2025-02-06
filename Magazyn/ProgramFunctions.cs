using Microsoft.Data.Sqlite;
using System;
using System.Windows.Forms;

namespace Magazyn
{
    public class ProgramFunctions
    {
        private readonly SqliteConnection connection;
        private readonly ListView listViewProdukty;

        public ProgramFunctions(SqliteConnection connection, ListView listViewProdukty)
        {
            this.connection = connection;
            this.listViewProdukty = listViewProdukty;
        }

        public void CreateTable()
        {
            try
            {
                connection.Open();
                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Produkty (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Kod TEXT UNIQUE,
                    Nazwa TEXT,
                    Ilość INTEGER,
                    Cena DECIMAL(10,2)
                )";
                SqliteCommand cmd = new SqliteCommand(createTableQuery, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy tworzeniu tabeli: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        public void DisplayDataInListView()
        {
            try
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Produkty";
                SqliteCommand cmd = new SqliteCommand(selectQuery, connection);
                SqliteDataReader reader = cmd.ExecuteReader();
                listViewProdukty.Items.Clear();
                while (reader.Read())
                {
                    ListViewItem item = new ListViewItem(reader["Id"].ToString());
                    item.SubItems.Add(reader["Kod"].ToString());
                    item.SubItems.Add(reader["Nazwa"].ToString());
                    item.SubItems.Add(reader["Ilość"].ToString());
                    item.SubItems.Add(decimal.Parse(reader["Cena"].ToString()).ToString("F2", new System.Globalization.CultureInfo("pl-PL")));
                    listViewProdukty.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy wyświetlaniu danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
