using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magazyn
{
    public class SearchProduct
    {
        private readonly SqliteConnection connection;

        public SearchProduct(SqliteConnection connection)
        {
            this.connection = connection;
        }

        public void searchProduct()
        {
            using (Form searchForm = new Form())
            {
                searchForm.Text = "Szukaj Produktu";
                searchForm.Size = new Size(300, 200);

                Label labelKod = new Label() { Text = "Kod produktu:", Location = new Point(10, 20), AutoSize = true };
                TextBox textBoxKod = new TextBox() { Location = new Point(120, 20), Width = 150 };

                Button buttonSearch = new Button() { Text = "Szukaj", Location = new Point(50, 60), DialogResult = DialogResult.OK };
                Button buttonCancel = new Button() { Text = "Anuluj", Location = new Point(150, 60), DialogResult = DialogResult.Cancel };

                searchForm.Controls.Add(labelKod);
                searchForm.Controls.Add(textBoxKod);
                searchForm.Controls.Add(buttonSearch);
                searchForm.Controls.Add(buttonCancel);

                if (searchForm.ShowDialog() == DialogResult.OK)
                {
                    string kodProduktu = textBoxKod.Text.Trim();

                    if (string.IsNullOrWhiteSpace(kodProduktu))
                    {
                        MessageBox.Show("Kod produktu nie może być pusty.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    FindProductInDatabase(kodProduktu);
                }
            }
        }

        private void FindProductInDatabase(string kodProduktu)
        {
            try
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Produkty WHERE Kod = @Kod";
                SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);
                selectCmd.Parameters.AddWithValue("@Kod", kodProduktu);

                SqliteDataReader reader = selectCmd.ExecuteReader();
                if (reader.Read())
                {
                    string nazwa = reader["Nazwa"].ToString();
                    int ilosc = Convert.ToInt32(reader["Ilość"]);
                    decimal cena = Convert.ToDecimal(reader["Cena"]);

                    MessageBox.Show($"Kod: {kodProduktu}\nNazwa: {nazwa}\nIlość: {ilosc}\nCena: {cena:C2}",
                        "Szczegóły Produktu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Nie znaleziono produktu o podanym kodzie.", "Brak Produktu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy wyszukiwaniu produktu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
