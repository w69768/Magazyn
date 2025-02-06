using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magazyn
{
    public class EditProduct
    {
        private readonly SqliteConnection connection;
        private readonly Action displayDataInListView;

        public EditProduct(SqliteConnection connection, Action displayDataInListView)
        {
            this.connection = connection;
            this.displayDataInListView = displayDataInListView;
        }

        public void editProduct()
        {
            using (Form searchForm = new Form())
            {
                searchForm.Text = "Edytuj Produkt";
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

                    ShowEditProductForm(kodProduktu);
                }
            }
        }

        private void ShowEditProductForm(string kodProduktu)
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
                    string currentKod = reader["Kod"].ToString();
                    string currentNazwa = reader["Nazwa"].ToString();
                    int currentIlosc = Convert.ToInt32(reader["Ilość"]);
                    decimal currentCena = Convert.ToDecimal(reader["Cena"]);

                    reader.Close();

                    using (Form editForm = new Form())
                    {
                        editForm.Text = "Edytuj Produkt";
                        editForm.Size = new Size(300, 300);

                        Label labelKod = new Label() { Text = "Kod produktu:", Location = new Point(10, 20), AutoSize = true };
                        TextBox textBoxKod = new TextBox() { Location = new Point(120, 20), Width = 150, Text = currentKod };

                        Label labelNazwa = new Label() { Text = "Nazwa produktu:", Location = new Point(10, 60), AutoSize = true };
                        TextBox textBoxNazwa = new TextBox() { Location = new Point(120, 60), Width = 150, Text = currentNazwa };

                        Label labelIlosc = new Label() { Text = "Ilość:", Location = new Point(10, 100), AutoSize = true };
                        TextBox textBoxIlosc = new TextBox() { Location = new Point(120, 100), Width = 150, Text = currentIlosc.ToString() };

                        Label labelCena = new Label() { Text = "Cena:", Location = new Point(10, 140), AutoSize = true };
                        TextBox textBoxCena = new TextBox() { Location = new Point(120, 140), Width = 150, Text = currentCena.ToString("F2") };

                        Button buttonSave = new Button() { Text = "Zapisz", Location = new Point(50, 200), DialogResult = DialogResult.OK };
                        Button buttonCancel = new Button() { Text = "Anuluj", Location = new Point(150, 200), DialogResult = DialogResult.Cancel };

                        editForm.Controls.Add(labelKod);
                        editForm.Controls.Add(textBoxKod);
                        editForm.Controls.Add(labelNazwa);
                        editForm.Controls.Add(textBoxNazwa);
                        editForm.Controls.Add(labelIlosc);
                        editForm.Controls.Add(textBoxIlosc);
                        editForm.Controls.Add(labelCena);
                        editForm.Controls.Add(textBoxCena);
                        editForm.Controls.Add(buttonSave);
                        editForm.Controls.Add(buttonCancel);

                        if (editForm.ShowDialog() == DialogResult.OK)
                        {
                            string newKod = textBoxKod.Text.Trim();
                            string newNazwa = textBoxNazwa.Text.Trim();
                            if (!int.TryParse(textBoxIlosc.Text.Trim(), out int newIlosc) || newIlosc < 0 ||
                                !decimal.TryParse(textBoxCena.Text.Trim().Replace('.', ','), out decimal newCena) || newCena <= 0)
                            {
                                MessageBox.Show("Wprowadź poprawne dane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            newCena = Math.Round(newCena, 2);
                            UpdateProductInDatabase(kodProduktu, newKod, newNazwa, newIlosc, newCena);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nie znaleziono produktu o podanym kodzie.", "Brak Produktu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy edytowaniu produktu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateProductInDatabase(string oldKod, string newKod, string newNazwa, int newIlosc, decimal newCena)
        {
            try
            {
                string updateQuery = "UPDATE Produkty SET Kod = @NewKod, Nazwa = @NewNazwa, Ilość = @NewIlosc, Cena = @NewCena WHERE Kod = @OldKod";
                SqliteCommand updateCmd = new SqliteCommand(updateQuery, connection);
                updateCmd.Parameters.AddWithValue("@NewKod", newKod);
                updateCmd.Parameters.AddWithValue("@NewNazwa", newNazwa);
                updateCmd.Parameters.AddWithValue("@NewIlosc", newIlosc);
                updateCmd.Parameters.AddWithValue("@NewCena", newCena);
                updateCmd.Parameters.AddWithValue("@OldKod", oldKod);

                updateCmd.ExecuteNonQuery();
                displayDataInListView();
                MessageBox.Show("Produkt został zaktualizowany.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy aktualizowaniu produktu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
